using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float raycastRadius = 0.5f;

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
            _currentMovement.z = baseSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _currentMovement.z = -baseSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _currentMovement.x = baseSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _currentMovement.x = -baseSpeed;
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
            Collider bottleCollider = null;
            if (size > 0)
            {
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
                Bottle = bottleToGrab;
                bottleToGrab.GrabBottle();
            }

            _pressedE = false;
        }
        else if (_pressedE && Bottle)
        {
            // todo play sound or something
        }
        body.MovePosition(transform.position + (_currentMovement * Time.fixedDeltaTime));
        _currentMovement = Vector3.zero;
    }
}
