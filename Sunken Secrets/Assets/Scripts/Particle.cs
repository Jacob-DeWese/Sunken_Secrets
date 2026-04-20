using NUnit.Framework;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using System.Collections;
using System.Collections.Generic;

public class Particle : MonoBehaviour
{ 
    public string particleName;

    // trigger particles are used to activate particles with tag "Conditional"
    public bool trigger = false;
    public GameObject postcedingParticle;

    // dialogue dependent particles are particles that will only be activated after a certain dialogue has been completed.
    // This is to ensure that the player cannot skip certain parts of the game by activating particles out of order.
    //TODO: COME BACK TO THIS!
    public bool dialogueDependent = false;
    public List<Particle> npcs = new List<Particle>();

    // access teleport particle destroy script
    Teleport_Particle_Destroy teleportParticleDestroy;
   

    void Start()
    {
        teleportParticleDestroy = FindFirstObjectByType<Teleport_Particle_Destroy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Particle particle = this.gameObject.GetComponent<Particle>();
            teleportParticleDestroy.particles.Remove(particle);
            this.gameObject.SetActive(false);   

            if (particle.trigger)
            {
                particle.postcedingParticle.SetActive(true);
                teleportParticleDestroy.triggerParticles.Remove(particle);
            }
      
        }
    }
}
