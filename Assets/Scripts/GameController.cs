using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] FoodGenerator foodGenerator;
    [SerializeField] SnakeController snakeController;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text lengthText;
    [SerializeField] TMP_Text timeText;
    [ReadOnly] public TimeSpan time;
    [ReadOnly] public bool isGameOver = true;

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
        snakeController.StartMove();
        isGameOver = false;
    }

    public void ResetGame()
    {
        gameOverText.text = "";
        snakeController.Reset();
        foodGenerator.DestoryFoodCurrent();
        GenerateNextFood(snakeController.snakePosHistories);
        time = TimeSpan.Zero;
        UpdateTimeText(time);
    }

    public float GetDistanceBetweenSnakeFood()
    {
        return Vector3.Distance(foodGenerator.GetFoodCurrentLocalPos(), snakeController.GetLocalPos());
    }

    public Vector3 GetCurrentFoodLocalPos()
    {
        return foodGenerator.GetFoodCurrentLocalPos();
    }
}
