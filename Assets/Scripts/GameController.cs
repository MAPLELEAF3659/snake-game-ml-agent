using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    FoodGenerator foodGenerator;
    SnakeController snakeController;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text lengthText;
    [SerializeField] TMP_Text timeText;
    [ReadOnly] public TimeSpan time;
    [ReadOnly] public bool isGameOver = true;

    private void Awake()
    {
        foodGenerator = GameObject.FindGameObjectWithTag("FoodGenerator").GetComponent<FoodGenerator>();
        snakeController = GameObject.FindGameObjectWithTag("Snake").GetComponent<SnakeController>();
    }

    private void Start()
    {
        ResetGame();
        StartGame();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            time += TimeSpan.FromSeconds(Time.deltaTime);
            UpdateTimeText(time);
            return;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetGame();
            StartGame();
        }
    }

    public void GenerateNextFood(List<Vector3> posHistories)
    {
        foodGenerator.GenerateFood(posHistories);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.text = "GAME OVER!";
    }

    public void UpdateLengthText(int length)
    {
        lengthText.text = string.Format("LENGTH: {0}", length);
    }

    public void UpdateTimeText(TimeSpan time)
    {
        timeText.text = string.Format("TIME: {0}:{1}:{2}.{3}",
            time.Hours.ToString("00"),
            time.Minutes.ToString("00"),
            time.Seconds.ToString("00"),
            time.Milliseconds.ToString().Substring(0, 1));
    }

    public void StartGame()
    {
        GenerateNextFood(snakeController.snakePosHistories);
        snakeController.StartMove();
        isGameOver = false;
    }

    public void ResetGame()
    {
        gameOverText.text = "";
        snakeController.Reset();
        time = TimeSpan.Zero;
        UpdateTimeText(time);
        foreach (var food in GameObject.FindGameObjectsWithTag("Food"))
        {
            Destroy(food);
        }
    }
}
