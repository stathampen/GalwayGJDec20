using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float raycastRadius = 0.5f;
    [SerializeField] private Transform bottleHoldPoint;
    [SerializeField] private CapsuleCollider capsuleCollider;

    public float CapsuleRadius => capsuleCollider.radius;
    public float CapsuleHeight => capsuleCollider.height;

    private readonly Collider[] _colliders = new Collider[16];

    private readonly List<float> _sphereOffsets = new List<float>();

    public Bottle Bottle
    {
        get;
        private set;
    }

    private Vector3 _movement = Vector3.zero;

    void Awake()
    {
        var numCapsulesNeeded = Mathf.RoundToInt(CapsuleHeight / CapsuleRadius);
        numCapsulesNeeded = numCapsulesNeeded > 1 ? numCapsulesNeeded - 1 : numCapsulesNeeded;

        var currentOffset = 0.0f;

        for (var i = 0; i < numCapsulesNeeded; i++)
        {
            currentOffset += CapsuleRadius;
            _sphereOffsets.Add(currentOffset);
        }

        capsuleCollider.enabled = false;
    }

    private void Update()
    {
        var buttonPressed = false;
        var moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            buttonPressed = true;
            moveDirection += transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            buttonPressed = true;
            moveDirection += -transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            buttonPressed = true;
            moveDirection += transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            buttonPressed = true;
            moveDirection += -transform.right;
        }

        if (!buttonPressed)
        {
            moveDirection = Vector3.zero;
        }

        GetBottleInput();

        MovementUpdate(moveDirection.normalized);
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
                            } else if (Vector3.Distance(position, convenyerCollider.transform.position) >
                                Vector3.Distance(position, col.transform.position))
                            {
                                conveyerToUse = conveyer;
                                convenyerCollider = col;
                            }
                        }
                        // bottle stops placement
                        else if (col.GetComponent<Bottle>())
                        {
                            conveyerToUse = null;
                            break;
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
    private void MovementUpdate(Vector3 moveDirection)
    {
        // we're going to use a fixed time step, but via Update, so we can have more control
        var delta = Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            if (Math.Abs(Mathf.Sign(_movement.x) - Mathf.Sign(moveDirection.x)) > 0.01f)
            {
                _movement.x = 0;
            }

            if (Math.Abs(Mathf.Sign(_movement.z) - Mathf.Sign(moveDirection.z)) > 0.01f)
            {
                _movement.z = 0;
            }
            _movement = Vector3.MoveTowards(_movement, moveDirection * baseSpeed, (baseSpeed) * delta);
        }
        else
        {
            _movement = Vector3.zero;
        }

        transform.position += (_movement) * delta;

        while (delta > Time.fixedDeltaTime)
        {
            SingleMovementStep();
            delta -= Time.fixedDeltaTime;
        }

        if (delta > 0f)
        {
            SingleMovementStep();
        }
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
            var contact = false;
            foreach (var offset in _sphereOffsets)
            {
                var spherePosition = transform.position + offset * transform.up;
                var numOverlaps = Physics.OverlapSphereNonAlloc(spherePosition, CapsuleRadius, _colliders);

                for (var i = 0; i < numOverlaps; i++)
                {
                    var overlappedCollider = _colliders[i];

                    if (capsuleCollider == overlappedCollider ||
                        !ClosestPointOnSurface(overlappedCollider, spherePosition, out var contactPoint)) continue;
                    var pushbackVector = contactPoint - spherePosition;

                    if (pushbackVector == Vector3.zero) continue;
                    if (Physics.SphereCast(new Ray(spherePosition, pushbackVector.normalized),
                        0.01f,
                        pushbackVector.magnitude + 0.01f))
                    {
                        if (Vector3.Distance(spherePosition, contactPoint) < CapsuleRadius)
                        {
                            pushbackVector = pushbackVector.normalized * ((CapsuleRadius - pushbackVector.magnitude) * -1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        pushbackVector = pushbackVector.normalized *
                            (CapsuleRadius + pushbackVector.magnitude);
                    }

                    contact = true;

                    if (pushbackVector.y > 0)
                    {
                        pushbackVector.y = 0;
                    }

                    transform.position += pushbackVector;
                }
            }

            if (transform.position.y != 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }


            if (depth < maxDepth && contact)
            {
                depth++;
                continue;
            }

            break;
        }
    }

    private bool ClosestPointOnSurface(Collider collider, Vector3 toPoint, out Vector3 contactPoint)
    {
        if (collider is BoxCollider boxCollider)
        {
            var colliderTransform = boxCollider.transform;

            var localPoint = colliderTransform.InverseTransformPoint(toPoint) - boxCollider.center;

            var halfSizeOfBox = boxCollider.size * 0.5f;

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
            else if (distanceY < distanceX && distanceY < distanceZ)
            {
                localNormals.y = Mathf.Sign(localNormals.y) * halfSizeOfBox.y;
            }
            else if (distanceZ < distanceX && distanceZ < distanceY)
            {
                localNormals.z = Mathf.Sign(localNormals.z) * halfSizeOfBox.z;
            }

            localNormals += boxCollider.center;

            contactPoint = colliderTransform.TransformPoint(localNormals);

            return true;
        }


        contactPoint = Vector3.zero;
        return false;
    }
}
