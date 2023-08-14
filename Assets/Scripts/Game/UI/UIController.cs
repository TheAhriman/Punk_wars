using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviourPun
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Button shootButton;
    [SerializeField] private WinPopUp popUpMessage;

    public void DisableControls()
    {
        joystick.enabled = false;
        shootButton.enabled = false;
    }
    public void EnableControls()
    {
        joystick.enabled = true;
        shootButton.enabled = true;
    }
    public void OpenPopUp(string winnerName,int coins)
    {
        DisableControls();
        popUpMessage.gameObject.SetActive(true);
        popUpMessage.SetText(winnerName, coins);
    }
}
