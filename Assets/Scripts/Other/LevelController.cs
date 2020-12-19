using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelRound [] levelRounds;
	private GameObject _currentLevel;

	private readonly List<BottleSpawner> _bottleSpawners = new List<BottleSpawner>();

	private int _currentRound;

	private int _currentFailsAllowed = 5;

	public void OnEnable()
	{
		foreach (var spawner in _bottleSpawners)
		{
			spawner.CanSpawnBottles = true;
		}
	}

	public void OnDisable()
	{
		foreach (var spawner in _bottleSpawners)
		{
			spawner.CanSpawnBottles = false;
		}
	}

	public void LoadLevel()
	{
		if (_currentLevel)
		{
			foreach (var spawner in _bottleSpawners)
			{
				spawner.CanSpawnBottles = false;
			}
			_bottleSpawners.Clear();
			_currentLevel.SetActive(false);
		}
		_currentLevel = Instantiate(levelRounds[_currentRound].prefab);
		_currentFailsAllowed = levelRounds[_currentRound].maxMissesCount;
		var spawnerObjects = GameObject.FindGameObjectsWithTag("BottleSpawner");

		Debug.Log("Found spawners: " + spawnerObjects.Length);

		foreach (var spawnerObject in spawnerObjects)
		{
			var spawner = spawnerObject.GetComponent<BottleSpawner>();
			spawner.CanSpawnBottles = true;
			_bottleSpawners.Add(spawner);
		}
	}

	//check the user has passed the right potion
	public void CheckPotion(string potionName)
	{
		//first check if the potion is one we want
		for (var i = 0; i < levelRounds[_currentRound].typesWanted.Length; i++)
		{
			//the user has passed a correct potion
			if (potionName == levelRounds[_currentRound].typesWanted[i].potionName)
			{
				//AND still want more of those potions
				if(levelRounds[_currentRound].typesWanted[i].potionCount > 0)
				{
					levelRounds[_currentRound].typesWanted[i].potionCount--;
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
		_currentFailsAllowed--;

		if(_currentFailsAllowed <= 0)
		{
			EndGame();
		}
	}

	public int GetRemainingPotions(int i)
	{
		Debug.Log(levelRounds[_currentRound].typesWanted[i].potionCount);
		return levelRounds[_currentRound].typesWanted[i].potionCount;
	}

	public int GetListPotions()
	{
		return levelRounds[_currentRound].typesWanted.Length;
	}

	// move to the next level
	private void AdvanceLevel()
	{
		Debug.Log("current round: " + _currentRound);

		//as long as the current round is less that the max number of rounds the game can continue
		if(_currentRound < levelRounds.Length - 1)
		{
			_currentRound++; //to the next round
			LoadLevel();
		}
		else
		{
			foreach (var spawner in _bottleSpawners)
			{
				spawner.CanSpawnBottles = false;
			}
			_bottleSpawners.Clear();
		}
	}

	private void EndGame()
	{
		foreach (var spawner in _bottleSpawners)
		{
			spawner.CanSpawnBottles = false;
		}

		Debug.Log("END GAME");
	}
}

