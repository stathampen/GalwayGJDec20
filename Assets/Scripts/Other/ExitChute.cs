using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChute : MonoBehaviour
{
    public AudioSource audioSource;
	private LevelController levelController;

    private void Start() {
        try {
			levelController = GameObject.Find("The God Monobehaviour").GetComponent<LevelController>();
		}
		catch
		{
			Debug.Log("EXIT CHUTE MISSING LEVEL CONTROLLER");
		}
    }

    private void OnTriggerEnter(Collider other) {
        if(levelController)
        {
            var bottle = other.GetComponent<Bottle>();

            if (bottle)
            {
	            //pass on up to the level controller to decide if the potion is the correct one
	            levelController.CheckPotion(bottle.Potion.name);

                audioSource.Play();

	            //unload the bottle as it's no longer needed
	            Destroy(bottle.gameObject);
            }
        }
    }

}
