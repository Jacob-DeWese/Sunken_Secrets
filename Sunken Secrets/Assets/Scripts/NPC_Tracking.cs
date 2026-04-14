using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class NPC_Tracking : MonoBehaviour
{
    /*
    Thinking this through: 
    - Make a list of all NPCs in each level, either tagged as required or not
    - For each GameObject in the list, if they are required, push that GameObject to another list called "required list"
    - When collided with, remove them from the required list
    - When the required list is empty, move the ocean to reveal the sunken city
    - *Encouraged: make a range for the light source to move to emulate daylight, take size of required list and move the light range/size of list degrees
    */

    [Header("Ocean Trigger System")]
    [Tooltip("List to store all the NPC GameObjects")]
    [SerializeField] protected List<GameObject> npcs = new();
    [Tooltip("List for all the required NPCs")]
    [SerializeField] protected List<GameObject> required_npcs = new();
    [Tooltip("Ocean GameObject")]
    [SerializeField] protected GameObject ocean;
    [SerializeField] protected float moveTime = 3f;
    private float timer = 0f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected Light lightSource;

    [Header("Pier Trigger")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject pierPosition;
    [SerializeField] private float withinPierPosition = 1f;
    [SerializeField] private float delayTideRecede = 5.5f;
    private float? delayTimer = null;
    private bool isReceding = false;
    private int initialRequiredCount;

    private bool PlayerAtPier()
    {
        return Vector3.Distance(player.position, pierPosition.transform.position) <= withinPierPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i].CompareTag("NPC_Required"))
                required_npcs.Add(npcs[i]);
        }
        initialRequiredCount = required_npcs.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (required_npcs.Count == 0 && PlayerAtPier() && !isReceding)
        {
            if (delayTimer == null)
                delayTimer = 0f;

            delayTimer += Time.deltaTime;

            if (delayTimer >= delayTideRecede)
            {
                isReceding = true;
            }
        }

        if (isReceding && ocean.activeSelf)
        {
            if (timer < moveTime)
            {
                ocean.transform.Translate(Vector3.down * speed * Time.deltaTime);
                timer += Time.deltaTime;
            }
            else
            {
                ocean.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required"))
        {
            if (required_npcs.Count > 0 && required_npcs.Contains(other.gameObject))
            {
                required_npcs.Remove(other.gameObject);

                int npcsInteracted = initialRequiredCount - required_npcs.Count; // ✅ required only
                float progress = npcsInteracted / (float)initialRequiredCount;   // 0.0 → 1.0

                // Map progress across your intended angle range (e.g. 180° → 270°)
                float startAngle = 180f;
                float endAngle = 270f;
                float targetX = Mathf.Lerp(startAngle, endAngle, progress);      // ✅ respects start angle

                Vector3 currentEuler = lightSource.transform.localEulerAngles;
                lightSource.transform.localEulerAngles = new Vector3(targetX, currentEuler.y, currentEuler.z);
            }
        }
    }
}
