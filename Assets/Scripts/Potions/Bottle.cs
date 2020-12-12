using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Rigidbody body;

	private void OnCollisionEnter(Collision other)
	{
		var conveyer = other.gameObject.GetComponent<Conveyer>();
		// todo also check if colliding with player
		if (!conveyer)
		{
			Destroy(this);
		}
	}
}

