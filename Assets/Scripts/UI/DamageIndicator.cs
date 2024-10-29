 using System.Collections;
 using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]private Image image;
    
    [Header("UI Settings")]
    [SerializeField]private float flashSpeed;


    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.player.condition.OnDamgeEvent += Flash;
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        image.enabled = false;
    }
}