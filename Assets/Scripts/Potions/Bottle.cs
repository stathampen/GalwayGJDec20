using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Potion [] potionArray;

	public GameObject bottleModel;

	public Material currentMaterial;

	public Rigidbody body;

	public void GrabBottle()
	{
		// todo something
		Debug.Log("We have grabbed the bottle");
	}


	public void SetPotion(int potionNumber)
	{
		//call this function from other scripts
		SetMaterial(potionArray[potionNumber].potionMaterial);
	}


	private void Start() {
		if(potionArray.Length > 0)
		{
			SetMaterial(potionArray[0].potionMaterial);
		}
	}

	private void Update() {

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
			/*Destroy(gameObject);\#1#
		}*/
	}

}

