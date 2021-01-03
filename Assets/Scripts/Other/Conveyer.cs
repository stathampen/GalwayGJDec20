using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
	[SerializeField] private float speedOfConveyer = default;
	private Vector3 _direction;
	private readonly List<Rigidbody> _bodiesToPush = new List<Rigidbody>(16);

	public bool canPutBottleOn;

	public void RemoveRigidbody(Bottle bottleToRemove)
	{
		var index = _bodiesToPush.FindIndex(rigidBody => rigidBody.GetInstanceID() ==
			bottleToRemove.body.GetInstanceID());

		if (index > -1)
		{
			var body = _bodiesToPush[index];
			body.velocity = Vector3.zero;
			_bodiesToPush.RemoveAt(index);
		}
	}

	private void Awake()
	{
		_direction = transform.forward;
	}

	private void FixedUpdate()
	{
		var pushForce = _direction * (speedOfConveyer * Time.fixedDeltaTime);
		var bodiesToPushCopy = new List<Rigidbody>(_bodiesToPush);
		foreach (var body in bodiesToPushCopy)
		{
			if (body && !body.IsSleeping())
			{

				if (!body.gameObject.activeSelf)
				{
					Debug.Log("removing body we no longer need to push because it is invalid");
					_bodiesToPush.Remove(body);
				}
				var bottle = body.gameObject.GetComponent<Bottle>();
				if (bottle.CurrentConveyor == gameObject.GetInstanceID())
				{
					body.MovePosition(body.position + pushForce);
				}
			}
			else
			{
				Debug.Log("removing body we no longer need to push");
				_bodiesToPush.Remove(body);
				if (body)
				{
					var bottle = body.gameObject.GetComponent<Bottle>();
					if (bottle.CurrentConveyor == gameObject.GetInstanceID())
					{
						bottle.SetConveyor(-1);
					}
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();
		if (bottle)
		{
			_bodiesToPush.Add(bottle.body);
			if (bottle.CurrentConveyor < 0)
			{
				bottle.SetConveyor(gameObject.GetInstanceID());
			}

			if (bottle.CurrentConveyor != gameObject.GetInstanceID())
			{
				bottle.SetConveyor(gameObject.GetInstanceID());
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();
		if (bottle)
		{
			RemoveRigidbody(bottle);
		}
	}
}
