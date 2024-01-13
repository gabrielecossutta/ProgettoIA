using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject FoodPrefab;
    private GameObject food;
    public Transform transformFood;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool HasFoodSpawned()
    {
        if (food!=null)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public bool HasFoodSpawner()
    {
        if (food != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnFood()
    {
        food = Instantiate(FoodPrefab, new Vector3(Random.Range(-3f, +1f), 0, Random.Range(4f, -4f)), Quaternion.identity);
    }
    public Transform GetLastFoodTransform()
    {
        if (food!=null)
        {
        return food.transform;

        }
        else
        {
            return transformFood;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
