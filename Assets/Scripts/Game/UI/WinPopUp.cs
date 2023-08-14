using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WinPopUp : MonoBehaviour
{
    private TextMeshProUGUI winText;

    private void Awake()
    {
        winText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetText(string winnerName, int coins)
    {
        winnerName = winnerName.Substring(0, winnerName.Length - 7);
        winText.text = $"{winnerName} win. The player has collected {coins} coins";
    }

}
