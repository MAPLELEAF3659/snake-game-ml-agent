using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class SnakeController : MonoBehaviour
{
    [SerializeField] GameController gameController;

    [SerializeField]
    GameObject snakeBody;
    [ReadOnly]
    public List<GameObject> snakeBodies = new List<GameObject>();
    [ReadOnly]
    public List<Vector3> snakePosHistories = new List<Vector3>();

    [SerializeField, Range(0, 1f)]
    float moveInterval = 0.5f;
    float timerTick = 0f;

    bool isGameOver = true;
    bool isTurnning;
    bool isGrowwing;

    Direction direction = Direction.Right;

    void Update()
    {
        if (isGameOver || isTurnning)
        {
            return;
        }

        // **up/down/left/right key events**
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)))
        {
            TryToTurn(Direction.Up); // try to turn up
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            TryToTurn(Direction.Right); // try to turn right
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            TryToTurn(Direction.Down); // try to turn down
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            TryToTurn(Direction.Left); // try to turn left
        }
    }

    void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }

        if (timerTick < moveInterval)
        {
            timerTick += Time.fixedDeltaTime;
            return;
        }
        else
        {
            timerTick = 0;
        }

        // check if all clear
        if (GetLength() == 128)
        {
            isGameOver = true;
            gameController.GameOver();
            return;
        }

        // turn around
        if (isTurnning)
        {
            switch (direction)
            {
                case Direction.Up:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.Right:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Direction.Left:
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
            isTurnning = false;
        }

        // grow 1 body
        if (isGrowwing)
        {
            GenerateBody();
            isGrowwing = false;
        }

        // move forward
        transform.position += transform.forward;
        transform.position = new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));

        // update body pos
        for (int i = 0; i < snakeBodies.Count; i++)
        {
            snakeBodies[i].transform.position = snakePosHistories[snakePosHistories.Count - 1 - i];
        }
        for (int i = 0; i < snakePosHistories.Count; i++)
        {
            if (i < snakePosHistories.Count - snakeBodies.Count)
                snakePosHistories.RemoveAt(i);
        }
        snakePosHistories.Add(transform.position);
    }

    public void StartMove()
    {
        snakePosHistories.Add(transform.position);
        isGameOver = false;
    }

    public void Reset()
    {
        snakePosHistories.Clear();
        foreach (var snake in snakeBodies)
        {
            Destroy(snake);
        }
        snakeBodies.Clear();
        gameController.UpdateLengthText(GetLength());
        transform.localPosition = new Vector3(-5, 0, 0);
        isGrowwing = false;
        direction = Direction.Right;
        isTurnning = true;
        isGameOver = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Border":
            case "SnakeBody":
                isGameOver = true;
                gameController.GameOver();
                break;
            case "Food":
                gameController.GenerateNextFood(snakePosHistories);
                isGrowwing = true;
                break;
        }
    }

    void GenerateBody()
    {
        GameObject body = Instantiate(snakeBody,
            snakePosHistories[snakePosHistories.Count - 1],
            Quaternion.identity);
        body.transform.parent = transform.parent;
        snakeBodies.Add(body);
        gameController.UpdateLengthText(snakeBodies.Count + 1);
    }

    public void TryToTurn(Direction targetDirection)
    {
        switch (targetDirection)
        {
            case Direction.Up:
                if (direction == Direction.Down)// check if turnning back
                {
                    return;
                }
                break;
            case Direction.Down:
                if (direction == Direction.Up)// check if turnning back
                {
                    return;
                }
                break;
            case Direction.Left:
                if (direction == Direction.Right)// check if turnning back
                {
                    return;
                }
                break;
            case Direction.Right:
                if (direction == Direction.Left)// check if turnning back
                {
                    return;
                }
                break;
        }

        direction = targetDirection;
        isTurnning = true;
    }

    public int GetLength()
    {
        return snakeBodies.Count + 1;
    }

    public Vector3 GetLocalPos()
    {
        return transform.localPosition;
    }

    public Direction GetDirection()
    {
        return direction;
    }
}
