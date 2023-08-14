using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int maxHp = 5;
    private int currentHp;
    [SerializeField] private UnityEvent<float,float> OnHpChange;

    private int coins = 0;
    private Slider coinsBar;
    public int Coins
    {
        get { return coins; }
        private set { coins = value; }
    }
    public int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            OnHpChange.Invoke(currentHp, maxHp);
            if (currentHp <= 0)
            {
                Die();
            }
        }
    }
    private void Awake()
    {
        if (photonView.IsMine)
        {
            Slider[] sliders = FindObjectsOfType<Slider>();
            foreach (Slider sl in sliders)
            {
                if (sl.GetComponent<HealthBar>() == null)
                {
                    coinsBar = sl;
                    break;
                }
            }
        }       
    }
    private void Start()
    {
        CurrentHp = maxHp;
    }
    public void PickUpCoin()
    {
        if (photonView.IsMine)
        {
            Coins++;
            coinsBar.value = Coins;
        } 
    }

    [PunRPC]
    public void TakeDamage_RPC()
    {
        if (photonView.IsMine)
        {
            CurrentHp--;
        }
    }

    public void TakeDamage()
    {
        photonView.RPC(nameof(TakeDamage_RPC), RpcTarget.AllBuffered);
    }
    private void Die()
    { 
        if (photonView.IsMine)
        {
            GetComponentInChildren<Weapon>().gameObject.GetComponentInChildren<Animator>().SetBool("Dead",true);
            GetComponent<Animator>().SetBool("Dead", true);
            GameManager.UIController.DisableControls();
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.OnPlayerDies();
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHp);
            stream.SendNext(coins);
        }
        else
        { 
            currentHp = (int)stream.ReceiveNext();
            coins = (int)stream.ReceiveNext();
            if (currentHp <= 0)
            {
                Die();
            }
            
        }
    }
    public void DestroyPlayer()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}