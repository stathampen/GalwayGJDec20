using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class BottleSuccessCounter : MonoBehaviour
{
	[SerializeField] private int numberToBeat;
	[SerializeField] private LevelController levelController;
	[SerializeField] private BottleFailureCounter bottleFailureCounter;
	[SerializeField] private Potion[] typesWanted;

	private int _currentCount;
	private void OnCollisionEnter(Collision other)
	{
		var potion = other.collider.GetComponent<PotionBehaviour>();

		if (potion)
		{
			var bottle = potion.GetComponent<Bottle>();
			if (bottle.potions.Length != typesWanted.Length)
			{
				bottleFailureCounter.AddFailure(bottle);
			}
			else
			{
				var correctTypes = 0;
				for (var i = 0; i < typesWanted.Length; i++)
				{
					if (typesWanted[i].name == bottle.potions[i].name)
					{
						correctTypes++;
					}
				}

				if (correctTypes == typesWanted.Length)
				{
					_currentCount++;
				}
				else
				{
					bottleFailureCounter.AddFailure(bottle);
				}
			}
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
