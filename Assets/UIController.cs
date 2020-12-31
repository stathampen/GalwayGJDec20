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

    public void MakePanels()
    {
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

        for(int i = 1; i <= potionNo; i++)
        {
            GameObject potionPanel = Instantiate(potionPanelPrefab,
            new Vector3(potionPanelPos.transform.position.x, potionPanelPos.transform.position.y + ( (panelHeight) * i), potionPanelPos.transform.position.z),
            gameObject.transform.rotation,                                                                              //gives the panels a little gap between each
            gameObject.transform);

            panelArray.Add(potionPanel);
        }

        PopulateUI();
    }

    public void PopulateUI()
    {

        panelArray[0].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingMisses()
            );

        for (int i = 1; i < panelArray.Count; i++)
        {
            panelArray[i].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingPotions(i-1)
            );

            panelArray[i].GetComponent<PotionPanelController>().setDisplayBottleText(
                levelController.GetPotionName(i-1)
            );
        }
    }

    public void ResetUI()
    {

		var potionPanels = GameObject.FindGameObjectsWithTag("PotionPanel");

		foreach (var potionPanel in potionPanels)
		{
			potionPanel.SetActive(false);
			Destroy(potionPanel);
		}

        panelArray.Clear();

        Debug.Log("hello");

        //remake the ui
        MakePanels();

    }

}
