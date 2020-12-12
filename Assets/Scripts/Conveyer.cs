using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
	[SerializeField] private float speedOfConveyer = default;
	private Vector3 _direction;
	private readonly List<Rigidbody> _bodiesToPush = new List<Rigidbody>(16);

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
		var component = other.GetComponent<Rigidbody>();
		var bottle = other.GetComponent<Bottle>();

		if (component && bottle)
		{
			_bodiesToPush.Add(component);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var component = other.GetComponent<Rigidbody>();
		var bottle = other.GetComponent<Bottle>();
		if (component && bottle)
		{
			_bodiesToPush.Remove(component);
		}
	}
}
