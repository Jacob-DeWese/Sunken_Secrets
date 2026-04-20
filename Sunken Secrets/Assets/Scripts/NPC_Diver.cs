using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NPC_Diver : MonoBehaviour
{
    public List<Transform> points;
    public float speed = 1f;
    bool followingPlayer;
    Transform targetPoint;
    bool lockMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPoint = points[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockMove) {
            if (transform.position == targetPoint.position)
            {
                if (targetPoint == points[points.Count - 1])
                    targetPoint = points[0];
                else if(points.Find(element => targetPoint) != null && !followingPlayer)
                    targetPoint = points[points.IndexOf(targetPoint) + 1];
                else if (followingPlayer) {}
                    // i don't think we need this but ok
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }
    }

    void onTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lockMove = true;
            
        }
    }

      void onTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lockMove = false;
            
        }
    }
}
