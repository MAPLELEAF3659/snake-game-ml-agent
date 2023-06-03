using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject food;

    [Header("EnvRange")]
    [SerializeField]
    float maxX;
    [SerializeField]
    float minX;
    [SerializeField]
    float maxZ;
    [SerializeField]
    float minZ;

    GameObject foodCurrent;

    public void GenerateFood(List<Vector3> posHistories)
    {
        DestoryFoodCurrent();
        Vector3 foodPos = new Vector3();
        int index = 0;
        while (index != -1) // check if new pos is on snake body
        {
            foodPos = new Vector3(Mathf.RoundToInt(Random.Range(minX + 1, maxX - 1)),
                        0f,
                        Mathf.RoundToInt(Random.Range(minZ + 1, maxZ - 1)));
            index = posHistories.IndexOf(foodPos);
        }
        foodCurrent = Instantiate(food); // generate food at empty space
        foodCurrent.transform.SetParent(transform.parent);
        foodCurrent.transform.localPosition = foodPos;
    }

    public Vector3 GetFoodCurrentLocalPos()
    {
        return foodCurrent.transform.localPosition;
    }

    public void DestoryFoodCurrent()
    {
        Destroy(foodCurrent);
    }
}
