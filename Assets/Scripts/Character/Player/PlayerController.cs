using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravityModifer = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;
    [SerializeField] private InputAction newMovmentInput;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensivity = 1f;
    [SerializeField] float maxViewAngle = 60f;
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    private CharacterController characterController;

    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;


    private Vector3 heightMovement;

    private bool jump = false;

    private Transform mainCamera;

    void Awake()
    {
        characterController=GetComponent<CharacterController>();
        if (Camera.main.GetComponent<CameraController>()==null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        mainCamera= GameObject.FindWithTag("CameraPoint").transform;
    }
    private void OnEnable()
    {
        newMovmentInput.Enable();
    }
    private void OnDisable()
    {
        newMovmentInput.Disable();
    }

    void Update()
    {
        KeyboardInput();
    }

  
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y+MouseInput().x,
            transform.eulerAngles.z);

        if (mainCamera!=null)
        {
            if (mainCamera.eulerAngles.x>maxViewAngle&&mainCamera.eulerAngles.x<180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle,mainCamera.eulerAngles.y,mainCamera.eulerAngles.z);
            }
            else if (mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f- maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles +
                      new Vector3(-MouseInput().y, 0f, 0f));
            }
        }
    }

    private void Move()
    {

        if (jump)
        {
            heightMovement.y = jumpPower;
            jump = false;
        }

        heightMovement.y-=gravityModifer*Time.deltaTime;

        Vector3 localVerticalVector = transform.forward * verticalInput; // baðtýðýn yöne gidebilsin diye vertical ve horizontali böyle yazdým
        Vector3 localHorizontalVector = transform.right * horizontalInput;

        Vector3 movmentVector = localHorizontalVector + localVerticalVector;
        movmentVector.Normalize();              // normal giderken hýz 8 sað çapraza giderken 8.4 olmasýn diye yazdým sebebi vectorel
        movmentVector *= currentSpeed * Time.deltaTime;
        characterController.Move(movmentVector+heightMovement);

        if (characterController.isGrounded)
        {
            heightMovement.y = 0f;
        }
    }

    private void KeyboardInput()
    {
        horizontalInput = newMovmentInput.ReadValue<Vector2>().x;
        verticalInput = newMovmentInput.ReadValue<Vector2>().y;

        if (Keyboard.current.spaceKey.wasPressedThisFrame&&characterController.isGrounded)
        {
            jump = true;
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
    private Vector2 MouseInput()
    {
        return new Vector2(invertX ? -Mouse.current.delta.x.ReadValue() : Mouse.current.delta.x.ReadValue(),
            invertY ? -Mouse.current.delta.y.ReadValue() : Mouse.current.delta.y.ReadValue()) *mouseSensivity; 
    }

}
