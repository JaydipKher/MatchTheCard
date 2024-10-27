using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public int cardId;
    public int spriteId;
    [SerializeField] private Image cardImage;

    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float rotationValue= 180;
    [SerializeField] private AudioData audioData;
    public bool isCardSpriteSet;
    public bool isCardMatched;
    public RectTransform rectTransform;
    private Button currentButton;

    private void OnEnable() {
        currentButton=GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetCardImage(Sprite _cardImage,int _spriteID)
    {
        cardImage.sprite = _cardImage;
        spriteId = _spriteID;
    }
    public void OnCardClick()
    {
        FlipToShow();
        AudioManager.Instance.PlayEffect(audioData.cardFlip);
    }
    public void FlipToShow()
    {
        rectTransform.DORotate(new Vector3(0, rotationValue, 0), duration).OnUpdate(OnFlipAnimationUpdate).OnComplete(OnShowComplete);
    }
    public void FlipToHide()
    {
        rectTransform.DORotate(new Vector3(0, 0, 0),duration).OnUpdate(() =>
        {
            cardImage.enabled = false;
        }).OnComplete(() =>
        {
            GameActionManager.Instance.hideLoadingComplete?.Invoke();
        });
    }
    public void OnStartShow()
    {
        currentButton.interactable = false;
        rectTransform.DORotate(new Vector3(0, rotationValue, 0), duration).OnUpdate(OnFlipAnimationUpdate).OnComplete(OnStartShowComplete);
    }
    public void OnMatched()
    {
        isCardMatched = true;
        rectTransform.DOScale(Vector3.zero, duration).OnComplete(() =>
        {
        });
    }
    private void OnFlipAnimationUpdate()
    {
        if (rectTransform.rotation.eulerAngles.y >= rotationValue / 2)
        {
            cardImage.enabled = true;
        }
    }
    private void OnShowComplete()
    {
        GameActionManager.Instance.onShowComplete?.Invoke(this);
    }
    private void OnStartShowComplete()
    {
        Invoke("FlipToHide", 1.5f);
        currentButton.interactable = true;
    }
}
