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
    private void Update() {
        PopulateUI();
    }

    public void MakePanels()
    {
        int potionNo = levelController.GetListPotions();

        float panelHeight = potionPanelPrefab.GetComponent<RectTransform>().rect.height;

        for(int i = 0; i < potionNo; i++)
        {
            GameObject Panel = Instantiate(potionPanelPrefab,
            new Vector3(potionPanelPos.transform.position.x, potionPanelPos.transform.position.y + ( (panelHeight + panelHeight/2) * i), potionPanelPos.transform.position.z),
    gameObject.transform.rotation,                                                                              //gives the panels a little gap between each
            gameObject.transform);

            panelArray.Add(Panel);
        }

        PopulateUI();
    }

    private void PopulateUI()
    {

        for (int i = 0; i < panelArray.Count; i++)
        {
            panelArray[i].GetComponent<PotionPanelController>().setDisplayText(
                levelController.GetRemainingPotions(i)
            );

            panelArray[i].GetComponent<PotionPanelController>().setDisplayBottleText(
                levelController.GetPotionName(i)
            );
        }
    }

}
