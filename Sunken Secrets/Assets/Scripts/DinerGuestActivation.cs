using UnityEngine;

public class DinerGuestActivation : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float lightRotationActivationPoint = 180f;
    [SerializeField] private float lightRotationDeactivationPoint = 90f;
    [SerializeField] private GameObject npcParent;
    private float currentLightPosition;
    private bool npcRequiredInteracted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentLightPosition = Mathf.Round(lightSource.transform.eulerAngles.x);
        if (currentLightPosition <= lightRotationDeactivationPoint)
        {
            if (CompareTag("NPC_Required") && npcRequiredInteracted)
            {
                npcParent.SetActive(false);
            }
            else if (CompareTag("NPC_Optional"))
            {
                npcParent.SetActive(false);
            }        
        }
        else if (currentLightPosition <= lightRotationActivationPoint)
        {
            npcParent.SetActive(true);
        }
    }

    // Check to see if the dialogue encounter occurred (or pull from another script)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC_Required"))
        {
            npcRequiredInteracted = true;
        }
    }
}
