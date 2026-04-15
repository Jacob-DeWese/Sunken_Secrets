using UnityEngine;
using System.Collections;

public class Trigger_Change_Animation_Controllers : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController locationController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animation_Trigger_Switching switcher = other.GetComponent<Animation_Trigger_Switching>();

            if (switcher != null)
            {
                StartCoroutine(DelayAnimationSwitch(switcher));
            }
        }

    }

    private IEnumerator DelayAnimationSwitch(Animation_Trigger_Switching switcher)
    {
        yield return new WaitForSeconds(0.22f);
        switcher.SetAnimationController(locationController);
    }
}
