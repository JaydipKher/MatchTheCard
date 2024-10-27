using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class GamePlayScreen : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CardDta cardData;
    [SerializeField] private List<CardSpriteData> cardSprites = new List<CardSpriteData>();
    [SerializeField] private AudioData audioData;
    private HashSet<int> accessedIndices = new HashSet<int>();

    private System.Random rng = new System.Random();

    private void OnEnable()
    {
        GameActionManager.Instance.levelGenerated += OnLevelGenerated;
        GameActionManager.Instance.onShowComplete += OnShowCardComplete;
        GameActionManager.Instance.showLoadingComplete += onLoadingComplete;
    }
    private void OnDisable()
    {
        if (GameActionManager.Instance == null) return;
        GameActionManager.Instance.levelGenerated -= OnLevelGenerated;
        GameActionManager.Instance.onShowComplete -= OnShowCardComplete;
         GameActionManager.Instance.showLoadingComplete -= onLoadingComplete;
    }
    private void OnLevelGenerated()
    {
        accessedIndices.Clear();
        gameData.gameCards.ForEach(card => card.OnStartShow());
        int pairCout = gameData.gameCards.Count / 2;
        for (int pairIndex = 0; pairIndex < pairCout; pairIndex++)
        {
            int randomIndex = GetRandomIndex();
            cardSprites.Add(cardData.cardSprites[randomIndex]);
            cardSprites.Add(cardData.cardSprites[randomIndex]);
        }
        Shuffle();
        for (int cardIndex = 0; cardIndex < gameData.gameCards.Count; cardIndex++)
        {
            gameData.gameCards[cardIndex].SetCardImage(cardSprites[cardIndex].cardSprite, cardSprites[cardIndex].pairid);
        }
        GameActionManager.Instance.hideLoading?.Invoke();
    }
    public void Shuffle()
    {
        int count = cardSprites.Count;
        for (int listIndex = 0; listIndex < count - 1; listIndex++)
        {
            int selectedIndex = rng.Next(listIndex, count); // Pick a random index from i to n-1
            (cardSprites[listIndex], cardSprites[selectedIndex]) = (cardSprites[selectedIndex], cardSprites[listIndex]); // Swap elements
        }
    }

    private int GetRandomIndex()
    {
        if (accessedIndices.Count >= cardData.cardSprites.Count)
        {
            Debug.LogWarning("All indices have been used. Returning -1 or resetting logic.");
            return -1; // Optional: you might want to reset accessedIndices here if needed.
        }

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardData.cardSprites.Count);
        } while (accessedIndices.Contains(randomIndex));

        accessedIndices.Add(randomIndex);
        return randomIndex;
    }

    private void OnShowCardComplete(Card card)
    {
        gameData.selectedCards.Add(card);
        CheckRule();
    }
    private void CheckRule()
    {
        if (gameData.selectedCards.Count == 2)
        {
            if (gameData.selectedCards[0].spriteId== gameData.selectedCards[1].spriteId)
            {
                gameData.selectedCards[0].OnMatched();
                gameData.selectedCards[1].OnMatched();
                GameActionManager.Instance.UpdateScrore?.Invoke(true);
                AudioManager.Instance.PlayEffect(audioData.cardMatch);

            }
            else
            {
                gameData.selectedCards[0].FlipToHide();
                gameData.selectedCards[1].FlipToHide();
                GameActionManager.Instance.UpdateScrore?.Invoke(false);
                AudioManager.Instance.PlayEffect(audioData.cardMismatch);
            }
            gameData.selectedCards.Clear();
        }
        bool isLevelCleared = gameData.gameCards.All(card => card.isCardMatched);
        if(isLevelCleared)
        {
            GameActionManager.Instance.displayScore?.Invoke(onDisplayScoreComplete);
            GameActionManager.Instance.onLevelComplete?.Invoke();
            cardSprites.Clear();
            AudioManager.Instance.PlayEffect(audioData.gameWin);
        }
    }
    private void onDisplayScoreComplete()
    {
       GameActionManager.Instance.showLoading?.Invoke();
    }
    private void onLoadingComplete()
    {
        Debug.Log("Loading Complete");
        GameActionManager.Instance.onLevelReset();
        GameActionManager.Instance.generateLevel?.Invoke(PlayerManager.Instance.currentLevel);
    }
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleBG();
    }
    public void ToggleSound()
    {
        AudioManager.Instance.ToggleFX();
    }
}
