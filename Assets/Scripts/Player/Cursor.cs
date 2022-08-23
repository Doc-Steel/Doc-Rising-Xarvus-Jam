using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private bool canMove = true;
    private void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }
    private void Update()
    {
        if (!canMove) { return; }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canMove = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canMove = true;
    }
}
