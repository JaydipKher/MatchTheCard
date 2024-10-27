using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class DisplayScore : MonoBehaviour
{
    public TMP_Text scoreText;
    private Action scoreDisplayCompleteCallback;

    private void OnEnable()
    {
        GameActionManager.Instance.displayScore += displayScore;
    }
    private void OnDisable()
    {
        if(GameActionManager.Instance == null) return;
        GameActionManager.Instance.displayScore -= displayScore;
    }
    private void displayScore(Action callBack)
    {
        scoreDisplayCompleteCallback = callBack;
        AnimateScoreIncrement(PlayerManager.Instance.score,ScoreConfigManager.Instance.GetCurrentScore());
    }
    private void AnimateScoreIncrement(int fromScore, int toScore)
    {
        // Animates the score from `fromScore` to `toScore` over 0.5 seconds
        DOVirtual.Int(fromScore, toScore, 0.5f, value =>
        {
            UpdateScoreText(value);
        }).SetEase(Ease.Linear).OnComplete(OnCompleteDisplayScore);
    }
    private void OnCompleteDisplayScore()
    {
        scoreDisplayCompleteCallback?.Invoke();
    }
    private void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}