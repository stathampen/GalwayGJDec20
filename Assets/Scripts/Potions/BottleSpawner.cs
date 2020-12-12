using UnityEngine;

public class BottleSpawner : MonoBehaviour
{
	[SerializeField] private GameObject bottlePrefab;
	[SerializeField] private float timeBetweenSpawnsSecs;
	[SerializeField] private Transform spawnTransform;
	[SerializeField] private LevelController levelController;

	private float _timer;

	public bool CanSpawnBottles
	{
		get;
		set;
	}

	private void Awake() {
		try {
			levelController = GameObject.Find("levelController").GetComponent<LevelController>();
		}
		catch
		{
			Debug.Log("MISSING LEVEL CONTROLLER");
		}
	}

	private void Update()
	{
		if (CanSpawnBottles)
		{
			_timer += Time.deltaTime;

			if (_timer >= timeBetweenSpawnsSecs)
			{
				var bottle = Instantiate(bottlePrefab, spawnTransform.position, spawnTransform.rotation);

				if(levelController)
				{
					bottle.GetComponent<Bottle>().Init(levelController);
					_timer = 0.0f;
				}
				else
				{
					Debug.Log("MISSING LEVEL CONTROLLER");
				}
			}
		}
	}
}
