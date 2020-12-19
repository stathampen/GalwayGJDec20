using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitChute : MonoBehaviour
{

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
            if(other.gameObject.tag == "PotionBottle")
            {
                var bottle = other.GetComponent<Bottle>();

                //pass on up to the level controller to decide if the potion is the correct one
                levelController.CheckPotion(bottle.Potion.name);

                //unload the bottle as it's no longer needed
                Destroy(bottle);
            }
        }
    }

}
