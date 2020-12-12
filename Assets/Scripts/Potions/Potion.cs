using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/PotionInstance")]
public class Potion : ScriptableObject
{
	public string potionName;
	public Material potionMaterial;
}
