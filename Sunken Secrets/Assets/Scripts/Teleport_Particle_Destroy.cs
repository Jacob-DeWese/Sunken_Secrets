using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class Teleport_Particle_Destroy : MonoBehaviour
{
    // store all particle systems here
     public List<Particle> particles = new List<Particle>();
     public List<Particle> triggerParticles = new List<Particle>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Particle particle in particles)
        {
            if (particle == null)
            {
                UnityEngine.Debug.Log("One of the particle systems in the list is not assigned.");
            }
            if (particle.CompareTag("Conditional"))
            {
                particle.gameObject.SetActive(false);
            }
            if (particle.CompareTag("Directional"))
            {
                particle.gameObject.SetActive(true);
            }
            if (particle.trigger)
            {
                triggerParticles.Add(particle);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
