using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Potion [] potions;

	public GameObject bottleModel;

	public Material currentMaterial;

	public Rigidbody body;

	private BottleFailureCounter _bottleFailureCounter;

	private Transform _transformToFollow;

	public Potion Potion
	{
		get;
		private set;
	}

	public void Grab(Transform transformToFollow)
	{
		body.position = transformToFollow.position;
		body.isKinematic = true;
		body.useGravity = false;
		body.velocity = Vector3.zero;
		_transformToFollow = transformToFollow;
		body.mass = 0;
		body.detectCollisions = false;
		body.Sleep();
	}

	public void Drop(Vector3 dropPosition)
	{
		body.isKinematic = false;
		body.useGravity = true;
		Debug.Log("We have dropped the bottle");
		_transformToFollow = null;
		body.mass = 1;
		body.detectCollisions = true;
		body.WakeUp();
		body.position = dropPosition;
	}

	public void Init(BottleFailureCounter failureCounter)
	{
		_bottleFailureCounter = failureCounter;
	}

	public void SetPotion(int potionNumber)
	{
		//call this function from other scripts
		SetMaterial(potions[potionNumber].potionMaterial);
		Potion = potions[potionNumber];
	}

	public string GetPotion()
	{
		return Potion.potionName;
	}

	private void Start() {
		if(potions.Length > 0)
		{
			SetMaterial(potions[0].potionMaterial);
			Potion = potions[0];
		}
	}

	private void SetMaterial(Material newMaterial)
	{
		var meshRenderer = bottleModel.GetComponent<MeshRenderer>();
        // Get the current material applied on the GameObject
        // Set the new material on the GameObject
        meshRenderer.material = newMaterial;
        currentMaterial = meshRenderer.material;
	}

	private void Update()
	{
		if (_transformToFollow && body.isKinematic)
		{
			transform.position = _transformToFollow.position;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		/*// perhaps this should check if it hits the floor instead
		var floor = other.gameObject.GetComponent<Floor>();
		if (floor)
		{
			_bottleFailureCounter.AddFailure(this);
			Destroy(gameObject);
		}*/
	}
}

