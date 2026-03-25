using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_JailCapacityText : MonoBehaviour
{
    JailQueue jail;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        jail = GetComponentInParent<JailQueue>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = $"{jail.Count} / {jail.MaxCapacity}";
        text.color = jail.HasSpace ? Color.white : Color.red;
    }
}
