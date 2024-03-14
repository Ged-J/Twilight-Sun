using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;
    private int difficultyScore = 0; // Higher means the game will be harder
    
    private int maxDifficultyScore = 5; // Maximum difficulty
    private int minDifficultyScore = 0; // Minimum difficulty
    
    public event Action OnDifficultyChanged; // Define an event

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            difficultyScore = 5; // Starting difficulty score, adjust as needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied()
    {
        difficultyScore = Mathf.Max(minDifficultyScore, difficultyScore - 1);
        Debug.Log($"Player died. Decreasing difficulty to {difficultyScore}.");
        OnDifficultyChanged?.Invoke(); // Invoke the event
    }

    public void LevelCompleted()
    {
        difficultyScore = Mathf.Min(maxDifficultyScore, difficultyScore + 1);
        
        Debug.Log($"Level completed. Increasing difficulty to {difficultyScore}.");
        OnDifficultyChanged?.Invoke(); // Invoke the event
    }

    // Provide a method to get the current difficultyScore
    public int GetCurrentDifficultyScore()
    {
        return difficultyScore;
    }
    
}


