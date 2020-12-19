using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private BottleSpawner bottleSpawner;
	[SerializeField] private LevelRound [] levelRounds;
	private GameObject _currentLevel;

	private int currentRound = 0;

	public void OnEnable()
	{
		bottleSpawner.CanSpawnBottles = true;
	}

	public void OnDisable()
	{
		bottleSpawner.CanSpawnBottles = false;
	}

	public void LoadLevel()
	{
		if (_currentLevel)
		{
			_currentLevel.SetActive(false);
		}
		_currentLevel = Instantiate(levelRounds[currentRound].prefab);
	}

	//check the user has pased the right potion
	public void CheckPotion(string potionName)
	{
		//first check if the potion is one we want
		for (var i = 0; i < levelRounds[currentRound].typesWanted.Length; i++)
		{
			//the user has passed a correct potion
			if (potionName == levelRounds[currentRound].typesWanted[i].potionName)
			{
				//AND still want more of those potions
				if(levelRounds[currentRound].typesWanted[i].potionCount > 0)
				{
					levelRounds[currentRound].typesWanted[i].potionCount--;
				}
				else
				{
					// advance the level routine
					AdvanceLevel();
				}
			}
			else
			{
				FailedPotion();
			}
		}
	}

	//either wrong or broken potion
	public void FailedPotion()
	{
		levelRounds[currentRound].maxMissesCount--;

		if(levelRounds[currentRound].maxMissesCount <= 0)
		{
			EndGame();
		}
	}

	// move to the next level
	private void AdvanceLevel()
	{
		Debug.Log("current round: " + currentRound);

		//as long as the current round is less that the max number of rounds the game can continue
		if(currentRound < levelRounds.Length)
		{
			currentRound++; //to the next round
			LoadLevel();
		}
		else
		{
			//need to do a better way than killing the programme
			Application.Quit();
		}
	}

	private void EndGame()
	{
		bottleSpawner.CanSpawnBottles = false;

		Debug.Log("END GAME");
	}
}

