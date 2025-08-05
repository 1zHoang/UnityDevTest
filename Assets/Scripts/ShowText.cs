using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public static ShowText Instance;
    public bool isShowTu = true;
    public GameObject tuTextBg;
    public Text tuText;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowTextTu(string text, Action completed = null)
    {
        if (!isShowTu) return;
        tuTextBg.SetActive(true);
        tuText.text = text;
        tuTextBg.transform.DOKill();
        tuTextBg.transform.localScale = new Vector3(1f, 0, 1f);
        tuTextBg.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            completed?.Invoke();
        });
    }

    public void HideTextTu(float delayTime)
    {
        if (!isShowTu) return;
        StartCoroutine(OnHideTexTu(delayTime));
    }
    IEnumerator OnHideTexTu(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        tuTextBg.SetActive(false);
    }
}
