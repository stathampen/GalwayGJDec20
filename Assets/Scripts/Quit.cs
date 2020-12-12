using UnityEngine;

public class Quit : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Application.isEditor)
		{
			Application.Quit();
		}
	}

	public void QuitGame()
	{
		if (!Application.isEditor)
		{
			Application.Quit();
		}
	}
}

