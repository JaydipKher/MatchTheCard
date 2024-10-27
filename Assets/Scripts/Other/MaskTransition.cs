using DG.Tweening;
using UnityEngine;

public class MaskTransition : MonoBehaviour
{
    internal float defaultScale = 2;
    private void OnEnable()
    {
        GameActionManager.Instance.showLoading += FadeIn;
        GameActionManager.Instance.hideLoading += FadeOut;
    }
    private void OnDisable()
    {
        if (GameActionManager.Instance == null) return;
        GameActionManager.Instance.showLoading -= FadeIn;
        GameActionManager.Instance.hideLoading -= FadeOut;
    }
    public void FadeOut()
    {
        transform.gameObject.SetActive(true);
        transform.DOScale(100f, 1f).SetEase(Ease.Linear).SetDelay(0.1f)
        .OnComplete(() =>
        {
            GameActionManager.Instance.hideLoadingComplete?.Invoke();
        })
        .OnUpdate(() =>
        {
            if (transform.transform.localScale.x > defaultScale)
            {
                transform.Find("BlackImage").gameObject.SetActive(false);
            }
        });
    }

    public void FadeIn()
    {
        transform.gameObject.SetActive(true);

        transform.DOScale(defaultScale, 1f).SetEase(Ease.Linear).SetDelay(0.1f)
        .OnComplete(() =>
        {
            transform.Find("BlackImage").gameObject.SetActive(true);
            GameActionManager.Instance.showLoadingComplete?.Invoke();
        });
    }
}
