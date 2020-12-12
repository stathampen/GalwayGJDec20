using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Potion [] potions;

	public GameObject bottleModel;

	public Material currentMaterial;

	public Rigidbody body;

	private LevelController levelController;

	public Potion Potion
	{
		get;
		private set;
	}

	public void GrabBottle()
	{
		// todo something
		Debug.Log("We have grabbed the bottle");
	}

	public void Init(LevelController levelController)
	{
		this.levelController = levelController;
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
        // todo do something with this?
        var oldMaterial = meshRenderer.material;
        // Set the new material on the GameObject
        meshRenderer.material = newMaterial;
        currentMaterial = meshRenderer.material;
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

