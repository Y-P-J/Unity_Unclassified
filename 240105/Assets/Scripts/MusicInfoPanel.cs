using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicInfoPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField titleField;
    [SerializeField] TMP_InputField artistField;
    [SerializeField] TMP_InputField bpmField;

    Action onCallback;
    public void ShowInfoPanel(Action onCallback)
    {
        this.onCallback = onCallback;

        titleField.text = string.Empty;
        artistField.text = string.Empty;
        bpmField.text = string.Empty;

        gameObject.SetActive(true);
    }

    // 확인 버튼.
    public void OnConfirm()
    {
        MusicSheet.Instance.musicData.title = titleField.text;
        MusicSheet.Instance.musicData.artist = artistField.text;
        MusicSheet.Instance.musicData.bpm = int.Parse(bpmField.text);
        MusicSheet.Instance.Save();

        onCallback?.Invoke();
        gameObject.SetActive(false);
    }
}
