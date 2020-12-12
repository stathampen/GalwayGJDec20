using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class BottleSuccessCounter : MonoBehaviour
{
	[SerializeField] private int numberToBeat;
	[SerializeField] private LevelController levelController;
	[SerializeField] private BottleFailureCounter bottleFailureCounter;
	[SerializeField] private Potion typesWanted;

	private int _currentCount;
	private void OnCollisionEnter(Collision other)
	{
		var bottle = other.collider.GetComponent<Bottle>();
		if (bottle.Potion.name != typesWanted.name)
		{
			bottleFailureCounter.AddFailure(bottle);
		}
		else
		{
			_currentCount++;
		}

		Destroy(bottle.gameObject);

		if (_currentCount == numberToBeat)
		{
			// todo end level etc.
			_currentCount = 0;
			levelController.EndLevel(true);
		}
	}
}
