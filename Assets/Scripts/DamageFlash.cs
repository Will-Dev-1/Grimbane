using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine damageFlashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void CallDamageFlash()
    {
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    private void SetFlashColor()
    {
        material.SetColor("_FlashColor", flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        material.SetFloat("_FlashAmount", amount);
    }
}
