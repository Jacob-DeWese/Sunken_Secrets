using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class Teleport_Particle_Destroy : MonoBehaviour
{
    [Tooltip("The next particle in the chain to activate before this one deactivates")]
    public GameObject postcedingParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (postcedingParticle != null)
        {
            postcedingParticle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (postcedingParticle != null)
                postcedingParticle.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
