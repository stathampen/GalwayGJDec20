using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerScript : MonoBehaviour
{
    private Material potionMaterial;

    //when the potion enters the spot need to check the potions material
    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "PotionBottle")
        {
            GameObject potionBottle = other.gameObject;

            Debug.Log("This is a potion");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
