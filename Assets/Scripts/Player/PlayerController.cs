using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float raycastRadius = 0.5f;
    [SerializeField] private Transform bottleHoldPoint;
    [SerializeField] private CapsuleCollider capsuleCollider;

    public Vector3 CapsuleCentre => capsuleCollider.center;
    public float CapsuleRadius => capsuleCollider.radius;
    public float CapsuleHeight => capsuleCollider.height;

    private readonly Collider[] _colliders = new Collider[16];

    public Bottle Bottle
    {
        get;
        private set;
    }

    private Vector3 _currentMovement = Vector3.zero;

    private void Update()
    {
        var buttonPressed = false;
        if (Input.GetKey(KeyCode.W))
        {
            buttonPressed = true;
            _currentMovement += transform.forward * baseSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            buttonPressed = true;
            _currentMovement += transform.forward * -baseSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            buttonPressed = true;
            _currentMovement += transform.right * baseSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            buttonPressed = true;
            _currentMovement += transform.right * -baseSpeed;
        }

        if (!buttonPressed)
        {
            _currentMovement = Vector3.zero;
        }

        _currentMovement = Vector3.ClampMagnitude(_currentMovement, baseSpeed * 2);

        GetBottleInput();

        MovementUpdate();
    }

    private void GetBottleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var position = transform.position + capsuleCollider.center;
            if (!Bottle)
            {
                var size = Physics.OverlapSphereNonAlloc(position, raycastRadius, _colliders);

                Bottle bottleToGrab = null;
                if (size > 0)
                {
                    Collider bottleCollider = null;
                    for (var i = 0; i < size; i++)
                    {
                        var col = _colliders[i];
                        if (col.GetComponent<Bottle>())
                        {
                            Debug.Log("detect bottle");
                            if (!bottleCollider)
                            {
                                Debug.Log("get bottle");
                                bottleToGrab = col.GetComponent<Bottle>();
                                bottleCollider = col;
                            }

                            if (bottleCollider &&
                                Vector3.Distance(position, bottleCollider.transform.position) >
                                Vector3.Distance(position, col.transform.position))
                            {
                                bottleToGrab = col.GetComponent<Bottle>();
                                bottleCollider = col;
                            }
                        }
                    }
                }

                if (bottleToGrab)
                {
                    // todo do something to bottle
                    // let's pass in a transform point to hold the bottle
                    bottleToGrab.Grab(bottleHoldPoint);
                    Bottle = bottleToGrab;
                }
            }
            else if (Bottle)
            {
                // find the nearest conveyor
                var size = Physics.OverlapSphereNonAlloc(position, raycastRadius, _colliders);

                Conveyer conveyerToUse = null;
                Collider convenyerCollider = null;
                if (size > 0)
                {
                    Debug.Log("number found: " + size);
                    for (var i = 0; i < size; i++)
                    {
                        var col = _colliders[i];

                        var conveyer = col.GetComponent<Conveyer>();
                        if (conveyer && conveyer.canPutBottleOn)
                        {
                            if (!convenyerCollider)
                            {
                                conveyerToUse = conveyer;
                                convenyerCollider = col;
                            }

                            if (convenyerCollider &&
                                Vector3.Distance(position, convenyerCollider.transform.position) >
                                Vector3.Distance(position, col.transform.position))
                            {
                                conveyerToUse = conveyer;
                                convenyerCollider = col;
                            }
                        }
                    }
                }

                if (conveyerToUse)
                {
                    var bottleToPlace = Bottle;
                    bottleToPlace.Drop(convenyerCollider.bounds.center +
                        new Vector3(0, convenyerCollider.bounds.center.y + 0.5f));
                    Bottle = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + capsuleCollider.center, raycastRadius);
    }

    // based on SuperCharacterCollider
    private void MovementUpdate()
    {
        // we're going to use a fixed time step, but via Update, so we can have more control
        var delta = Time.deltaTime;

        while (delta > Time.fixedDeltaTime)
        {
            SingleMovementStep();
            delta -= Time.fixedDeltaTime;
        }

        if (delta > 0f)
        {
            SingleMovementStep();
        }

        transform.position += _currentMovement * delta;
    }

    private void SingleMovementStep()
    {
        RecursivePushback(0, 2);
    }

    // modified from https://nightowl.games/blog/unity/custom-character-controller-in-unity/, SuperCharacterController and Unity documentation
    private void RecursivePushback(int depth, int maxDepth)
    {
        while (true)
        {
            var centerPosition = transform.position + capsuleCollider.center;
            var numOverlaps = Physics.OverlapCapsuleNonAlloc(centerPosition + (Vector3.up * -capsuleCollider.height / 2),
                centerPosition + new Vector3(0, capsuleCollider.height / 2),
                capsuleCollider.radius,
                _colliders);

            var contact = false;

            for (var i = 0; i < numOverlaps; i++)
            {
                var overlappedCollider = _colliders[i];

                if (capsuleCollider != overlappedCollider && ClosestPointOnSurface(overlappedCollider, centerPosition, out var contactPoint))
                {
                    var pushbackVector = contactPoint - centerPosition;

                    if (pushbackVector != Vector3.zero)
                    {
                        if (Physics.SphereCast(new Ray(centerPosition, pushbackVector.normalized),
                            0.01f,
                            pushbackVector.magnitude + 0.01f))
                        {
                            if (Vector3.Distance(centerPosition, contactPoint) < capsuleCollider.radius)
                            {
                                pushbackVector = pushbackVector.normalized * ((capsuleCollider.radius - pushbackVector.magnitude) * -1);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            pushbackVector = pushbackVector.normalized *
                                (capsuleCollider.radius + pushbackVector.magnitude);
                        }

                        contact = true;

                        transform.position += pushbackVector;
                    }
                }
            }

            if (depth < maxDepth && contact)
            {
                depth = depth + 1;
                continue;
            }

            break;
        }
    }

    private bool ClosestPointOnSurface(Collider collider, Vector3 toPoint, out Vector3 contactPoint)
    {
        if (collider is BoxCollider)
        {
            var boxCollider = (BoxCollider) collider;
            var colliderTransform = boxCollider.transform;

            var localPoint = colliderTransform.InverseTransformPoint(toPoint) - boxCollider.center;

            var halfSizeOfBox = boxCollider.size / 2;

            var localNormals = new Vector3(Mathf.Clamp(localPoint.x, -halfSizeOfBox.x, halfSizeOfBox.x),
                Mathf.Clamp(localPoint.y, -halfSizeOfBox.y, halfSizeOfBox.y),
                Mathf.Clamp(localPoint.z, -halfSizeOfBox.z, halfSizeOfBox.z));

            var distanceX = Mathf.Min(Mathf.Abs(halfSizeOfBox.x - localNormals.x),
                Mathf.Abs(-halfSizeOfBox.x - localNormals.x));
            var distanceY = Mathf.Min(Mathf.Abs(halfSizeOfBox.y - localNormals.y),
                Mathf.Abs(-halfSizeOfBox.y - localNormals.y));
            var distanceZ = Mathf.Min(Mathf.Abs(halfSizeOfBox.z - localNormals.z),
                Mathf.Abs(-halfSizeOfBox.z - localNormals.z));

            if (distanceX < distanceY && distanceX < distanceZ)
            {
                localNormals.x = Mathf.Sign(localNormals.x) * halfSizeOfBox.x;
            }
            else if (distanceX < distanceY && distanceX < distanceZ)
            {
                localNormals.x = Mathf.Sign(localNormals.x) * halfSizeOfBox.x;
            }
            else if (distanceX < distanceY && distanceX < distanceZ)
            {
                localNormals.x = Mathf.Sign(localNormals.x) * halfSizeOfBox.x;
            }

            localNormals += boxCollider.center;

            contactPoint = colliderTransform.TransformPoint(localNormals);

            return true;
        }
        contactPoint = Vector3.zero;
        return false;
    }

    /*
    [CustomEditor(typeof(PlayerController))]
    public class DrawCapsule : Editor
    {
        private void OnSceneGUI()
        {
            var obj = (PlayerController) target;
            // drawing wire script from https://forum.unity.com/threads/drawing-capsule-gizmo.354634/
            var angleMatrix = Matrix4x4.TRS(obj.transform.position + obj.CapsuleCentre, obj.transform.rotation, Handles.matrix.lossyScale);
            Handles.color = Color.blue;
            using (new Handles.DrawingScope(angleMatrix))
            {
                var radius = obj.CapsuleRadius;
                var pointOffset = (obj.CapsuleHeight - (radius * 2)) / 2;
                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
            }
        }
    }*/
}
