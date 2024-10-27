using UnityEngine;

public class ScoreConfigManager : Singleton<ScoreConfigManager>
{
    public int pointsPerMatch = 10;
    public int mismatchPenalty = 5;
    public float comboMultiplier = 1.5f;
    private int currentScore = 0;
    private int consecutiveMatches = 0;

    // Method to calculate score based on match success or failure
    private void OnEnable()
    {
        GameActionManager.Instance.UpdateScrore += UpdateScore;
    }
    private void OnDisable()
    {
        if (GameActionManager.Instance == null) return;
        GameActionManager.Instance.UpdateScrore -= UpdateScore;
    }
    public void UpdateScore(bool isMatch)
    {
        if (isMatch)
        {
            consecutiveMatches++;
            int pointsEarned = (int)(pointsPerMatch * Mathf.Pow(comboMultiplier, consecutiveMatches - 1));
            currentScore += pointsEarned;
        }
        else
        {
            consecutiveMatches = 0;
            currentScore -= mismatchPenalty;

            if (currentScore < 0)
                currentScore = 0;
        }
    }

    // Method to get the current score
    public int GetCurrentScore()
    {
        return currentScore;
    }
    public void ResetScore()
    {
        currentScore = 0;
        consecutiveMatches = 0;
    }
}
