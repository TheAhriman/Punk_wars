
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }


    [SerializeField] CoinsSpawner coinsSpawner;

    [Header("Players Spawn")]
    [SerializeField] List<GameObject> playerPrefabs;
    [SerializeField] List<Transform> playerSpawns;

    [Header("UI")]
    [SerializeField] UIController uiController;
    [SerializeField] Button shootButton;
    [SerializeField] Slider coinsBar;

    [Header("Coins")]
    private int playerCount;
    [SerializeField] int coinsToSpawnCount = 10;
    public static UIController UIController => Instance.uiController;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SpawnCoins();
        SpawnPlayer();
    } 
    private void SpawnCoins()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            coinsSpawner.SpawnCoin(coinsToSpawnCount);
        }
        coinsBar.maxValue = coinsToSpawnCount;
    }
    private void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        Quaternion rotation = playerPrefabs[playerIndex].transform.rotation;
        GameObject player = PhotonNetwork.Instantiate(playerPrefabs[playerIndex].name, playerSpawns[playerIndex].position, rotation);
        shootButton.onClick.AddListener(player.GetComponent<PlayerMove>().Shoot);
        uiController.DisableControls();
    }
    [PunRPC]
    public void UpdatePlayersCount_RPC()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount > 1)
        {
            uiController.EnableControls();
        }
    }
    [PunRPC]
    private void OnPlayerDies_RPC()
    {
        playerCount--;
        if (playerCount == 1)
        {
            GameObject winner = GameObject.FindGameObjectWithTag("Player");
            string winnerName = winner.name;
            int winnerCoins = winner.GetComponent<Character>().Coins;
            uiController.OpenPopUp(winnerName, winnerCoins);
        }
    }
    public void OnPlayerDies()
    {
        photonView.RPC(nameof(OnPlayerDies_RPC), RpcTarget.All);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("UpdatePlayersCount_RPC", RpcTarget.AllBuffered);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}