using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class NPC_Food_Order : MonoBehaviour
{
    [SerializeField] protected GameObject foodOrder;
    [SerializeField] protected List<GameObject> foodOptions;
    [SerializeField] private Dictionary<GameObject, string> foodObjToText = new();

    void Awake()
    {
        for (int i = 0; i < foodOptions.Count; i++)
        {
            if (foodOptions[i].name == "burgerfries")
            {
                foodObjToText.Add(foodOptions[i], "Burger (med) + fries");
            }
            else if (foodOptions[i].name == "chickenwaffles")
            {
                foodObjToText.Add(foodOptions[i], "Chicken and waffles");
            }
            else if (foodOptions[i].name == "pancakes")
            {
                foodObjToText.Add(foodOptions[i], "Pancakes, large portion");
            }
            else if (foodOptions[i].name == "steakeggs")
            {
                foodObjToText.Add(foodOptions[i], "Steak (med rare) + eggs");
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetOrderText()
    {
        if (foodOrder == null)
        {
            return null;
        }

        if (foodObjToText.TryGetValue(foodOrder, out string orderText))
        {
            return orderText;
        }

        return null;
    }

    public GameObject GetFood()
    {
        return foodOrder;
    }
}
