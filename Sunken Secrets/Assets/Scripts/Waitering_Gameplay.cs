using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class Waitering_Gameplay : MonoBehaviour
{

    [Header("Waitering Gameplay")]
    [Tooltip("List of locations to duplicate each object")]
    [SerializeField] private List<Transform> foodSpawnLocations;

    [Header("Animation Controllers")]
    [SerializeField] private RuntimeAnimatorController defaultController;
    [SerializeField] private RuntimeAnimatorController servingController;
    [SerializeField] private Animator playerAnimator;

    private List<GameObject> npcFoodOrders = new List<GameObject>();
    private GameObject currentSpawnedFood = null;
    private GameObject currentlyServing = null;
    private bool activelyServingFood = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NPC_Food_Order[] allFoodOrders = FindObjectsByType<NPC_Food_Order>(FindObjectsSortMode.None);
        foreach (NPC_Food_Order npcOrder in allFoodOrders)
        {
            npcOrder.DeactivateAllFoodOptions();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNPCFoodOrders()
    {
        npcFoodOrders.Clear();

        NPC_Food_Order[] allFoodOrders = FindObjectsByType<NPC_Food_Order>(FindObjectsSortMode.None);

        foreach (NPC_Food_Order npcOrder in allFoodOrders)
        {
            GameObject foodObj = npcOrder.GetFood();

            if (foodObj != null)
            {
                npcFoodOrders.Add(foodObj);
                foodObj.SetActive(false);
                Debug.Log($"Collected food order '{foodObj.name}' from NPC '{npcOrder.gameObject.name}'");
            }
            else
            {
                Debug.LogWarning($"NPC '{npcOrder.gameObject.name}' has an NPC_Food_Order but GetFood() returned null.");
            }
        }
        Debug.Log($"Total NPC food orders collected: {npcFoodOrders.Count}");
    }

    void WaiteringGameplay()
    {
        if (currentSpawnedFood != null || activelyServingFood)
        {
            return;
        }

        if (npcFoodOrders.Count == 0)
        {
            Debug.Log("All food orders have been served!");
            return;
        }

        int randomOrder = Random.Range(0, npcFoodOrders.Count);
        GameObject nextOrder = npcFoodOrders[randomOrder];

        if (nextOrder.CompareTag("burger"))
        {
            for (int i = 0; i < foodSpawnLocations.Count; i++)
            {
                if (foodSpawnLocations[i].name == "burgerfries_spawn")
                {
                    currentSpawnedFood = Instantiate(nextOrder, foodSpawnLocations[i].position, foodSpawnLocations[i].rotation);
                    currentSpawnedFood.SetActive(true);
                    break;
                }
            }
        }
        else if (nextOrder.CompareTag("waffles"))
        {
            for (int i = 0; i < foodSpawnLocations.Count; i++)
            {
                if (foodSpawnLocations[i].name == "chickenwaffles_spawn")
                {
                    currentSpawnedFood = Instantiate(nextOrder, foodSpawnLocations[i].position, foodSpawnLocations[i].rotation);
                    currentSpawnedFood.SetActive(true);
                    break;
                }
            }
        }
        else if (nextOrder.CompareTag("pancakes"))
        {
            for (int i = 0; i < foodSpawnLocations.Count; i++)
            {
                if (foodSpawnLocations[i].name == "pancakes_spawn")
                {
                    currentSpawnedFood = Instantiate(nextOrder, foodSpawnLocations[i].position, foodSpawnLocations[i].rotation);
                    currentSpawnedFood.SetActive(true);
                    break;
                }
            }
        }
        else if (nextOrder.CompareTag("steak"))
        {
            for (int i = 0; i < foodSpawnLocations.Count; i++)
            {
                if (foodSpawnLocations[i].name == "steakeggs_spawn")
                {
                    currentSpawnedFood = Instantiate(nextOrder, foodSpawnLocations[i].position, foodSpawnLocations[i].rotation);
                    currentSpawnedFood.SetActive(true);
                    break;
                }
            }
        }

        Debug.Log($"Spawned order: {nextOrder.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activelyServingFood && other.gameObject == currentSpawnedFood)
        {
            currentlyServing = other.gameObject;
            currentSpawnedFood = null;

            currentlyServing.SetActive(false);
            activelyServingFood = true;

            SetAnimatorController(true);

            Debug.Log($"Player picked up: {currentlyServing.name}");
            return;
        }

        if (!other.CompareTag("NPC_Required") && !other.CompareTag("NPC_Optional"))
        {
            return;
        }

        NPC_Food_Order npcOrder = other.GetComponent<NPC_Food_Order>();
        if (npcOrder == null) return;

        GameObject foodObj = npcOrder.GetFood();
        if (foodObj == null) return;

        if (!activelyServingFood)
        {
            if (!npcFoodOrders.Contains(foodObj))
            {
                npcFoodOrders.Add(foodObj);
                foodObj.SetActive(false);

                Debug.Log($"Collected order '{foodObj.name}' from '{other.gameObject.name}'");

                // Spawn food after collecting
                WaiteringGameplay();
            }
            else
            {
                Debug.Log($"Order for '{foodObj.name}' already collected.");
            }

            return;
        }

        if (currentlyServing == null)
        {
            Debug.Log("Player has no food to serve.");
            return;
        }

        if (currentlyServing.CompareTag(foodObj.tag))
        {
            Debug.Log($"Correct order delivered to '{other.gameObject.name}'!");

            npcFoodOrders.Remove(foodObj);
            Destroy(currentlyServing);

            currentlyServing = null;
            activelyServingFood = false;

            SetAnimatorController(false);

            WaiteringGameplay();
        }
        else
        {
            Debug.Log($"Wrong order! NPC wants '{foodObj.name}' but player is holding '{currentlyServing.name}'.");
        }
    }

    void SetAnimatorController(bool isServing)
    {
        if (playerAnimator == null)
        {
            return;
        }

        if (isServing == true)
        {
            playerAnimator.runtimeAnimatorController = servingController;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = defaultController;
        }
    }
}
