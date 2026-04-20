using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NPC_Diver : MonoBehaviour
{
    public List<Transform> points;
    public float speed = 1f;
    Transform targetPoint;
    bool lockMove = false;
    public Animator animator;
    bool left = true;
    bool right = false;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        targetPoint = points[0];
        rb = GetComponent<Rigidbody>();
        if (animator != null)
        {
            animator.SetBool("isWalkingLeft", left);
            animator.SetBool("isWalkingRight", right);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // isWalking is true
        if (!lockMove)
        {
            if (transform.position == targetPoint.position)
            {
                if (targetPoint == points[points.Count - 1])
                {
                    changeDirection();
                    targetPoint = points[0];
                }
                else if (points.Find(element => targetPoint) != null)
                {
                    changeDirection();
                    targetPoint = points[points.IndexOf(targetPoint) + 1];
                }

            }
            // animator here
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime));
            //transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lockMove = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lockMove = false;

        }
    }

    void changeDirection()
    {
        if (animator != null)
        {
            if (left)
            {
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isWalkingRight", true);
                left = false;
                right = true;
            }
            if (right)
            {
                animator.SetBool("isWalkingLeft", true);
                animator.SetBool("isWalkingRight", false);
                left = true;
                right = false;
            }
        }
    }
}

