using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    [SerializeField] private new Animation animation;
    [SerializeField] private SpriteRenderer[] unitSpriteRenderers;

    public SpriteRenderer[] UnitSpriteRenderers => unitSpriteRenderers;

    public void Show(Action onComplete)
    {
        StartCoroutine(ShowRoutine(onComplete));
    }

    private IEnumerator ShowRoutine(Action onComplete)
    {
        animation.Play();

        while (animation.isPlaying)
        {
            yield return null;
        }

        onComplete?.Invoke();
        Destroy(this.gameObject);
    }
}
