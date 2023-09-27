using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System;


public class NowLoadingAnimation: MonoBehaviour
{
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private GameObject text3;

    [SerializeField] private float delayTime = 2f;

    private void Start()
    {
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
    }

    public async void TextShow()
    {
        text1.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // ë“ã@èàóù

        text2.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // ë“ã@èàóù

        text3.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // ë“ã@èàóù
    }
}
