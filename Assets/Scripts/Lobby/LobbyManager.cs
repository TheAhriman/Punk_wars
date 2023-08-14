using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI createRoomInput;
    [SerializeField] private TextMeshProUGUI joinRoomInput;
    private int maxPlayers = 4;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions() { MaxPlayers = maxPlayers });
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
