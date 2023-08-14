using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private float speed = 10;
    [SerializeField] Transform bulletSpawn;
    public void Shoot(Vector2 direction, PhotonView shooterPhotonView)
    {
        Bullet bullet = PhotonNetwork.Instantiate(bulletPrefab.name,bulletSpawn.position,Quaternion.identity).GetComponent<Bullet>();
        bullet.Launch(direction.normalized * speed);
        bullet.SetShooter(shooterPhotonView);
    }
}
