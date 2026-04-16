// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Logan Kemper

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
Add a checkbox condition to see if the teleport is the one from the diner to the dock/boat
If yes, check to see what the position of the light source is. If it isn't at its final destination, then don't teleport.
If the light is at the final position, then it will let the user teleport
*/

namespace DigitalWorlds.StarterPackage3D
{
    /// <summary>
    /// Teleports the player to another location.
    /// </summary>
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

        [Tooltip("The key input that the script is listening for.")]
        [SerializeField] private KeyCode teleportKey = KeyCode.Space;

        [Header("Fade In/Out System")]
        [Tooltip("The screen object that is used to fade in and out during teleportation")]
        [SerializeField] private CanvasGroup fadeBlackScreen;

        [Tooltip("Duration of the fade (in/out)")]
        [SerializeField] protected float fadeTimeDuration = 0.2f;

        [Tooltip("Duration of holding on black before fading again")]
        [SerializeField] protected float timeToHold = 0.2f;

        [Space(20)]
        [SerializeField] private UnityEvent onTeleported;

        private Transform player;
        private bool isFading = false;

        private void Update()
        {
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
                
                onTeleported.Invoke();
            }
        }

        private IEnumerator FadeScreen()
        {
            isFading = true;
            fadeBlackScreen.gameObject.SetActive(true);
            yield return StartCoroutine(SetFadeAlpha(0f, 1f, fadeTimeDuration));
            yield return new WaitForSeconds(timeToHold);
            
            if (useDestinationRotation)
            {
                player.SetPositionAndRotation(destination.position, destination.rotation);
            }
            else
            {
                player.position = destination.position;
            }

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
    }
}