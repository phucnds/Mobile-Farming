using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    

    private void Start()
    {
        int loaded = PlayerPrefs.GetInt("loaded");
        if (loaded > 0)
        {
            StartCoroutine(FadeCorotineOut());
            PlayerPrefs.SetInt("loaded", 0);

        }
    }

    [NaughtyAttributes.Button]
    public void LoadScenes()
    {
        PlayerPrefs.SetInt("loaded", 1);
        StartCoroutine(FadeCorotineIn());
    }

    IEnumerator FadeCorotineIn()
    {
        float fade = 0;
        while (fade <= 1.5)
        {
            fade += Time.deltaTime * 2;
            canvasGroup.alpha = fade;
            yield return null;
        }

        SceneManager.LoadScene(1);

    }

    IEnumerator FadeCorotineOut()
    {
        float fade = 0;
        while (fade <= 1)
        {
            fade += Time.deltaTime * 2;
            canvasGroup.alpha = 1 - fade;
            yield return null;
        }
    }


    


   
}
