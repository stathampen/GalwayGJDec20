using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
	[SerializeField] private float speedOfConveyer = default;
	private Vector3 _direction;
	private readonly List<Rigidbody> _bodiesToPush = new List<Rigidbody>(16);

	public void RemoveRigidbody(Bottle bottleToRemove)
	{
		var index = _bodiesToPush.FindIndex(rigidBody => rigidBody.GetInstanceID() ==
			bottleToRemove.body.GetInstanceID());

		if (index > -1)
		{
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
			body.MovePosition(body.position + pushForce);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();

		if (bottle)
		{
			Debug.Log("adding body");
			_bodiesToPush.Add(bottle.body);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();
		if (bottle)
		{
			Debug.Log("removing body");
			RemoveRigidbody(bottle);
		}
	}
}
