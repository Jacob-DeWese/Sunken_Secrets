using UnityEngine;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
