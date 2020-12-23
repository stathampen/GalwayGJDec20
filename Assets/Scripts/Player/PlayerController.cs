using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private Rigidbody body;
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
                                bottleToGrab =col.GetComponent<Bottle>();
                                bottleCollider = col;
                            }

                            if (bottleCollider &&
                                Vector3.Distance(position, bottleCollider.transform.position) >
                                Vector3.Distance(position, col.transform.position))
                            {
                                bottleToGrab =col.GetComponent<Bottle>();
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

    // modified from https://nightowl.games/blog/unity/custom-character-controller-in-unity/ and Unity documentation
    private void FixedUpdate()
    {
        var numOverlaps = Physics.OverlapCapsuleNonAlloc(
            transform.position + capsuleCollider.center + (Vector3.up * -capsuleCollider.height / 2),
            transform.position + capsuleCollider.center + new Vector3(0, capsuleCollider.height / 2),
            capsuleCollider.radius,
            _colliders);

        for (var i = 0; i < numOverlaps; i++)
        {
            var overlappedCollider = _colliders[i];
            if (Physics.ComputePenetration(capsuleCollider,
                transform.position,
                transform.rotation,
                overlappedCollider,
                overlappedCollider.transform.position,
                overlappedCollider.transform.rotation,
                out var direction,
                out var distance) && capsuleCollider != overlappedCollider)
            {
                var penetrationVector = direction * distance;
                var velocityProjection = Vector3.Project(_currentMovement, -direction);
                transform.position += penetrationVector;
                _currentMovement -= velocityProjection;
                Debug.Log("penetrated collider: " + overlappedCollider + ", name: "
                    + overlappedCollider.gameObject.name + ", distance: " + distance + ", direction: " + direction);
            }
        }
        transform.position += (_currentMovement * Time.deltaTime);
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
