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

    public void GenerateFood()
    {
        Instantiate(food,
            new Vector3(Mathf.RoundToInt(Random.Range(minX, maxX - 1)) + 0.5f,
                        0f,
                        Mathf.RoundToInt(Random.Range(minZ, maxZ - 1)) + 0.5f),
            Quaternion.identity);
    }
}
