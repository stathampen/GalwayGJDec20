using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public GameObject potionPanelPrefab;
    public GameObject potionPanelPos;

    public LevelController levelController;
    public List<GameObject>  panelArray;

    //get how many potions are still wanted in the level

    private void Start() {
        MakePanels();
    }
    
    public void MakePanels ()
    {
        int potionNo = levelController.GetListPotions();

        for(int i = 0; i < potionNo; i++)
        {
            GameObject Panel = Instantiate(potionPanelPrefab, 
            new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z),
            gameObject.transform.rotation,
            gameObject.transform);

            panelArray.Add(Panel);
        }

        for (int i = 0; i < panelArray.Count; i++)
        {
            panelArray[i].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingPotions(i)
            );
        }
    }
    
}
