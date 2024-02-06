using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputSystem inputActions;
    Animator animator;
    Rigidbody rigid;

    /// <summary>
    /// 이동 방향(1 : 전진, 0, 정지, -1 : 후진)
    /// </summary>
    public float moveDirection = 0.0f;

    public float moveSpeed = 3.0f;

    public float rotateSpeed = 180.0f;

    public float jumpPower = 3.0f;

    /// <summary>
    /// 회전 방향(1 : 우회전, 0 : 정지, -1 : 좌회전)
    /// </summary>
    public float rotateDirection = 0.0f;

    bool isJump = false;

    readonly int IsMove = Animator.StringToHash("IsMove");
    private void Awake()
    {
        inputActions = new();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Use.performed += OnUse;
        inputActions.Player.Drop.performed += OnDrop;
            
    }


    private void OnDisable()
    {
        inputActions.Player.Drop.performed -= OnDrop;
        inputActions.Player.Use.performed -= OnUse;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }
    private void OnMove(InputAction.CallbackContext context) 
    {
        //Debug.Log("OnMove");
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        Debug.Log(isJump);
        Jump();
    }
    private void OnUse(InputAction.CallbackContext context)
    {
        Debug.Log("OnUse");
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        Debug.Log("OnDrop");
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDirection * transform.forward);
    }

    void Rotate()
    {
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection, transform.up);

        rigid.MoveRotation(rigid.rotation *  rotate);
    }

    void SetInput(Vector3 input, bool isMove)
    {
        moveDirection = input.x;
        rotateDirection = input.y;

        animator.SetBool(IsMove, isMove);
    }

    void Jump()
    {
        if (!isJump)
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            isJump = true;
        }
    }

}
