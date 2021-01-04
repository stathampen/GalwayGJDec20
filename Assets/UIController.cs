using System.Collections.Generic;
using UnityEngine;

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
        //the actual panels in the scene which need to be deleted on each level load
        var panels = GameObject.FindGameObjectsWithTag("PotionPanel");
		foreach (var panel in panels)
		{
			panel.SetActive(false);
			Destroy(panel);
		}

        //just the array to keep track of the potion banels
        panelArray = new List<GameObject>();

        int potionNo = levelController.GetListPotions();

        var panelHeight = potionPanelPrefab.GetComponent<RectTransform>().rect.height;

        //put the number of failures at the bottom of the ui
        var failPanel = Instantiate(potionPanelPrefab,
            new Vector3(potionPanelPos.transform.position.x, potionPanelPos.transform.position.y, potionPanelPos.transform.position.z),
            gameObject.transform.rotation,                                                                              //gives the panels a little gap between each
            gameObject.transform);

            failPanel.GetComponent<PotionPanelController>().SetDisplayBottleText(
                "Misses Allowed"
            );

            panelArray.Add(failPanel);

        for(var i = 0; i < potionNo; i++)
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

        panelArray[0].GetComponent<PotionPanelController>().SetDisplayText(
                levelController.GetRemainingMisses()
            );

        for (var i = 1; i < panelArray.Count; i++)
        {
            panelArray[i].GetComponent<PotionPanelController>().SetDisplayText(
                levelController.GetRemainingPotions(i-1)
            );

            panelArray[i].GetComponent<PotionPanelController>().SetDisplayBottleText(
                levelController.GetPotionName(i-1)
            );
        }
    }

}
