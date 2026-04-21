using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnemy_Movement : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    private float speed = 2f;
    private bool wait = false;
    private bool facingLeft = false;
    private bool facingRight = true;
    private bool facingUp = false;
    private bool facingDown = false;
    private float delay = 0.5f;

    public Transform objectToMove;
    public Transform graphicsChild;
    public Transform pathPoint1;
    public Transform pathPoint2;
    private Transform targetPoint;

    [SerializeField] private Animator childAnimator;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        facingRight = true;
        facingLeft = false;
        facingUp = false;
        facingDown = false;

        graphicsChild.localEulerAngles = Vector3.zero;
        targetPoint = pathPoint2;
    }

    void Update()
    {
        if (!wait)
        {
            Vector3 moveDir = Vector3.zero;

            if (facingRight)
            {
                moveDir = Vector3.right;
                SetAnim(true, false, false, false);
            }
            else if (facingLeft)
            {
                moveDir = Vector3.left;
                SetAnim(false, true, false, false);
            }
            else if (facingUp)
            {
                moveDir = Vector3.forward;
                SetAnim(false, false, true, false);
            }
            else if (facingDown)
            {
                moveDir = Vector3.back;
                SetAnim(false, false, false, true);
            }

            objectToMove.Translate(moveDir * Time.deltaTime * speed, Space.World);
        }

        if ((targetPoint == pathPoint1 && objectToMove.position.x < targetPoint.position.x) ||
            (targetPoint == pathPoint2 && objectToMove.position.x > targetPoint.position.x))
        {
            if (!wait)
            {
                wait = true;
                objectToMove.position = targetPoint.position;
                StartCoroutine(delayTurn(delay));
            }
        }
    }

    IEnumerator delayTurn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (targetPoint == pathPoint1)
        {
            targetPoint = pathPoint2;

            facingRight = true;
            facingLeft = false;
            facingUp = false;
            facingDown = false;
        }
        else
        {
            targetPoint = pathPoint1;

            // go LEFT
            facingRight = false;
            facingLeft = true;
            facingUp = false;
            facingDown = false;
        }

        wait = false;
    }

    void SetAnim(bool right, bool left, bool up, bool down)
    {
        if (childAnimator == null) return;

        childAnimator.SetBool("movingRight", right);
        childAnimator.SetBool("movingLeft", left);
        childAnimator.SetBool("movingUp", up);
        childAnimator.SetBool("movingDown", down);
    }
}