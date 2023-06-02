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
        sensor.AddObservation(snakeController.GetDirection() == Direction.Up);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Down);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Left);
        sensor.AddObservation(snakeController.GetDirection() == Direction.Right);
        sensor.AddObservation(snakeController.GetLocalPos().x);
        sensor.AddObservation(snakeController.GetLocalPos().y);
        sensor.AddObservation(gameController.GetCurrentFoodLocalPos().x);
        sensor.AddObservation(gameController.GetCurrentFoodLocalPos().y);
        sensor.AddObservation(lengthPrev);
        sensor.AddObservation(distancePrev);
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
            AddReward(-10f);
            //print(string.Format("Game over! length: {0}, reward: {1}", snakeController.GetLength(), GetCumulativeReward()));
            EndEpisode();
            return;
        }

        //AddReward(-0.005f); //every step

        if (snakeController.GetLength() != lengthPrev) //eat food
        {
            AddReward(3f);
            lengthPrev = snakeController.GetLength();
            //print("eat food! reward(+0.1): " + GetCumulativeReward());
            return;
        }

        if (gameController.GetDistanceBetweenSnakeFood() < distancePrev)
        {
            AddReward(0.2f); // close to food
            //print("dist(close): " + distancePrev + ", reward(+0.01): " + GetCumulativeReward());
        }
        else if (gameController.GetDistanceBetweenSnakeFood() > distancePrev)
        {
            AddReward(-0.05f); // go away from food
            //print("dist(go away): " + distancePrev + ", reward(-0.01): " + GetCumulativeReward());
        }
        distancePrev = gameController.GetDistanceBetweenSnakeFood();
    }
}
