using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_LightingIndicator : MonoBehaviour
{
    [SerializeField] protected Light lightSource;
    [SerializeField] private Image lightIndicatorBase;
    [SerializeField] private List<Sprite> lightAndNight;

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
            for (int i = 0; i < lightAndNight.Count; i++)
            {
                if (lightAndNight[i].name == "Night Indicator_0")
                {
                    lightIndicatorBase.sprite = lightAndNight[i];
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            for (int i = 0; i < lightAndNight.Count; i++)
            {
                if (lightAndNight[i].name == "Day Indicator_0")
                {
                    lightIndicatorBase.sprite = lightAndNight[i];
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
