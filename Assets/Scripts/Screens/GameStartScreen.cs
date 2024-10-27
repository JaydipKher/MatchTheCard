using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class GameStartScreen : MonoBehaviour
{
    [SerializeField]private TMP_Text textComponent;
    [SerializeField]private float duration = 1f;
    [SerializeField]private CanvasGroup playButton;
    [SerializeField]private Color txtColor;

    [SerializeField] private Canvas GamePlayScreen;
    
    private void OnEnable()
    {
        OnStartGame();
    }
    public void OnStartGame()
    {
        textComponent.alpha = 0f;
        playButton.alpha = 0f;
        textComponent.color = Color.yellow;
        textComponent.transform.localScale = Vector3.zero;
        textComponent.DOFade(1f, duration);
        textComponent.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBounce);
        textComponent.DOColor(txtColor,duration).SetLoops(-1, LoopType.Yoyo);
        playButton.DOFade(1f, 2);
    }
    public void OnPlay()
    {
        GameActionManager.Instance.generateLevel?.Invoke(PlayerManager.Instance.currentLevel);
        GamePlayScreen.enabled=true;
        this.enabled = false;
    }
}
