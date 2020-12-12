using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bottle : MonoBehaviour
{
	public Potion [] potionArray;

	public GameObject bottleModel;

	public Material currentMaterial;

	public Rigidbody body;


	public void setPotion(int potionNumber)
	{
		//call this function from other scripts
		setMaterial(potionArray[potionNumber].potionMaterial);
	}


	private void Start() {
		if(potionArray.Length > 0)
		{	
			setMaterial(potionArray[0].potionMaterial);
		}
	}

	private void Update() {

	}

	private void setMaterial(Material newMaterial)
	{
		MeshRenderer meshRenderer = bottleModel.GetComponent<MeshRenderer>();
        // Get the current material applied on the GameObject
        Material oldMaterial = meshRenderer.material;
        // Set the new material on the GameObject
        meshRenderer.material = newMaterial;
	}

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

