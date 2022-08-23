using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 10f;

    [Header("Sword")]
    [SerializeField] Transform shoulderPivot;
    [SerializeField] float armLength = 1f;
    [SerializeField] GameObject sword;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shoulderPivot = sword.transform.parent.transform;
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
        
        SetSwordPosition();
        SetSwordAngle();
    }

    private void SetSwordAngle()
    {
        Vector3 relative = transform.InverseTransformPoint(sword.transform.position);
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        sword.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void SetSwordPosition()
    {
        Vector3 shoulderToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shoulderPivot.position;
        shoulderToMouseDirection.z = 0;
        sword.transform.position = shoulderPivot.position + (armLength * shoulderToMouseDirection.normalized);
    }
}
