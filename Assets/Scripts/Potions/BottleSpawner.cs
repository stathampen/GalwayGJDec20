using UnityEngine;

public class BottleSpawner : MonoBehaviour
{
	[SerializeField] private GameObject bottlePrefab;
	[SerializeField] private float timeBetweenSpawnsSecs;
	[SerializeField] private Transform spawnTransform;

	public AudioSource audioSource;

	private float _timer;

	public bool CanSpawnBottles
	{
		get;
		set;
	}

	private void Awake()
	{
		_timer = timeBetweenSpawnsSecs;
	}

	private void Update()
	{
		if (CanSpawnBottles)
		{
			_timer += Time.deltaTime;

			if (_timer >= timeBetweenSpawnsSecs)
			{

				audioSource.Play();
				var bottle = Instantiate(bottlePrefab, spawnTransform.position, spawnTransform.rotation);
				bottle.GetComponent<Bottle>().Init();

				_timer = 0.0f;
			}
		}
	}
}
