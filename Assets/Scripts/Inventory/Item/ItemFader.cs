using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    //Gradually recover to original colour

    public void FadeIn()
    {
        Color targetColour= new Color(1, 1, 1, 1);
        spriteRenderer.DOColor(targetColour, Settings.itemFadeDuration);
    }

    //Gradually change the transparency of the sprite
    public void FadeOut()
    {
        Color targetColour = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRenderer.DOColor(targetColour, Settings.itemFadeDuration);
    }
}
