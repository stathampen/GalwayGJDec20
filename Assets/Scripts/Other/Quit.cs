using UnityEngine;

public class Quit : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			QuitGame();
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

