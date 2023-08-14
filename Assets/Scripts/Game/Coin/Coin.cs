using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonNetwork.Destroy(gameObject);
        collision.gameObject.GetComponent<Character>().PickUpCoin();
    }
}
