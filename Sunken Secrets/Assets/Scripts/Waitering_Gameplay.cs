using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

/*
1. Randomly generate 1 of the 4 options that an NPC can order
    - Just pick a random number between 1 and 4, corresponds to index of a list
2. Each food object will be tagged with a tag that specifies the order (burger, pancakes, waffles, steak)
    - Each food object will be in a different location
3. When creating a random version, if there is one already in the scene it will use that one
3a. If there is not that food object at that position (i.e. it was already served), then create a duplicate at the starting point of the first object
4. When colliding with a food object, set a boolean condition 'servingFoodActive' to trigger a switch to using the serving animations
5. When colliding with an NPC, check that their order GameObject has part of the name the same as their tag
5a. If it matches, set it in front of the NPC
*/

public class Waitering_Gameplay : MonoBehaviour
{

    [Header("Waitering Gameplay")]
    [Tooltip("List of locations to duplicate each object")]
    [SerializeField] private List<Transform> foodSpawnLocations;

    private List<GameObject> npcFoodOrders = new List<GameObject>();
    private GameObject currentSpawnedFood = null;
    private GameObject currentlyServing = null;
    private bool activelyServingFood = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNPCFoodOrders();

        if (npcFoodOrders.Count > 0)
        {
            WaiteringGameplay();
        }
        else
        {
            Debug.LogWarning("No NPC food orders found in scene.");
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
        if (!activelyServingFood && (other.CompareTag("burger") || other.CompareTag("waffles") 
            || other.CompareTag("pancakes") || other.CompareTag("steak")))
        {
            currentlyServing = other.gameObject;
            currentSpawnedFood = null;
            currentlyServing.SetActive(false);
            activelyServingFood = true;

            Debug.Log($"Player picked up: {currentlyServing.name}");
            return;
        }

        if (!other.CompareTag("NPC_Required") && !other.CompareTag("NPC_Optional"))
        {
            return;
        }


        if (!activelyServingFood || currentlyServing == null)
        {
            Debug.Log("Player has no food to serve.");
            return;
        }

        NPC_Food_Order npcOrder = other.GetComponent<NPC_Food_Order>();
        if (npcOrder == null)
        {
            return;
        }
        GameObject foodObj = npcOrder.GetFood();
        if (foodObj == null)
        {
            return;
        }

        if (currentlyServing.name == foodObj.name + "(Clone)")
        {
            Debug.Log($"Correct order delivered to '{other.gameObject.name}'!");

            npcFoodOrders.Remove(foodObj);
            Destroy(currentlyServing);
            currentlyServing = null;
            activelyServingFood = false;

            WaiteringGameplay();
        }
        else
        {
            Debug.Log($"Wrong order! NPC wants '{foodObj.name}' but player is holding '{currentlyServing.name}'.");
        }
    }
}
