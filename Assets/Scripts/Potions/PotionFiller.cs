using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PotionFiller : MonoBehaviour
{
	[SerializeField] private Potion potion;
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "PotionBottle")
        {
			var bottle = other.GetComponent<Bottle>();

			if (bottle)
			{
				switch (bottle.GetPotion())
				{
					case "Empty":
						setNewPotion(bottle, potion.name);
					break;

					case "Blue":
						//currently Blue
						if(potion.name == "Green")
							setNewPotion(bottle, "Cyan");
						if(potion.name == "Red")
							setNewPotion(bottle, "Magenta");
						break;

					case "Red":
						//currently Red
						if(potion.name == "Green")
							setNewPotion(bottle, "Yellow");
						if(potion.name == "Blue")
							setNewPotion(bottle, "Magenta");
						break;

					case "Green":
						//currently Green
						if(potion.name == "Red")
							setNewPotion(bottle, "Yellow");
						if(potion.name == "Blue")
							setNewPotion(bottle, "Cyan");
						break;

					default:
						bottle.BreakBottle(false);
						break;
				}
			}
        }
	}

	private void setNewPotion(Bottle bottle, String newPotionName)
	{
		var success = false;
		for (var i = 0; i < bottle.potions.Length; i++)
		{
			if (newPotionName == bottle.potions[i].name)
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
