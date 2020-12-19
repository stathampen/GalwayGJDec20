using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Potion [] potions;

	public GameObject bottleModel;

	public Material currentMaterial;

	public Rigidbody body;

	private LevelController _levelController;
	[SerializeField] private GameObject explosionPrefab;

	private Transform _transformToFollow;

	public Potion Potion
	{
		get;
		private set;
	}

	public void Grab(Transform transformToFollow)
	{
		body.position = transformToFollow.position;
		body.rotation = Quaternion.identity;
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
		body.rotation = Quaternion.identity;
		body.position = dropPosition;
	}

	public void Init()
	{
		this._levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
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

	public void BreakBottle(bool dropped)
	{
		//if the user has dropped the bottle of mixed the wrong potion it should explode
		var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation).GetComponent<Explosion>();
		explosion.Explode();

		_levelController.FailedPotion();

		gameObject.SetActive(false);
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
		// perhaps this should check if it hits the floor instead
		var tag = other.gameObject.tag;
		if (tag == "Floor")
		{
			BreakBottle(gameObject);
		}
	}
}

