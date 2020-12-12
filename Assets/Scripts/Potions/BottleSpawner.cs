using UnityEngine;

public class BottleSpawner : MonoBehaviour
{
	[SerializeField] private GameObject bottlePrefab;
	[SerializeField] private float timeBetweenSpawnsSecs;
	[SerializeField] private Transform spawnTransform;
	[SerializeField] private BottleFailureCounter bottleFailureCounter;

	private float _timer;

	public bool CanSpawnBottles
	{
		get;
		set;
	}

	private void Update()
	{
		if (CanSpawnBottles)
		{
			_timer += Time.deltaTime;

			if (_timer >= timeBetweenSpawnsSecs)
			{
				var bottle = Instantiate(bottlePrefab, spawnTransform.position, spawnTransform.rotation);
				bottle.GetComponent<Bottle>().Init(bottleFailureCounter);
				_timer = 0.0f;
			}
		}
	}
}
