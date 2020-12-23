using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionPanelController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI bottleText;

    public Bottle displayBottle;

    public void setDisplayText(int _potionCount)
    {
        countText.text = _potionCount.ToString() + " X ";
    }

    public void setDisplayBottleText(string _potionName)
    {
        bottleText.text = _potionName;
    }

}
