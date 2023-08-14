using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourPun
{
    private Slider healthBar;
    [SerializeField] Vector3 offset;
    private PlayerMove player;
    private void Awake()
    {
        player = GetComponentInParent<PlayerMove>();
        healthBar = GetComponent<Slider>();
    }
    [PunRPC]
    public void SetHealthOnBar_RPC(float currentHp, float maxHp)
    {
        healthBar.gameObject.SetActive(currentHp < maxHp);
        healthBar.value = currentHp;
        healthBar.maxValue = maxHp;
    }
    public void SetHealthOnBar(float currentHp, float maxHp)
    {
        photonView.RPC(nameof(SetHealthOnBar_RPC), RpcTarget.All,currentHp,maxHp);
    }
    private void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);
    }
}
