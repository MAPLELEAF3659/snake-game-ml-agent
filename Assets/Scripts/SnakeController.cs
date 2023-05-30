using System;
using System.Collections;
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

    [SerializeField, Range(0, 1)]
    float moveInterval = 1f;

    bool isHitObstacle;
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
            yield return new WaitForSeconds(moveInterval);
        }
        print("GameOver");
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Border":
                isHitObstacle = true;
                break;
            case "Food":
                Destroy(other.gameObject);
                gameController.GenerateNextFood();
                print(string.Format("Length={0}", ++length));
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Border"))
        {
            isHitObstacle = false;
        }
    }
}
