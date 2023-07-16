using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Hit : MonoBehaviour
{
    private Transform owner;
    private int damage;
    private Collider hitColider;
    private Rigidbody rb;

    private Animator anim;

    private void Awake()
    {
        owner=transform.root;
        hitColider=GetComponent<BoxCollider>();
        rb=GetComponent<Rigidbody>();   
        hitColider.isTrigger = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        hitColider.enabled = false; 
    }
    private void Start()
    {
        if (owner.tag == "Player")
        {
            damage= owner.GetComponent<AttackController>().GetDamage();
            anim = GetComponentInParent<Transform>().GetComponentInParent<Animator>() ;
        }
        else if (owner.tag == "Enemy")
        {
            damage=owner.GetComponent<EnemyController>().GetDamage();
        }
    }

    private void Update()
    {
        if (!anim.IsInTransition(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.5f&&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.55f)
        {
            ControlTheColider(true);
            print("Colider Open");
        }
        else
        {
            ControlTheColider(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Health health=other.GetComponent<Health>(); 
        if (health != null&&health.gameObject!=owner.gameObject) 
        {
            health.GiveDamage(damage);
        }
    }
    private void ControlTheColider(bool open)
    {
        hitColider.enabled = open;
    }
}
