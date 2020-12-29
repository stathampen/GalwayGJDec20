using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionPanelController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI bottleText;

    public void setDisplayText(int _potionCount)
    {
        countText.text = _potionCount + " X ";
    }

    public void setDisplayBottleText(string _potionName)
    {
        bottleText.text = _potionName;
    }

}
