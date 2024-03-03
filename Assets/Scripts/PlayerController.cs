using System.Collections;
using System.Collections.Generic;
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

    Rigidbody rb;
    PlayerInput input;
    Vector3 mouseWorldPoint;
    bool isGrounded = true;
    Vector3 lastXZMoveDir;
    bool toJump = false;
    float lastjumpTime = float.MinValue;

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
        // raycast from cursor to ground for aim position
        //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit, 50f, mask, QueryTriggerInteraction.UseGlobal);
        //mouseWorldPoint = hit.point;

        //// set forward to look at the cursor
        //Vector3 newForward = mouseWorldPoint - transform.position;
        //newForward.y = 0;
        //newForward.Normalize();
        //// transform.forward = newForward;

        // move with input
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        moveDir.Normalize();
        transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;

        // jump
        if (input.Player.Dash.IsPressed() && isGrounded && !toJump && Time.time > lastjumpTime + jumpCooldown)
        {
            toJump = true;
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
