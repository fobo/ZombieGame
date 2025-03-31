using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    public Sprite icon;
    public string upgradeName;
    public int maxLevel;
    public int baseCost;
    public string upgradeId;


}
