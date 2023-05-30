using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class SnakeController : MonoBehaviour
{
    GameController gameController;

    [SerializeField]
    GameObject snakeBody;
    [ReadOnly]
    public List<GameObject> snakeBodies = new List<GameObject>();
    [ReadOnly]
    public List<Vector3> snakePosHistories = new List<Vector3>();

    [SerializeField, Range(0, 1)]
    float moveInterval = 1f;

    bool isHitObstacle;
    bool isGameOver;
    int length = 0;
    bool isTurnning;
    bool isGrowwing;

    Direction direction = Direction.Up;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        StartCoroutine(Move());
    }

    void Update()
    {
        if (isTurnning)
        {
            return;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !direction.Equals(Direction.Down))
        {
            direction = Direction.Up;
            isTurnning = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && !direction.Equals(Direction.Left))
        {
            direction = Direction.Right;
            isTurnning = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !direction.Equals(Direction.Up))
        {
            direction = Direction.Down;
            isTurnning = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && !direction.Equals(Direction.Right))
        {
            direction = Direction.Left;
            isTurnning = true;
        }
    }

    int tick = 0;
    IEnumerator Move()
    {
        while (true)
        {
            // check if hit obstacle
            tick = isHitObstacle ? tick + 1 : 0;
            if (tick >= 1)
            {
                break;
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

            if (isGrowwing)
            {
                GenerateBody();
                isGrowwing = false;
            }

            // move forward
            transform.Translate(Vector3.forward);

            // update body pos
            for (int i = 0; i < snakeBodies.Count; i++)
            {
                snakeBodies[i].transform.position = snakePosHistories[snakePosHistories.Count - 1 - i];
            }
            for (int i = 0; i < snakePosHistories.Count; i++)
            {
                if (i < snakePosHistories.Count - length)
                    snakePosHistories.RemoveAt(i);
            }

            snakePosHistories.Add(transform.position);

            // wait for interval
            yield return new WaitForSeconds(moveInterval);
        }

        // stop moving
        print("GameOver");
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Border":
            case "SnakeBody":
                isHitObstacle = true;
                break;
            case "Food":
                Destroy(other.gameObject);
                gameController.GenerateNextFood();
                isGrowwing = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Border":
            case "SnakeBody":
                isHitObstacle = false;
                break;
        }
    }

    void GenerateBody()
    {
        GameObject body = Instantiate(snakeBody,
            snakePosHistories[snakePosHistories.Count - 1],
            Quaternion.identity);
        snakeBodies.Add(body);
        length++;
    }
}
