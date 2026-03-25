using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Money : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = GameManager.Instance.Money.ToString();
    }
}
