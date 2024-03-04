using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stoppingTime = 0.2f;
    [SerializeField] float jumpForceMultiplier = 10f;
    [SerializeField] float gravityMultiplier = 1f;
    [SerializeField] float jumpCooldown = 0.1f;
    [SerializeField] float atkCooldown = 0.1f;
    [SerializeField] float bulletHeightFromFeet = 1f;
    [SerializeField] float moveXMin = -10f;
    [SerializeField] float moveXMax = 10f;
    [SerializeField] GameObject bulletPrefab;

    Rigidbody rb;
    PlayerInput input;
    Vector3 mouseWorldPoint;
    bool isGrounded = true;
    Vector3 lastXZMoveDir;
    bool toJump = false;
    float lastjumpTime = float.MinValue;
    float lastAtkTime = float.MinValue;

    void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // move with input
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        moveDir.Normalize();
        transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
        if (transform.position.x < moveXMin)
        {
            transform.position = new Vector3(moveXMin, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > moveXMax)
        {
            transform.position = new Vector3(moveXMax, transform.position.y, transform.position.z);
        }

        // jump
        if (input.Player.Jump.IsPressed() && isGrounded && !toJump && Time.time > lastjumpTime + jumpCooldown)
        {
            toJump = true;
        }

        // attack
        if (input.Player.Attack.IsPressed() && Time.time > lastAtkTime + atkCooldown)
        {
            lastAtkTime = Time.time; 
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.position = new Vector3(
                transform.position.x, 
                transform.position.y + bulletHeightFromFeet, 
                transform.position.z
            );
        }
    }

    private void FixedUpdate()
    {
        if (toJump)
        {
            toJump = false;
            lastjumpTime = Time.time;
            rb.AddForce(Vector3.up * jumpForceMultiplier, ForceMode.Impulse);
        }

        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(mouseWorldPoint, 1f);
        }
    }
}
