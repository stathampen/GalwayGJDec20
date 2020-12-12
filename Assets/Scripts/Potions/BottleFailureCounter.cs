using UnityEngine;

public class BottleFailureCounter : MonoBehaviour
{

	[SerializeField] private int numberOfFailuresAllowed;
	[SerializeField] private LevelController levelController;

	private int _currentFailures;

	public void AddFailure(Bottle smashedBottle)
	{
		_currentFailures++;

		if (_currentFailures == numberOfFailuresAllowed)
		{
			// todo pause, end game
			_currentFailures = 0;
			levelController.EndLevel(false);
		}
	}
}

