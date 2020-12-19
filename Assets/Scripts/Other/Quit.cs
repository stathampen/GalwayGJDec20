using UnityEngine;

public class Quit : MonoBehaviour
{
	public void QuitGame()
	{
		if (!Application.isEditor)
		{
			Application.Quit();
		}
	}
}

