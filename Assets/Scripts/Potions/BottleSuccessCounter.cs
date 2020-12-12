using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class BottleSuccessCounter : MonoBehaviour
{
	[SerializeField] private LevelController levelController;

	private int _currentCount;
	private void OnCollisionEnter(Collision other)
	{
		var bottle = other.collider.GetComponent<Bottle>();

		levelController.checkPotion(bottle.Potion.name);
	}
}
