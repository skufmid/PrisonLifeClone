using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeechBubble : MonoBehaviour
{
    Prisoner prisoner;
    [SerializeField] Image fill;
    [SerializeField] Vector3 defaultPosition;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] TextMeshProUGUI amountText;

    private void Awake()
    {
        prisoner = GetComponentInParent<Prisoner>();
    }

    private void Start()
    {
        UpdateSlider();
        UpdateText();
    }

    private void Update()
    {
        UpdateRotate();
    }

    public void UpdateSlider()
    {
        if (prisoner.RemainingHandcuff < 0) return;
        if (prisoner.RemainingHandcuff == 0)
        {
            GetComponent<Canvas>().enabled = false;
        }

        float t = 1f - ((float)prisoner.RemainingHandcuff / prisoner.RequiredHandcuff);
        t = Mathf.Clamp01(t);


        fill.rectTransform.anchoredPosition =
            Vector2.Lerp(defaultPosition, targetPosition, t);
    }

    public void UpdateText()
    {
        amountText.text = prisoner.RemainingHandcuff.ToString();
    }

    private void UpdateRotate()
    {
        // 카메라가 바라보는 방향을 바라보기
        Vector3 cameraForward = Camera.main.transform.forward;

        if (cameraForward.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = targetRotation;
        }
    }
}
