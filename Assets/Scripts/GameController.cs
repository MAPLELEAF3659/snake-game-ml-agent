using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    FoodGenerator foodGenerator;

    private void Awake()
    {
        foodGenerator = GameObject.FindGameObjectWithTag("FoodGenerator").GetComponent<FoodGenerator>();
        GenerateNextFood();
    }

    public void GenerateNextFood()
    {
        foodGenerator.GenerateFood();
    }
}
