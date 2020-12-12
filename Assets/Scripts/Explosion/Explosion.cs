using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Explosion : MonoBehaviour
{
	[SerializeField] private ParticleSystem particleSystem;

	public void Explode()
	{
		particleSystem.Play();
		Destroy(gameObject, particleSystem.main.duration);
	}
}
