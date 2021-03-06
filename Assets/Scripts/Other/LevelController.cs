using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelRound [] levelRounds;
	[SerializeField] private GameObject playerObject;
	[SerializeField] private UIController playerUI;
	[SerializeField] private UiTransitionController transitionController;
	[SerializeField] private GameObject gameOverMenu;

	private GameObject _currentLevel;

	private readonly List<BottleSpawner> _bottleSpawners = new List<BottleSpawner>();

	private int _currentRound;

	private int _currentFailsAllowed = 5;

	private readonly int[] _currentSuccessesAllowed = new int[10];

	private bool[] _successTable = new bool[10];

	private float _fixedDeltaTime;

	void Awake()
	{
		_fixedDeltaTime = Time.fixedDeltaTime;
	}

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

		for (var i = 0; i < levelRounds[_currentRound].typesWanted.Length; i++)
		{
			_currentSuccessesAllowed[i] = levelRounds[_currentRound].typesWanted[i].potionCount;
		}
		_successTable = new bool[levelRounds[_currentRound].typesWanted.Length];

		for (var i = 0; i < _successTable.Length; i++)
		{
			_successTable[i] = false;
		}

		var panels = GameObject.FindGameObjectsWithTag("PotionPanel");

		foreach (var panel in panels)
		{
			panel.SetActive(false);
			Destroy(panel);
		}

		playerUI.MakePanels();



		var bottles = GameObject.FindGameObjectsWithTag("PotionBottle");

		foreach (var bottle in bottles)
		{
			bottle.SetActive(false);
			Destroy(bottle);
		}

		var spawnerObjects = GameObject.FindGameObjectsWithTag("BottleSpawner");

		//move the player back to the starting spot, which is defined in the prefab for the level layouts as PlayerStart with the PlayerSpawner tag
		playerObject.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawner").transform.position;

		//remove all the bottles currently loaded from the previous level

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
		var breakLoop = false;
		for (var i = 0; i < levelRounds[_currentRound].typesWanted.Length; i++)
		{
			//the user has passed a correct potion
			if (!breakLoop)
			{
				if (potionName == levelRounds[_currentRound].typesWanted[i].potionName)
				{
					//AND still want more of those potions
					if (_currentSuccessesAllowed[i] > 0)
					{
						_currentSuccessesAllowed[i]--;

						playerUI.UpdatePanels();

						if(_currentSuccessesAllowed[i] == 0)
						{
							// advance the level routine
							_successTable[i] = true;
						}

						breakLoop = true;
					}
					else
					{
						FailedPotion();
						breakLoop = true;

					}
				}
			}
		}

		if (!breakLoop)
		{
			FailedPotion();
		}

		foreach (var success in _successTable)
		{
			if (!success)
			{
				return;
			}
		}

		AdvanceLevel();
	}

	//either wrong or broken potion
	public void FailedPotion()
	{
		_currentFailsAllowed--;

		playerUI.UpdatePanels();

		if(_currentFailsAllowed <= 0)
		{
			GameOver();
		}
	}

	public string GetPotionName(int i)
	{
		return levelRounds[_currentRound].typesWanted[i].potionName;
	}

	public int GetRemainingPotions(int i)
	{
		return _currentSuccessesAllowed[i];
	}

	public int GetListPotions()
	{
		return levelRounds[_currentRound].typesWanted.Length;
	}

	public int GetRemainingMisses()
	{
		return _currentFailsAllowed;
	}

	// move to the next level
	private void AdvanceLevel()
	{
		//as long as the current round is less that the max number of rounds the game can continue
		if(_currentRound < levelRounds.Length - 1)
		{
			_currentRound++; //to the next round
			DoTransition();
		}
		else
		{
			GameOver();
		}
	}

	private void DoTransition()
	{
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		transitionController.BeginTransition(LoadLevel, () =>
		{
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		});
	}

	private void GameOver()
	{
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		foreach (var spawner in _bottleSpawners)
		{
			spawner.CanSpawnBottles = false;
		}
		gameOverMenu.SetActive(true);
	}

	public void EndGame()
	{
		gameOverMenu.SetActive(false);
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
		Debug.Log("END GAME");
		SceneManager.LoadScene("FinalScene");
	}
}
