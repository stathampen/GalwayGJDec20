using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float raycastRadius = 0.5f;
    [SerializeField] private Transform bottleHoldPoint;

    private bool _pressedE;

    public Bottle Bottle
    {
        get;
        private set;
    }

    private Vector3 _currentMovement = Vector3.zero;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _currentMovement.x = baseSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _currentMovement.x = -baseSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _currentMovement.z = -baseSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _currentMovement.z = baseSpeed;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("pressed e");
            _pressedE = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raycastRadius);
    }

    private void FixedUpdate()
    {
        if (_pressedE && !Bottle)
        {
            var colliders = new Collider[16];
            var size = Physics.OverlapSphereNonAlloc(transform.position, raycastRadius, colliders);

            Bottle bottleToGrab = null;
            if (size > 0)
            {
                Collider bottleCollider = null;
                for (var i = 0; i < size; i++)
                {
                    var col = colliders[i];
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
                            Vector3.Distance(transform.position, bottleCollider.transform.position) >
                            Vector3.Distance(transform.position, col.transform.position))
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

            _pressedE = false;
        }
        else if (_pressedE && Bottle)
        {
            // find the nearest conveyor
            var colliders = new Collider[16];
            var size = Physics.OverlapSphereNonAlloc(transform.position, raycastRadius, colliders);

            Conveyer conveyerToUse = null;
            Collider convenyerCollider = null;
            if (size > 0)
            {
                Debug.Log("number found: " + size);
                for (var i = 0; i < size; i++)
                {
                    var col = colliders[i];

                    var conveyer = col.GetComponent<Conveyer>();
                    if (conveyer)
                    {
                        if (!convenyerCollider)
                        {
                            conveyerToUse = conveyer;
                            convenyerCollider = col;
                        }

                        if (convenyerCollider &&
                            Vector3.Distance(transform.position, convenyerCollider.transform.position) >
                            Vector3.Distance(transform.position, col.transform.position))
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
                bottleToPlace.Drop(convenyerCollider.bounds.center + new Vector3(0, convenyerCollider.bounds.center.y + 0.4f));
                Bottle = null;
            }

            _pressedE = false;
        }
        body.MovePosition(transform.position + (_currentMovement * Time.fixedDeltaTime));
        _currentMovement = Vector3.zero;
    }
}
