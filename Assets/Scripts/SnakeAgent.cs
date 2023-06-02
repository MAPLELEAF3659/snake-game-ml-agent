using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using Unity.Mathematics;

public class SnakeAgent : Agent
{
    [SerializeField] GameController gameController;
    [SerializeField] SnakeController snakeController;
    float distancePrev;
    int lengthPrev;

    [SerializeField] ObstacleDetector[] obstacleDetectors; // 0,1,2,3=up,down,left,right

    public override void OnEpisodeBegin()
    {
        gameController.ResetGame();
        lengthPrev = snakeController.GetLength();
        distancePrev = gameController.GetDistanceBetweenSnakeFood();
        gameController.StartGame();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKeyUp(KeyCode.UpArrow)) { discreteActionsOut[0] = 1; }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { discreteActionsOut[0] = 2; }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { discreteActionsOut[0] = 3; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { discreteActionsOut[0] = 4; }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // observe snake's direction
        sensor.AddObservation(snakeController.GetDirection() == Direction.Up);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Down);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Left);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Right);

        Vector3 snakePos = snakeController.GetLocalPos();
        sensor.AddObservation(snakePos.x);
        sensor.AddObservation(snakePos.z);

        Vector3 fooodPos = gameController.GetCurrentFoodLocalPos();
        sensor.AddObservation(fooodPos.x);
        sensor.AddObservation(fooodPos.z);

        sensor.AddObservation(obstacleDetectors[0].GetDetection()); // observe if snake close to obstacle/food(4 directions)
        sensor.AddObservation(obstacleDetectors[1].GetDetection()); // 0,1,2,3=up,down,left,right
        sensor.AddObservation(obstacleDetectors[2].GetDetection());
        sensor.AddObservation(obstacleDetectors[3].GetDetection());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        switch (actions.DiscreteActions[0])
        {
            case 1:
                snakeController.TryToTurn(Direction.Up);
                break;
            case 2:
                snakeController.TryToTurn(Direction.Down);
                break;
            case 3:
                snakeController.TryToTurn(Direction.Left);
                break;
            case 4:
                snakeController.TryToTurn(Direction.Right);
                break;
            default:
                break;
        }

        if (gameController.isGameOver)
        {
            AddReward(-1);
            EndEpisode();
            return;
        }

        if (snakeController.GetLength() != lengthPrev) //eat food
        {
            AddReward(0.1f);
            lengthPrev = snakeController.GetLength();
        }

        if (gameController.GetDistanceBetweenSnakeFood() != distancePrev)
        {
            if (gameController.GetDistanceBetweenSnakeFood() < distancePrev)
            {
                AddReward(0.01f); // close to food
            }
            else if (gameController.GetDistanceBetweenSnakeFood() > distancePrev)
            {
                AddReward(-0.01f); // go away from food
            }
            distancePrev = gameController.GetDistanceBetweenSnakeFood();
        }
    }
}
