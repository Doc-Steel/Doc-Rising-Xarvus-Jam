using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolBehaviour : MonoBehaviour
{
    [Header("Enemy Patrol Stats")]
    public float patrolSpeed;
    public float distance;

    private bool movingRight = true;

    [Header("Enemy Patrol Reference")]
    public Transform detector;


    private void Update()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);

        RaycastHit2D raycastInfo = Physics2D.Raycast(detector.position, Vector2.down, distance);

        if(raycastInfo.collider == false)
        {
            if(movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            } else
            {

                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}
