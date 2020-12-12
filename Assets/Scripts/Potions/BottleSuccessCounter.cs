using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class BottleExitCounter : MonoBehaviour
{
	[SerializeField] private int numberToBeat;
	[SerializeField] private LevelController levelController;

	private int _currentCount = 0;
	private void OnCollisionEnter(Collision other)
	{
		var potion = other.collider.GetComponent<PotionBehaviour>();

		if (potion)
		{
			_currentCount++;
			Destroy(potion.gameObject);
		}

		if (_currentCount == numberToBeat)
		{
			// todo end level etc.
			_currentCount = 0;
			levelController.EndLevel(true);
		}
	}
}
