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
        if (currentWeapon!=null)
        {
            SpawnWeapon();

        }
        

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

    public void EquipWeapon(WeaponData weaponType)
    {
        if (currentWeapon!=null)
        {
            currentWeapon.Drop();
        }
        currentWeapon = weaponType;
        SpawnWeapon();
    }
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentWeapon.AttackRate);
        isAttacking= false;
    }

    public int GetDamage()
    {
        if (currentWeapon!=null)
        {
           return currentWeapon.Damage;
        }
        return 0;
    }
}
