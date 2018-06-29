using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TC;

public class SampleAudioManager : MonoBehaviour
{

    [SerializeField]
    private Transform bgmButtonParent, seButtonParent1, seButtonParent2;
    [SerializeField]
    private Button buttonPrefab;


    // Use this for initialization
	void Start () {
        for(int i = 0; i < SampleAudioData.BGMResourceFilePaths.Length; i++)
        {
            string audioFilePath = SampleAudioData.BGMResourceFilePaths[i];
            Button button = CreateButton(i.ToString(), () =>
            {
                AudioManager.Instance.PlayBGM(audioFilePath);
            });
            button.transform.SetParent(bgmButtonParent.transform);
        }
        for(int i = 0; i < SampleAudioData.SEResourceFilePaths.Length; i++)
        {
            string audioFilePath = SampleAudioData.SEResourceFilePaths[i];
            Button button = CreateButton(i.ToString(), () =>
            {
                AudioManager.Instance.PlaySE(audioFilePath);
            });
            button.transform.SetParent(
                SampleAudioData.SEResourceFilePaths.Length / 2 >= i
                ? seButtonParent1.transform
                : seButtonParent2.transform);
        }

//        AudioManager.CreateIfNeed();
	}

        
    private Button CreateButton(string _title, UnityEngine.Events.UnityAction _onClickCallback)
    {
        Button button = GameObject.Instantiate(buttonPrefab);
        button.gameObject.name = _title;
        button.GetComponentInChildren<Text>().text = _title;
        button.onClick.AddListener(_onClickCallback);
        return button;
    }
}
