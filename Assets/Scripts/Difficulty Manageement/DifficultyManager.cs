using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;
    private int difficultyScore = 1; // Higher means the game will be harder
    
    private int maxDifficultyScore = 5; // Maximum difficulty
    private int minDifficultyScore = 1; // Minimum difficulty
    
    public event Action OnDifficultyChanged; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            difficultyScore = 5; 
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
        OnDifficultyChanged?.Invoke(); 
    }

    public void LevelCompleted()
    {
        difficultyScore = Mathf.Min(maxDifficultyScore, difficultyScore + 1);
        
        Debug.Log($"Level completed. Increasing difficulty to {difficultyScore}.");
        OnDifficultyChanged?.Invoke(); 
    }
    
    public int GetCurrentDifficultyScore()
    {
        return difficultyScore;
    }
    
}


