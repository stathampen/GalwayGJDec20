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
			var success = false;
			for (var i = 0; i < bottle.potions.Length; i++)
			{
				if (potion.name == bottle.potions[i].name)
				{
					bottle.SetPotion(i);
					success = true;
				}
			}

			if (!success)
			{
				throw new Exception("Potion assigned here is not available in bottle prefab");
			}
		}
	}
}
