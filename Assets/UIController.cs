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

    // private void Awake() {
    //     MakePanels();
    // }

    public void UpdatePanels()
    {
        PopulateUI();
    }

    public void MakePanels()
    {
        panelArray = new List<GameObject>();
        var bottles = GameObject.FindGameObjectsWithTag("PotionBottle");

		foreach (var bottle in bottles)
		{
			bottle.SetActive(false);
			Destroy(bottle);
		}

        int potionNo = levelController.GetListPotions();

        float panelHeight = potionPanelPrefab.GetComponent<RectTransform>().rect.height;

        //put the number of failures at the bottom of the ui
        GameObject failPanel = Instantiate(potionPanelPrefab,
            new Vector3(potionPanelPos.transform.position.x, potionPanelPos.transform.position.y, potionPanelPos.transform.position.z),
            gameObject.transform.rotation,                                                                              //gives the panels a little gap between each
            gameObject.transform);

            failPanel.GetComponent<PotionPanelController>().setDisplayBottleText(
                "Misses Allowed"
            );

            panelArray.Add(failPanel);

        for(int i = 0; i < potionNo; i++)
        {
            GameObject potionPanel = Instantiate(potionPanelPrefab,
            new Vector3(potionPanelPos.transform.position.x, potionPanelPos.transform.position.y + ((panelHeight + 30) * (i + 1)), potionPanelPos.transform.position.z),
            gameObject.transform.rotation,                                                                              //gives the panels a little gap between each
            gameObject.transform);

            panelArray.Add(potionPanel);
        }

        PopulateUI();
    }

    private void PopulateUI()
    {

        panelArray[0].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingMisses()
            );

        for (int i = 1; i < panelArray.Count; i++)
        {
            Debug.Log("panel " + i);
            Debug.Log("panel Name " + i);
            panelArray[i].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingPotions(i-1)
            );

            Debug.Log("Count " + i);
            panelArray[i].GetComponent<PotionPanelController>().setDisplayBottleText(
                levelController.GetPotionName(i-1)
            );
        }
    }

}
