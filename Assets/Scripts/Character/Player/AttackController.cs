using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeapon;

    private Transform mainCamera;

    private Animator anim;
    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
        anim=mainCamera.transform.GetChild(0).GetComponent<Animator>();
        SpawnWeapon();

    }
    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (Mouse.current.leftButton.isPressed) 
        {
            anim.SetTrigger("Attack");
        }
    }

    private void SpawnWeapon()
    {
        if (currentWeapon== null)
        {
            return;
        }
        currentWeapon.SpawnNewWeapon(mainCamera.transform.GetChild(0).GetChild(0));
    }
}
