using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField] SnakeController snakeController;
    [SerializeField] Vector3 targetVector;
    int detectedItem; // 0,1,2 = nothing,obstacle,food

    private void Update()
    {
        transform.localPosition = snakeController.GetLocalPos() + targetVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Border":
            case "SnakeBody":
                detectedItem = 1;
                break;
            case "Food":
                detectedItem = 2;
                break;
            default:
                detectedItem = 0;
                break;
        }
    }

    public int GetDetection()
    {
        return detectedItem;
    }
}
