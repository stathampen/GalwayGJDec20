using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private BottleSpawner bottleSpawner;

	public void OnEnable()
	{
		bottleSpawner.CanSpawnBottles = true;
	}

	public void OnDisable()
	{
		bottleSpawner.CanSpawnBottles = false;
	}

	public void EndLevel(bool isSuccess)
	{
		bottleSpawner.CanSpawnBottles = false;
		if (isSuccess)
		{
			// good stuff
		}
		else
		{
			// bad stuff
		}
	}
}

