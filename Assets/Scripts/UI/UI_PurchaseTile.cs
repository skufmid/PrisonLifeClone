using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_PurchaseTile : MonoBehaviour
{
    PurchaseTile purchaseTile;
    [SerializeField] Image fill;
    [SerializeField] Vector3 defaultPosition;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] TextMeshProUGUI amountText;

    private void Awake()
    {
        purchaseTile = GetComponentInParent<PurchaseTile>();
    }

    private void Start()
    {
        UpdateSlider();
        UpdateText();
    }

    public void UpdateSlider()
    {
        if (purchaseTile.Price <= 0) return;

        float t = 1f - ((float)purchaseTile.RemainingAmount / purchaseTile.Price);
        t = Mathf.Clamp01(t);


        fill.rectTransform.anchoredPosition =
            Vector2.Lerp(defaultPosition, targetPosition, t);
    }

    public void UpdateText()
    {
        amountText.text = purchaseTile.RemainingAmount.ToString();
    }
}
