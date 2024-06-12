using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

public class TPSMovement_Test : MonoBehaviour
{
    private Rigidbody rd;
    private Animator anim;
    
    public float speed = 4;

    private Vector3 lookPos;

    private Transform cam;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;

    private float forwardAmount;
    private float turnAmount;
    
    private void Start()
    {
        rd = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        cam = Camera.main.transform;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = vertical * camForward + horizontal * cam.right;
        }
        else
        {
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        Move(move);
        
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        
        rd.AddForce(movement * speed / Time.deltaTime);

    }

    private void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.moveInput = move;
        
        ConvertMoveInput();
        UpdateAnimator();
    }

    private void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);

        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("MoveX", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("MoveY", turnAmount, 0.1f, Time.deltaTime);
    }
}
