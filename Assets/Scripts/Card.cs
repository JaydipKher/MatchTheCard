using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    private int spriteID;
    private int cardId;
    private bool flipped;
    private bool turning;
    [SerializeField] private Image cardImage;
    [SerializeField] private float flipTime;

    private IEnumerator FlipCard(Transform _thisCard, float time, bool changeSprite)
    {
        Quaternion startRotation = _thisCard.rotation;
        Quaternion endRotation = _thisCard.rotation * Quaternion.Euler(new Vector3(0, 90, 0));

        float rate = 1.0f / time;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            _thisCard.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        if (changeSprite)
        {
            flipped = !flipped;
            ChangeSprite();
            StartCoroutine(FlipCard(transform, time, false));
        }
        else
            turning = false;
    }
    public void Flip()
    {
        turning = true;
        //  AudioPlayer.Instance.PlayAudio(0);
        StartCoroutine(FlipCard(transform, flipTime, true));
    }
    private void ChangeSprite()
    {
        if (spriteID == -1 || cardImage == null) return;
        if (flipped)
            cardImage.sprite = LevelGenerator.instance.GetSprite(spriteID);
        else
            cardImage.sprite = LevelGenerator.instance.CardBack();
    }
    public void Inactive()
    {
        StartCoroutine(Fade());
    }
    private IEnumerator Fade()
    {
        float rate = 1.0f / 2.5f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            cardImage.color = Color.Lerp(cardImage.color, Color.clear, t);

            yield return null;
        }
    }
    public void Active()
    {
        if (cardImage)
            cardImage.color = Color.white;
    }
    public int SpriteID
    {
        set
        {
            spriteID = value;
            flipped = true;
            ChangeSprite();
        }
        get { return spriteID; }
    }
    public int CardId
    {
        set { cardId = value; }
        get { return cardId; }
    }
    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        flipped = true;
    }
    public void CardBtn()
    {
        if (flipped || turning)
            return;
        if (!LevelGenerator.instance.canClick())
            return;

        Flip();
        StartCoroutine(SelectionEvent());
    }
    private IEnumerator SelectionEvent()
    {
        yield return new WaitForSeconds(0.5f);
        LevelGenerator.instance.cardClicked(spriteID, cardId);
    }
}
