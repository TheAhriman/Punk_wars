using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviourPun
{
    private Joystick joystick;
    private Rigidbody2D rb;
    private Weapon weapon;
    private Animator bodyAnimator;

    [SerializeField] private int speed = 1;
    [SerializeField] private bool isRight = true;
    public Vector2 playerDirection;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        bodyAnimator = GetComponent<Animator>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        playerDirection = new Vector2(1, 0);   
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        MovePlayer();
        MovePlayerName();
        RotateWeapon();
    }

    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private Vector3 offsetPlayerName;
    private void MovePlayerName()
    {
        PlayerName.transform.position = Camera.main.WorldToScreenPoint(transform.position + offsetPlayerName);
    }
    private void MovePlayer()
    {
        Vector2 direction = joystick.Direction;
        rb.velocity = direction * speed;
        if (direction.x < 0 && isRight || direction.x>0 && !isRight)
        {
            transform.Rotate(0,180,0);
            isRight = !isRight;
        }
        if (direction != new Vector2(0, 0))
        {
            playerDirection = direction;
            bodyAnimator.SetBool("Walk", true);
        }
        else bodyAnimator.SetBool("Walk", false);
    }

    public void Shoot()
    {
        GetComponentInChildren<Weapon>().Shoot(playerDirection, photonView);
    }
    private void RotateWeapon()
    {
        if (joystick.Direction != new Vector2(0, 0))
        {
            float angle = Mathf.Atan2(joystick.Direction.y, joystick.Direction.x) * Mathf.Rad2Deg;
            if (joystick.Direction.x < 0)
            {
                weapon.transform.rotation = Quaternion.Euler(180, 0, -angle);
            }
            else weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
