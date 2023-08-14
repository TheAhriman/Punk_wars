using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private PhotonView shooterPhotonView;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
   }
    public void Launch(Vector2 direction)
    {
        rb.velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();
        if (targetPhotonView == shooterPhotonView) return;
        if (targetPhotonView != null && targetPhotonView != shooterPhotonView)
        {
            Character targetCharacter = collision.gameObject.GetComponent<Character>();
            if (targetCharacter != null)
            {
                targetCharacter.TakeDamage();
            }
        }
        PhotonNetwork.Destroy(gameObject);

    }
    public void SetShooter(PhotonView photonView)
    {
        shooterPhotonView = photonView;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}

