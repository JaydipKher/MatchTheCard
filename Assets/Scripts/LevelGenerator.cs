using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    [SerializeField] private List<Sprite> cardImages;
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private LevelConfiguration levelConfiguration;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Card[] cards;
    private bool gameStart;

    private LevelInfo currentLevelInfo;

    private int spriteSelected;
    private int cardSelected;
    private int cardLeft;

    private void OnEnable()
    {
        //  ActionManager.instance.levelGenerator += OnLevelGenerator;
    }

    private void OnDisable()
    {
        //  ActionManager.instance.levelGenerator -= OnLevelGenerator;
    }

    private void Awake()
    {
        instance = this;
    }

    public void Btn_GameStartClicked()
    {
        OnLevelGenerator(4);
    }

    public void OnLevelGenerator(int _levelNumber)
    {
        if (gameStart)
            return;

        gameStart = true;

        currentLevelInfo = levelConfiguration.levelInfo[_levelNumber];
        gridLayoutGroup.constraintCount = currentLevelInfo.columns;

        GridSize(currentLevelInfo.rows, currentLevelInfo.columns);

        cardSelected = spriteSelected = -1;
        cardLeft = cards.Length;

        SetCardPairs();
        StartCoroutine(CardBackSide());
    }

    private void GridSize(int _row, int _column)
    {
        cards = new Card[_row * _column];

        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                GameObject cardClone = Instantiate(cardPrefab, cardContainer);
                int index = i * _column + j;
                cards[index] = cardClone.GetComponent<Card>();
                cards[index].CardId = index;
            }
        }
    }

    private void SetCardPairs()
    {
        int i, j;
        int[] selectedID = new int[cards.Length / 2];

        for (i = 0; i < cards.Length / 2; i++)
        {
            int value = Random.Range(0, cardImages.Count - 1);

            for (j = i; j > 0; j--)
            {
                if (selectedID[j - 1] == value)
                    value = (value + 1) % cardImages.Count;
            }
            selectedID[i] = value;
        }

        for (i = 0; i < cards.Length; i++)
        {
            cards[i].Active();
            cards[i].SpriteID = -1;
            cards[i].ResetRotation();
        }

        for (i = 0; i < cards.Length / 2; i++)
        {
            for (j = 0; j < 2; j++)
            {
                int value = Random.Range(0, cards.Length - 1);

                while (cards[value].SpriteID != -1)
                    value = (value + 1) % cards.Length;

                cards[value].SpriteID = selectedID[i];
            }
        }
    }

    IEnumerator CardBackSide()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < cards.Length; i++)
            cards[i].Flip();
        yield return new WaitForSeconds(0.5f);
    }

    public Sprite GetSprite(int spriteId)
    {
        return cardImages[spriteId];
    }

    public Sprite CardBack()
    {
        return cardBackSprite;
    }

    public bool canClick()
    {
        return gameStart;
    }

    public void cardClicked(int spriteId, int cardId)
    {
        if (spriteSelected == -1)
        {
            spriteSelected = spriteId;
            cardSelected = cardId;
        }
        else
        {
            if (spriteSelected == spriteId)
            {
                cards[cardSelected].Inactive();
                cards[cardId].Inactive();
                cardLeft -= 2;
                CheckGameWin();
            }
            else
            {
                cards[cardSelected].Flip();
                cards[cardId].Flip();
            }
            cardSelected = spriteSelected = -1;
        }
    }

    private void CheckGameWin()
    {
        if (cardLeft == 0)
        { 
            gameStart = false;
            Debug.LogError("Game Over");
        }
    }
}
