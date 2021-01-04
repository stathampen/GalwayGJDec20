using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionPanelController : MonoBehaviour
{
    public TextMeshProUGUI countText;
    public TextMeshProUGUI bottleText;

    public void SetDisplayText(int potionCount)
    {
        countText.text = potionCount + " X ";
    }

    public void SetDisplayBottleText(string potionName)
    {
        bottleText.text = potionName;
    }

}
