using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SnakeAgent : Agent
{
    GameController gameController;
    SnakeController snakeController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        snakeController = GameObject.FindGameObjectWithTag("Snake").GetComponent<SnakeController>();
    }

    public override void OnEpisodeBegin()
    {
        gameController.ResetGame();
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
            SetReward(snakeController.GetLength() / 128);
            EndEpisode();
        }
    }
}
