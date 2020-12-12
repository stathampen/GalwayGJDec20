using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PotionFiller : MonoBehaviour
{
	[SerializeField] private Potion potion;
	private void OnTriggerEnter(Collider other)
	{
		var bottle = other.GetComponent<Bottle>();
		if (bottle)
		{
			Array.Resize(ref bottle.potions, bottle.potions.Length + 1);
			bottle.potions[bottle.potions.Length - 1] = potion;
			bottle.SetPotion(bottle.potions.Length - 1);
			var potionBehaviour = bottle.gameObject.AddComponent<PotionBehaviour>();
			potionBehaviour.potionMaterials = new Material[bottle.potions.Length];
			for (var i = 0; i < bottle.potions.Length; i++)
			{
				potionBehaviour.potionMaterials[i] = bottle.potions[i].potionMaterial;
			}
		}
	}
}
