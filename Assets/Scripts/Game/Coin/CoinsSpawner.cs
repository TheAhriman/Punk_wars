using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] Coin coinPrefab;
    [SerializeField] float width;
    [SerializeField] float height;
    public void SpawnCoin(int times)
    {
        for (int i = 0; i < times; i++)
        {
            PhotonNetwork.InstantiateRoomObject(coinPrefab.name, GetRandomSpawnPosition(), Quaternion.identity);
        }
    }
    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;

        bool positionFound = false;

        while (!positionFound)
        {
            randomPosition = new Vector3(
                Random.Range(-width, width),
                Random.Range(-height, height),
                0
            );

            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 0.1f);

            if (colliders.Length == 0)
            {
                positionFound = true;
            }
        }
        return randomPosition;
    }
}
