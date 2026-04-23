using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int index = 0;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeBlackScreen;
    [SerializeField] private float fadeTimeDuration = 0.5f;

    private bool isFading = false;

    public void LoadScene()
    {
        if (!isFading)
        {
            StartCoroutine(FadeThenLoad());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StartCoroutine(FadeThenLoad());
        }
    }

    private IEnumerator FadeThenLoad()
    {
        isFading = true;

        fadeBlackScreen.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(0f, 1f));
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(index);
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;
        fadeBlackScreen.alpha = start;

        while (elapsed < fadeTimeDuration)
        {
            elapsed += Time.deltaTime;
            fadeBlackScreen.alpha = Mathf.Lerp(start, end, elapsed / fadeTimeDuration);
            yield return null;
        }

        fadeBlackScreen.alpha = end;
    }
}