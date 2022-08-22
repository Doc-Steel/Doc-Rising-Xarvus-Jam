using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 10f;
    [SerializeField] float armLength = 1f;
    [SerializeField] Transform shoulderPivot;
    [SerializeField] GameObject mirror;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shoulderPivot = mirror.transform.parent.transform;
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
        
        SetMirrorPosition();
        SetMirrorAngle();
    }

    private void SetMirrorAngle()
    {
        Vector3 relative = transform.InverseTransformPoint(mirror.transform.position);
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        mirror.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void SetMirrorPosition()
    {
        Vector3 shoulderToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shoulderPivot.position;
        shoulderToMouseDirection.z = 0;
        mirror.transform.position = shoulderPivot.position + (armLength * shoulderToMouseDirection.normalized);
    }
}
