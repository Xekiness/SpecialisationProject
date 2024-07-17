using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelingSystem : MonoBehaviour
{
    public int currentLevel = 0;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;
    public int totalExperience = 0;
    
    public delegate void LevelUpAction();
    public static event LevelUpAction OnLevelUp;
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        totalExperience += amount;  
        if(currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        experienceToNextLevel = CalculateExperienceToNextLevel(currentLevel);

        if(OnLevelUp != null)
        {
            OnLevelUp();
        }
    }
    private int CalculateExperienceToNextLevel(int level)
    {
        //Formula for increasing experience for next level
        return Mathf.FloorToInt(100 * Mathf.Pow(1.1f, level - 1));
    }
}
