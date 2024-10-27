using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CardDta", menuName = "Data/CardDta", order = 1)]
public class CardDta : ScriptableObject
{
   [SerializeField] private List<Sprite> AllSprites = new List<Sprite>();
    public List<CardSpriteData> cardSprites = new List<CardSpriteData>();
    public void FillCardData()
    {
        for (int spriteIndex = 0; spriteIndex < AllSprites.Count; spriteIndex++)
        {
            CardSpriteData cardSpriteData = new CardSpriteData();
            cardSpriteData.pairid = spriteIndex;
            cardSpriteData.cardSprite = AllSprites[spriteIndex];
            cardSprites.Add(cardSpriteData);
        }
    }
    public void OnReset()
    {
        cardSprites.Clear();
    }
}

[Serializable]
public class CardSpriteData
{
    public int pairid;
    public Sprite cardSprite;
}