using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float atkCooldown = 0.1f;
    [SerializeField] float bulletHeightFromFeet = 1f;
    [SerializeField] float dashCooldown = 0.3f;
    [SerializeField] float dashStrength = 20;
    [SerializeField] float dashTime = 0.3f;
    [SerializeField] float playableAreaMin = -10f;
    [SerializeField] float playableAreaMax = 10f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float cursorPlaneY;
    [SerializeField] Animator animator;

    Rigidbody rb;
    TrailRenderer trailRenderer;
    PlayerInput input;
    Vector3 mouseWorldPoint;
    float lastAtkTime = float.MinValue;
    float lastDashTime = float.MinValue;
    Vector3 moveDir = Vector3.zero;
    Vector3 dashDir = Vector3.zero;
    Plane cursorPlane;
    bool isInverseMovementControls = false;
    Entity entity;
    bool inputEnabled;
    int animatorVelocityXHash, animatorVelocityZHash;
    public bool IsDead => entity.Health <= 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        entity = GetComponent<Entity>();

        animatorVelocityXHash = Animator.StringToHash("VelocityX");
        animatorVelocityZHash = Animator.StringToHash("VelocityZ");
    }

    void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();
        input.Player.Enable();
        inputEnabled = true;

        cursorPlane = new Plane(Vector3.up, -cursorPlaneY);
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Disable();
        inputEnabled = false;
    }

    void Update()
    {
        RaycastMouse();
        OrientForward();
        Move();
        Dash();
        ClampToPlayableArea();
        Attack();
        Trail();
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        float animatorVelocityX = animator.GetFloat(animatorVelocityXHash);
        float animatorVelocityZ = animator.GetFloat(animatorVelocityZHash);
        Vector3 rotatedMovement = transform.rotation * rb.velocity;
        animator.SetFloat(animatorVelocityXHash, Mathf.MoveTowards(animatorVelocityX, rotatedMovement.x / moveSpeed, (1 / 0.2f) * Time.deltaTime));
        animator.SetFloat(animatorVelocityZHash, Mathf.MoveTowards(animatorVelocityZ, rotatedMovement.z / moveSpeed, (1 / 0.2f) * Time.deltaTime));
    }

    private void RaycastMouse()
    {
        if (inputEnabled)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (cursorPlane.Raycast(mouseRay, out float enter))
            {
                mouseWorldPoint = mouseRay.GetPoint(enter);
            }
        }
    }
    
    private void OrientForward()
    {
        transform.forward = Vector3.ProjectOnPlane(mouseWorldPoint - transform.position, Vector3.up).normalized;
    }

    private void Move()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        if (isInverseMovementControls)
        {
            moveDir = new Vector3(-moveInput.x, 0, -moveInput.y);
        } 
        else
        {
            moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        }
        moveDir.Normalize();
        rb.velocity = Vector3.ClampMagnitude(rb.velocity + moveDir * moveSpeed, Mathf.Max(rb.velocity.magnitude, moveSpeed));
    }

    private void Dash()
    {
        if (input.Player.Dash.IsPressed() && Time.time > lastDashTime + dashCooldown)
        {
            lastDashTime = Time.time;
            rb.velocity += moveDir * dashStrength;
        }
    }

    private void ClampToPlayableArea()
    {
        if (transform.position.x < playableAreaMin)
        {
            transform.position = new Vector3(playableAreaMin, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > playableAreaMax)
        {
            transform.position = new Vector3(playableAreaMax, transform.position.y, transform.position.z);
        }
    }

    private void Attack()
    {
        if (input.Player.Attack.IsPressed() && Time.time > lastAtkTime + atkCooldown)
        {
            lastAtkTime = Time.time;
            GameObject obj = Instantiate(bulletPrefab);
            obj.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + bulletHeightFromFeet,
                transform.position.z
            );
            obj.GetComponent<Rigidbody>().velocity = transform.forward;
        }
    }

    void Trail()
    {
        if (rb.velocity.magnitude > moveSpeed + 1f)
        {
            trailRenderer.emitting = true;
        } 
        else
        {
            trailRenderer.emitting = false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        Gizmos.DrawSphere(mouseWorldPoint, 1f);
    //    }
    //}

    public void TransitionGameState()
    {
        isInverseMovementControls = true;
    }

    public void DisableInput()
    {
        input.Disable();
        inputEnabled = false;
    }

    public void EnableInput()
    {
        input.Enable();
        inputEnabled = true;
    }
}
