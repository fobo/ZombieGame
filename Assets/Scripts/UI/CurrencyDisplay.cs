using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    private TMP_Text currencyText;

    void Awake()
    {
        currencyText = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        MetaProgression.OnCurrencyChanged += UpdateCurrencyText;
        UpdateCurrencyText(); // Show current value immediately
    }

    void OnDisable()
    {
        MetaProgression.OnCurrencyChanged -= UpdateCurrencyText;
    }

    void UpdateCurrencyText()
    {
        currencyText.text = $"Souls: {MetaProgression.GetCurrency()}";
    }
}
