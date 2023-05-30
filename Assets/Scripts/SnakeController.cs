using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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
    List<GameObject> snakeBodies = new List<GameObject>();
    List<Vector3> snakeHeadPosHistories = new List<Vector3>();

    [SerializeField, Range(0, 1)]
    float moveInterval = 1f;

    bool isHitObstacle;
    bool isGameOver;
    int length = 1;

    Direction direction = Direction.Up;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        StartCoroutine(Move());
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !direction.Equals(Direction.Down))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && !direction.Equals(Direction.Left))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            direction = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && !direction.Equals(Direction.Up))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && !direction.Equals(Direction.Right))
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            direction = Direction.Left;
        }
    }

    int tick = 0;
    IEnumerator Move()
    {
        while (true)
        {
            tick = isHitObstacle ? tick + 1 : 0;
            if (tick >= 1)
            {
                break;
            }
            transform.Translate(Vector3.forward);
            snakeHeadPosHistories.Add(transform.position);

            for (int i = 0; i < snakeBodies.Count; i++)
            {
                snakeBodies[i].transform.position = snakeHeadPosHistories[snakeHeadPosHistories.Count - 2 - i];
            }

            yield return new WaitForSeconds(moveInterval);
        }
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
                GenerateBody();
                break;
        }
    }

    void GenerateBody()
    {
        GameObject body = Instantiate(snakeBody,
            snakeHeadPosHistories[snakeHeadPosHistories.Count - 2],
            Quaternion.identity);
        snakeBodies.Add(body);
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
}
