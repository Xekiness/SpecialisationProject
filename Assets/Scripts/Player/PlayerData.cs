using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int currentLevel = 0;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    public int totalMoney = 0;
    public int totalFragments = 0;
    public int enemiesKilled = 0;

    public float maxHealth = 100;
    public float healthRegen = 1;
    public float pickupRange = 1;
    public float movementSpeed = 5;
    public float dashRange = 2;
    public float dashCooldown = 5;

    public delegate void LevelUpAction();
    public static event LevelUpAction OnLevelUp;

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        experienceToNextLevel = CalculateExperienceToNextLevel(currentLevel);

        OnLevelUp?.Invoke();
    }

    private int CalculateExperienceToNextLevel(int level)
    {
        return Mathf.FloorToInt(100 * Mathf.Pow(1.1f, level - 1));
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
    }

    public void AddFragments(int amount)
    {
        totalFragments += amount;
    }
    public void AddEnemyKilled()
    {
        enemiesKilled++;
    }
}
