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

    [Header("NPC Detection")]
    [SerializeField] private float raycastLength = 5f;
    [SerializeField] private string playerTag = "Player";

    [Header("Caught Screen")]
    [SerializeField] private Transform playerRespawnPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Image caughtScreen;
    [SerializeField] private float caughtScreenDuration = 2f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        facingRight = true;
        facingLeft = false;
        facingUp = false;
        facingDown = false;

        graphicsChild.localEulerAngles = Vector3.zero;
        targetPoint = pathPoint2;

        if (caughtScreen != null)
        {
            caughtScreen.alpha = 0f;
            caughtScreen.gameObject.SetActive(false);
        }
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

            CheckRaycast();
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

    private void CheckRaycast()
    {
        Vector3 rayDirection = targetPoint == pathPoint2 ? Vector3.right : Vector3.left;

        Ray ray = new Ray(objectToMove.position, rayDirection);

        // Visible in the Scene view during play mode for debugging
        Debug.DrawRay(objectToMove.position, rayDirection * raycastLength, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                CatchPlayer();
            }
        }
    }

    private void CatchPlayer()
    {
        if (playerTransform != null && playerRespawnPoint != null)
        {
            playerTransform.position = playerRespawnPoint.position;
        }

        if (caughtScreen != null)
        {
            StartCoroutine(ShowCaughtScreen());
        }
    }

    private IEnumerator ShowCaughtScreen()
    {
        caughtScreen.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            caughtScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(caughtScreenDuration);

        elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            caughtScreen.alpha = Mathf.Lerp(1f, 0f, elapsed / 0.5f);
            yield return null;
        }

        caughtScreen.gameObject.SetActive(false);
    }
}