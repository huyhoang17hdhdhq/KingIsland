using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundButton : MonoBehaviour
{
    [Header("Danh sách các ButtonOption")]
    public List<Button> buttons = new List<Button>();

    [Header("Danh sách các ButtonExit")]
    public List<Button> buttonsExit = new List<Button>();

    [Header("Audio phát khi bấm nút")]
    public AudioSource clickAudioSource;

    [Header("Audio phát khi exit")]
    public AudioSource exitAudioSource; // ← SỬA TÊN BIẾN CHO ĐÚNG

    private void Start()
    {
        if (clickAudioSource == null)
        {
            Debug.LogWarning("Chưa gán AudioSource cho click!");
            return;
        }

        // Gán cho tất cả button "Option"
        foreach (Button btn in buttons)
        {
            if (btn != null)
                btn.onClick.AddListener(PlayClickSound);
        }

        // Gán cho tất cả button "Exit"
        foreach (Button btn in buttonsExit)
        {
            if (btn != null)
                btn.onClick.AddListener(PlayExitSound);
        }
    }

    private void PlayClickSound()
    {
        if (clickAudioSource != null)
        {
            if (clickAudioSource.isPlaying)
                clickAudioSource.Stop();
            clickAudioSource.Play();
        }
    }

    private void PlayExitSound()
    {
        if (exitAudioSource != null)
        {
            if (exitAudioSource.isPlaying)
                exitAudioSource.Stop();
            exitAudioSource.Play();
        }
    }
}