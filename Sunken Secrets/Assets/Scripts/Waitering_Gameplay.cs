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
    [Tooltip("List of food GameObjects to spawn")]
    [SerializeField] private List<GameObject> foodObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set all food order objects to inactive on start
        for (int i = 0; i < foodObjects.Count; i++)
        {
            foodObjects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int randomNumberGeneration()
    {
        int randomNumber = Random.Range(0, 3);
        return randomNumber;
    }

    void WaiteringGameplay()
    {
        // Randomly generate which food to produce first
        int randomNumber = randomNumberGeneration();
        GameObject randomFood = foodObjects[randomNumber];

        // Create a duplicate of the food
        GameObject duplicatedFoodOrder = Instantiate(randomFood);
        // Make sure it is active
        if (duplicatedFoodOrder.activeInHierarchy == false)
        {
            duplicatedFoodOrder.SetActive(true);
        }
        
        Debug.Log("Current duplicated food: {duplicatedFoodOrder}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC_Required") && !other.CompareTag("NPC_Optional"))
        {
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

        if (!foodObj.activeInHierarchy)
        {
            foodObj.SetActive(true);
        }
        
    }
}
