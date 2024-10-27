using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelConfiguration levelConfiguration;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameData gameData;
    [SerializeField] private CardDta cardData;
    private RectTransform gridRect;
    private LevelInfo currentlevelInfo;
    private void OnEnable()
    {
        Application.targetFrameRate = 90;
        cardData.FillCardData();
        gridRect = gridLayoutGroup.GetComponent<RectTransform>();
        GameActionManager.Instance.generateLevel += OnLevelGenerator;
        GameActionManager.Instance.onLevelReset += RestLevel;
    }
    private void OnDisable()
    {
        if (GameActionManager.Instance == null) return;
        GameActionManager.Instance.generateLevel -= OnLevelGenerator;
        GameActionManager.Instance.onLevelReset -= RestLevel;
    }
    public void OnLevelGenerator(int _levelNumber)
    {
        currentlevelInfo = levelConfiguration.levelInfo[_levelNumber];
        gridLayoutGroup.constraintCount = currentlevelInfo.columns;
        GenerateLevel(currentlevelInfo.rows, currentlevelInfo.columns);
    }
    private void GenerateLevel(int _row, int _column)
    {
        int totalCount = _row * _column;
        for (int cardIndex = 0; cardIndex < totalCount; cardIndex++)
        {
            Card card = Instantiate(cardPrefab, cardContainer);
            card.cardId = cardIndex;
            gameData.gameCards.Add(card);
        }
        Invoke("AdjustGridSize", 0.1f);
    }
    private void AdjustGridSize()
    {
        RectTransform rectTransform = cardContainer.GetComponent<RectTransform>();
        float availableWidth = rectTransform.rect.width;
        float availableHeight = rectTransform.rect.height;

        // Exit if dimensions are non-positive (e.g., when layout hasn't updated yet)
        if (availableWidth <= 0 || availableHeight <= 0)
        {
            Debug.LogWarning("Container dimensions are not valid. Ensure layout update before adjusting grid size.");
            return;
        }

        int rowCount = currentlevelInfo.rows;    // Number of rows
        int columnCount = currentlevelInfo.columns;  // Number of columns

        // Calculate the effective available width and height after accounting for padding and spacing
        float effectiveWidth = availableWidth - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right - (gridLayoutGroup.spacing.x * (columnCount - 1));
        float effectiveHeight = availableHeight - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom - (gridLayoutGroup.spacing.y * (rowCount - 1));

        // Avoid dividing by zero or ending up with negative cell sizes
        if (rowCount <= 0 || columnCount <= 0 || effectiveWidth <= 0 || effectiveHeight <= 0)
        {
            Debug.LogWarning("Effective grid dimensions are invalid.");
            return;
        }

        // Calculate the cell size by choosing the smaller size to keep cells square
        float cellWidth = effectiveWidth / columnCount;
        float cellHeight = effectiveHeight / rowCount;
        float cellSize = Mathf.Min(cellWidth, cellHeight);

        // Apply the calculated cell size
        gridLayoutGroup.cellSize = new Vector2(Mathf.RoundToInt(cellSize), Mathf.RoundToInt(cellSize));
        GameActionManager.Instance.levelGenerated?.Invoke();
    }

    private void RestLevel()
    {
        gameData.isGameStarted = false;
        for (int i = 0; i < cardContainer.childCount; i++)
        {
            Destroy(cardContainer.GetChild(i).gameObject);
        }
        gameData.gameCards.Clear();
        gameData.gameCards.ForEach(card => card.isCardSpriteSet = false);


    }
    private void OnDestroy()
    {
        gameData.gameCards.Clear();
        gameData.selectedCards.Clear();
        gameData.gameCards.ForEach(card => card.isCardSpriteSet = false);
        cardData.OnReset();

    }
}
