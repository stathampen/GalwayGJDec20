using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionPanelController : MonoBehaviour
{
    public TextMeshProUGUI countText;

    public Bottle displayBottle;

    public void setDisplayText(int pointCount)
    {
        countText.text = pointCount.ToString();
    }

    public void setDisplayBottle()
    {
        Debug.Log("hitting this");
        displayBottle.SetPotion(3);
    }

}
