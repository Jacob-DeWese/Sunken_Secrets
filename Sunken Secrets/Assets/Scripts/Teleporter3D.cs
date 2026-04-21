// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Logan Kemper

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/*
Add a checkbox condition to see if the teleport is the one from the diner to the dock/boat
If yes, check to see what the position of the light source is. If it isn't at its final destination, then don't teleport.
If the light is at the final position, then it will let the user teleport
*/

namespace DigitalWorlds.StarterPackage3D
{
    public class Teleporter3D : MonoBehaviour
    {
        [Tooltip("Enter the player's tag name. Could be used for other tags as well.")]
        [SerializeField] private string tagName = "Player";

        [Tooltip("Drag in the transform that the player will be teleported to.")]
        [SerializeField] private Transform destination;

        [Tooltip("If true, the player will be rotated to match the rotation of the destination transform.")]
        [SerializeField] private bool useDestinationRotation = false;

        [Tooltip("If true, the teleport key must be pressed once the player has entered the trigger. If false, they will be teleported as soon as they enter the trigger.")]
        [SerializeField] private bool requireKeyPress = true;

        [Tooltip("If true, the script checks the position of the light source. If it hasn't reached the end, it won't teleport. If off, it freely teleports")]
        [SerializeField] private bool isDinerInterior = false;

        [Header("List of particle indicators")]
        [Tooltip("If each object in the list is active, you cannot teleport")]
        [SerializeField] protected List<GameObject> particleIndicators = new();

        [SerializeField] protected Light lightSource;

        [Tooltip("The key input that the script is listening for.")]
        [SerializeField] private KeyCode teleportKey = KeyCode.Space;

        [Header("Fade In/Out System")]
        [Tooltip("The screen object that is used to fade in and out during teleportation")]
        [SerializeField] private CanvasGroup fadeBlackScreen;

        [Tooltip("Duration of the fade (in/out)")]
        [SerializeField] protected float fadeTimeDuration = 0.2f;

        [Tooltip("Duration of holding on black before fading again")]
        [SerializeField] protected float timeToHold = 0.2f;

        [SerializeField] private Animator boatAnimator;

        [Space(20)]
        [SerializeField] private UnityEvent onTeleported;

        private Transform player;
        private bool isFading = false;

        public bool AllParticlesDeactivated
        {
            get
            {
                for (int i = 0; i < particleIndicators.Count; i++)
                {
                    if (particleIndicators[i].activeInHierarchy == true)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private void Update()
        {
            bool lightAtTargetRotation = lightSource != null && Mathf.Approximately(lightSource.transform.eulerAngles.x, 270f);
            canLeaveDiner = !isDinerInterior || lightAtTargetRotation;

            if (Input.GetKeyDown(teleportKey) && requireKeyPress && player != null && !isFading)
            {
                TeleportPlayer();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(tagName) && other.CompareTag(tagName))
            {
                player = other.transform;

                for (int i = 0; i < particleIndicators.Count; i++)
                {
                    if (particleIndicators[i].activeInHierarchy == false)
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                if (!requireKeyPress && !isFading)
                {
                    TeleportPlayer();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!string.IsNullOrEmpty(tagName) && other.CompareTag(tagName))
            {
                player = null;
            }
        }

        // Teleport the player to the specified destination
        public void TeleportPlayer()
        {
            if (destination == null)
            {
                Debug.LogWarning("Teleporter destination is not assigned");
                return;
            }
            if (player == null)
            {
                Debug.LogWarning("Player character is not assigned");
                return;
            }
            if (isDinerInterior && !canLeaveDiner)
            {
                return;
            }

            if (!AllParticlesDeactivated)
            {
                Debug.Log("Cannot teleport — not all required particles collected.");
                return;
            }

            if (fadeBlackScreen != null)
            {
                StartCoroutine(FadeScreen());
            }
            else
            {
                if (useDestinationRotation)
                {
                    player.SetPositionAndRotation(destination.position, destination.rotation);
                }
                else
                {
                    player.position = destination.position;
                }

                PlayBoatAnimation();                
                onTeleported.Invoke();
            }
        }

        private IEnumerator FadeScreen()
        {
            isFading = true;
            fadeBlackScreen.gameObject.SetActive(true);
            yield return StartCoroutine(SetFadeAlpha(0f, 1f, fadeTimeDuration));
            yield return new WaitForSeconds(timeToHold);
            
            if (isDinerInterior && !canLeaveDiner)
            {
                isFading = false;
                yield break;
            }

            if (!AllParticlesDeactivated)
            {
                yield return StartCoroutine(SetFadeAlpha(1f, 0f, fadeTimeDuration));
                fadeBlackScreen.gameObject.SetActive(false);
                isFading = false;
                yield break;
            }

            if (useDestinationRotation)
            {
                player.SetPositionAndRotation(destination.position, destination.rotation);
            }
            else
            {
                player.position = destination.position;
            }

            PlayBoatAnimation();

            onTeleported.Invoke();
            yield return StartCoroutine(SetFadeAlpha(1f, 0f, fadeTimeDuration));
            fadeBlackScreen.gameObject.SetActive(false);
            isFading = false;

        }

        private IEnumerator SetFadeAlpha(float startAlpha, float endAlpha, float duration)
        {
            float elapsed = 0f;
            fadeBlackScreen.alpha = startAlpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadeBlackScreen.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                yield return null;
            }

            fadeBlackScreen.alpha = endAlpha;
        }

        public bool canLeaveDiner
        {
            get;
            private set;
        }

        private void PlayBoatAnimation()
        {
            if (boatAnimator == null)
            {
                return;
            }

            boatAnimator.ResetTrigger("teleportedBoat");
            boatAnimator.SetTrigger("teleportedBoat");
        }
    }
}