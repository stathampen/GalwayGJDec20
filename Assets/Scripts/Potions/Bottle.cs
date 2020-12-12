using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour
{
	public Potion [] potion;

	public GameObject bottleModel;

	public Material currentMaterial;

	private void Start() {
		if(potion.Length > 0)
		{		Debug.Log("Checiking Material");
			setMaterial(potion[0].potionMaterial);
		}
	}

	private void Update() {
		
	}

	private void setMaterial(Material newMaterial)
	{
		Debug.Log("setting Material");
		MeshRenderer meshRenderer = bottleModel.GetComponent<MeshRenderer>();
        // Get the current material applied on the GameObject
        Material oldMaterial = meshRenderer.material;
        // Set the new material on the GameObject
        meshRenderer.material = newMaterial;
	}

	
}

