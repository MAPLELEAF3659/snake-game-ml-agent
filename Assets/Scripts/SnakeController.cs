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

    [SerializeField, Range(0, 0.5f)]
    float moveInterval = 0.5f;

    bool isHitObstacle;
    bool isGameOver;
    bool isTurnning;
    bool isGrowwing;

    [SerializeField] Direction direction = Direction.Right;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (isGameOver || isTurnning)
        {
            return;
        }

        // **up/down/left/right key events**
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)))// try to turn up
        {
            if (direction == Direction.Down) // check if turnning back
            {
                return;
            }
            direction = Direction.Up;
            isTurnning = true;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))// try to turn right
        {
            if (direction == Direction.Left) // check if turnning back
            {
                return;
            }
            direction = Direction.Right;
            isTurnning = true;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))// try to turn down
        {
            if (direction == Direction.Up) // check if turnning back
            {
                return;
            }
            direction = Direction.Down;
            isTurnning = true;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) // try to turn left
        {
            if (direction == Direction.Right) // check if turnning back
            {
                return;
            }
            direction = Direction.Left;
            isTurnning = true;
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            // check if hit obstacle or all clear
            if (isHitObstacle || (snakeBodies.Count + 1 == 128))
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

            // wait for interval
            yield return new WaitForSeconds(moveInterval);
        }

        isGameOver = true;
        gameController.GameOver();
    }

    public void StartMove()
    {
        StartCoroutine(Move());
    }

    public void Reset()
    {
        snakePosHistories.Clear();
        foreach (var snake in snakeBodies)
        {
            Destroy(snake);
        }
        snakeBodies.Clear();
        gameController.UpdateLengthText(snakeBodies.Count + 1);
        transform.position = new Vector3(-5, 0, 0);
        isHitObstacle = false;
        isGrowwing = false;
        isGameOver = false;
        direction = Direction.Right;
        isTurnning = true;
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
}
