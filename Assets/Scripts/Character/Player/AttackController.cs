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

    private bool isAttacking = false;
    private void Start()
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
        if (Mouse.current.leftButton.isPressed&&!isAttacking) 
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void SpawnWeapon()
    {
        if (currentWeapon== null|| anim == null)
        {
            return;
        }
        currentWeapon.SpawnNewWeapon(mainCamera.transform.GetChild(0).GetChild(0),anim);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentWeapon.AttackRate);
        isAttacking= false;
    }
}
