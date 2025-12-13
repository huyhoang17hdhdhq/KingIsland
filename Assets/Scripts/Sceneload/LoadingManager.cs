using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Image fill loading")]
    public Image loadingFillImage;

    [Header("Text hiển thị %")]
    public TMP_Text percentText;

    [Header("Danh sách text loading")]
    public List<TMP_Text> loadingTexts;

    [Header("Thời gian hiển thị mỗi text")]
    public List<float> textDurations;

    [Header("FX khi loading")]
    public ParticleSystem loadingFX;

    [Header("Tên scene cần load")]
    public string sceneToLoad = "GameScene";

    [Header("Thời gian chờ sau khi load xong")]
    public float waitAfterLoad = 0.5f;

    private bool startFill = false;

    private void Start()
    {
        
        if (loadingFillImage != null)
            loadingFillImage.fillAmount = 0f;

        if (percentText != null)
            percentText.gameObject.SetActive(false);

        
        StartCoroutine(PlayLoadingTexts());
    }

    
    private IEnumerator PlayLoadingTexts()
    {
        
        foreach (var t in loadingTexts)
            if (t != null) t.gameObject.SetActive(false);

       
        for (int i = 0; i < loadingTexts.Count; i++)
        {
            if (loadingTexts[i] != null)
            {
                loadingTexts[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(textDurations[i]);
                loadingTexts[i].gameObject.SetActive(false);
            }
        }

        
        startFill = true;

        if (percentText != null)
            percentText.gameObject.SetActive(true);

        
        StartCoroutine(LoadSceneAsync());
    }

  
    private IEnumerator LoadSceneAsync()
    {
        if (loadingFX != null)
            loadingFX.Play();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (startFill)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                if (loadingFillImage != null)
                    loadingFillImage.fillAmount = progress;

                if (percentText != null)
                    percentText.text = Mathf.RoundToInt(progress * 100f) + "%";

                if (operation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(waitAfterLoad);
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
