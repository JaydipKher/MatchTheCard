using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerManager : Singleton<PlayerManager>
{
    public int currentLevel = 0;
    public int score = 0;
    private string filePath;
    private const string dataFileName = "playerdata.json";
    private UserData currentUserdData;
    private void OnEnable()
    {
        LoadData();
        currentLevel = currentUserdData.level;
        score = currentUserdData.score;
        GameActionManager.Instance.onLevelComplete += OnLevelComplete;
    }
    private void OnDisable()
    {
        if (GameActionManager.Instance == null) return;
        GameActionManager.Instance.onLevelComplete -= OnLevelComplete;
    }
    private void OnLevelComplete()
    {
        currentLevel++;
        SaveData(currentLevel, ScoreConfigManager.Instance.GetCurrentScore());
    }
    public void SaveData(int level, int score)
    {
        UserData userData = new UserData(level, score);
        string json = JsonUtility.ToJson(userData, true);
        string filePath = Path.Combine(Application.persistentDataPath, dataFileName);
        File.WriteAllText(filePath, json);
    }
    public void LoadData()
    {   
        string filePath = Path.Combine(Application.persistentDataPath, dataFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentUserdData= JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            SaveData(currentLevel,ScoreConfigManager.Instance.GetCurrentScore());
        }
    }
}
[System.Serializable]
public class UserData
{
    public int level;
    public int score;

    public UserData(int level, int score)
    {
        this.level = level;
        this.score = score;
    }
}