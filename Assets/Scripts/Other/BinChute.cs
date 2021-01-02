using UnityEngine;

public class BinChute : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	private void OnTriggerEnter(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();

		if (bottle)
		{
			if (audioSource)
			{
				audioSource.Play();
			}

			Destroy(bottle.gameObject);
		}
	}
}
