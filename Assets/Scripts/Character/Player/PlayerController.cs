using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;

    private CharacterController characterController;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;
    void Start()
    {
        characterController=GetComponent<CharacterController>();
    }

    void Update()
    {
        OnKeyboardInput();
    }

  
    private void FixedUpdate()
    {
        Vector3 localVerticalVector = transform.forward * verticalInput; // baðtýðýn yöne gidebilsin diye vertical ve horizontali böyle yazdým
        Vector3 localHorizontalVector=transform.right * horizontalInput;

        Vector3 movmentVector = localHorizontalVector + localVerticalVector;
        movmentVector.Normalize();              // normal giderken hýz 8 sað çapraza giderken 8.4 olmasýn diye yazdým sebebi vectorel
        movmentVector*=currentSpeed*Time.deltaTime;
        characterController.Move(movmentVector); 
    }

    private void OnKeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

}
