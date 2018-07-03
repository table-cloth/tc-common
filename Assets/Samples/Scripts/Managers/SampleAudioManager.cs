using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TC;

/// <summary>
/// Sample audio manager.
/// </summary>
public class SampleAudioManager : MonoBehaviour
{
    [SerializeField]
    private Transform bgmButtonParent, seButtonParent1, seButtonParent2;
    [SerializeField]
    private Button buttonPrefab;

    /// <summary>
    /// Clears the audio cache.
    /// </summary>
    public void ClearAudioCache()
    {
        AudioManager.Instance.ClearAudioClipCache();
    }

    /// <summary>
    /// Clears the pool.
    /// </summary>
    public void ClearPool()
    {
        AudioManager.Instance.ClearPool();
    }

    /// <summary>
    /// Clears all data.
    /// </summary>
    public void ClearAllData()
    {
        AudioManager.Instance.ClearAllData();
    }

    // Use this for initialization
	protected void Start () {

        // Create buttos and set click events for BGM.
        for(int i = 0; i < SampleAudioData.BGMResourceFilePaths.Length; i++)
        {
            string audioFilePath = SampleAudioData.BGMResourceFilePaths[i];
            string buttonIndex = i.ToString();
            Button button = CreateButton(i.ToString(), () =>
            {
                Debug.Log("Click button BGM : " + buttonIndex);
                AudioManager.Instance.PlayBGM(audioFilePath);
            });
            button.transform.SetParent(bgmButtonParent.transform);
        }

        // Create buttons and set click events for SE.
        for(int i = 0; i < SampleAudioData.SEResourceFilePaths.Length; i++)
        {
            string audioFilePath = SampleAudioData.SEResourceFilePaths[i];
            string buttonIndex = i.ToString();
            Button button = CreateButton(i.ToString(), () =>
            {
                Debug.Log("Click button SE : " + buttonIndex);
                AudioManager.Instance.PlaySE(audioFilePath);
            });
            button.transform.SetParent(
                SampleAudioData.SEResourceFilePaths.Length / 2 >= i
                ? seButtonParent1.transform
                : seButtonParent2.transform);
        }
	}

    /// <summary>
    /// Creates the button.
    /// </summary>
    /// <returns>The button.</returns>
    /// <param name="_title">Title.</param>
    /// <param name="_onClickCallback">On click callback.</param>
    private Button CreateButton(string _title, UnityEngine.Events.UnityAction _onClickCallback)
    {
        Button button = GameObject.Instantiate(buttonPrefab);
        button.gameObject.name = _title;
        button.GetComponentInChildren<Text>().text = _title;
        button.onClick.AddListener(_onClickCallback);
        return button;
    }
}
