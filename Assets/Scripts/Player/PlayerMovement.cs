using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float groundMoveSpeed = 10f;
    [SerializeField] float airborneSlowFactor = 0.75f;
    [SerializeField] float jumpSpeed = 10f;
    private float speed;

    [Header("Jump")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    private bool isGrounded;

    [Header("Sword")]
    [SerializeField] Transform shoulderPivot;
    [SerializeField] float stationaryArmLength = 1.5f;
    [SerializeField] float movingArmLength = 1.75f;
    private float currentArmLength;
    [SerializeField] GameObject sword;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        shoulderPivot = sword.transform.parent.transform;
        currentArmLength = stationaryArmLength;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(-Vector2.left * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * speed);
        }
    }

    private void Update()
    {
        SetGroundedStatus();
        SetSpeed();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        SetSwordPosition();
        SetSwordAngle();
    }

    private void SetSpeed()
    {
        speed = isGrounded ? groundMoveSpeed : groundMoveSpeed * airborneSlowFactor;
    }

    private void SetGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetSwordAngle()
    {
        Vector3 relative = transform.InverseTransformPoint(sword.transform.position);
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        sword.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void SetSwordPosition()
    {
        AdjustArmLength();
        Vector3 shoulderToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shoulderPivot.position;
        shoulderToMouseDirection.z = 0;
        sword.transform.position = shoulderPivot.position + (currentArmLength * shoulderToMouseDirection.normalized);
    }

    private void AdjustArmLength()
    {
        if (rb.velocity.magnitude > 0.1 && currentArmLength < movingArmLength)
        {
            currentArmLength += Time.deltaTime;
        }
        if (rb.velocity.magnitude < 0.1 && currentArmLength > stationaryArmLength)
        {
            currentArmLength -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
