using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class UpgradeCardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //public
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text costText;
    public Transform ticksParent;
    public GameObject tickPrefab;
    public Sprite untickedSprite;
    public Sprite tickedSprite;


    //private
    private UpgradeData upgradeData;
    private int currentLevel = 0;
    private Vector3 originalScale;
    private Tween scaleTween;


    private Vector3 hoveredScale;
    private bool isHovered = false;


    void Awake()
    {
        originalScale = transform.localScale;
        hoveredScale = originalScale * 1.1f;
        //RefreshTicks();
    }

    public void Initialize(UpgradeData data, int level)
    {
        upgradeData = data;
        currentLevel = level;

        icon.sprite = data.icon;
        nameText.text = data.upgradeName;
        costText.text = currentLevel >= upgradeData.maxLevel
            ? "MAXED"
            : $"Cost: {data.baseCost * (currentLevel + 1)}";

        RefreshTicks();
    }


    void RefreshTicks()
    {
        foreach (Transform child in ticksParent)
            Destroy(child.gameObject);

        for (int i = 0; i < upgradeData.maxLevel; i++)
        {

            GameObject tick = Instantiate(tickPrefab, ticksParent);
            Image tickImage = tick.GetComponent<Image>();
            tickImage.sprite = i < currentLevel ? tickedSprite : untickedSprite;
            // In RefreshTicks(), inside the for loop:
            if (i == currentLevel - 1)
            {
                // Just upgraded this one
                tickImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TryUpgrade();
    }

    void TryUpgrade()
    {
        if (currentLevel >= upgradeData.maxLevel)
            return;

        int upgradeCost = upgradeData.baseCost * (currentLevel + 1);

        if (!MetaProgression.SpendCurrency(upgradeCost))
        {
            Debug.Log("Not enough currency to upgrade.");
            return;
        }

        currentLevel++;
        MetaProgression.SetUpgradeLevel(upgradeData.upgradeId, currentLevel);

        AnimateScale(isHovered ? hoveredScale : originalScale); // Just apply correct scale

        RefreshTicks();
        costText.text = currentLevel >= upgradeData.maxLevel
            ? "MAXED"
            : $"Cost: {upgradeData.baseCost * (currentLevel + 1)}";
    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        AnimateScale(hoveredScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        AnimateScale(originalScale);
    }

    void AnimateScale(Vector3 target)
    {
        scaleTween?.Kill();
        scaleTween = transform.DOScale(target, 0.2f).SetEase(Ease.InOutSine);
    }

    void OnDestroy()
    {
        transform.DOKill(); // Prevent tweens from running on destroyed objects
    }


}
