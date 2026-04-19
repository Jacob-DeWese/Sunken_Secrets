using UnityEngine;

public class UI_LightingIndicator : MonoBehaviour
{
    [SerializeField] protected Light lightSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool lightAtTargetRotation = lightSource != null && Mathf.Approximately(lightSource.transform.eulerAngles.x, 270f);
        if (lightAtTargetRotation == true)
        {
            
        }
    }
}
