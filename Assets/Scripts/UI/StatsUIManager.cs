using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class StatsUIManager : MonoBehaviour
{
    public GameObject statRowPrefab;         // Assign in Inspector
    public Transform statsPanel;             // Parent panel to populate



    private void OnEnable()
    {
        TryUpdateStatsUI();
    }



    private IEnumerator Start()
    {
        Debug.Log("START METHOD - Momento System is " + MomentoSystem.Instance + " || HUD Instance is " + HUDController.Instance);
        yield return new WaitUntil(() => MomentoSystem.Instance != null);
        UpdateStatsUI();
    }


    // private void OnDisable()
    // {
    //     Debug.Log("StatsUIManager disabled. Unsubscribing from events.");

    //     HUDController.OnHUDReady -= TryUpdateStatsUI;
    //     MomentoSystem.OnMomentoReady -= TryUpdateStatsUI;
    // }


    private void TryUpdateStatsUI()
    {
        Debug.Log("Trying to update stats");
        Debug.Log("Momento System is " + MomentoSystem.Instance + " || HUD Instance is " + HUDController.Instance);
        if (MomentoSystem.Instance != null && HUDController.Instance != null)
            UpdateStatsUI();
    }

    private void AddHealthStat()
    {
        float current = HUDController.Instance?.GetHealth() ?? 0f;
        float max = HUDController.Instance?.GetMaxHealth() ?? 1f; // Avoid divide by zero

        AddStat("Health", $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}");
    }



    public void UpdateStatsUI()
    {
        Debug.Log("RUNNING UPDATE STATS UI");

        foreach (Transform child in statsPanel)
        {
            Destroy(child.gameObject);
        }

        AddHealthStat();


        AddModifier("Damage", MomentoSystem.Instance.GetDamageMultiplier());
        AddModifier("Armor Penetration", MomentoSystem.Instance.GetAPMultiplier());
        AddReverseModifier("Fire Rate", MomentoSystem.Instance.GetFireRateMultiplier());
        AddReverseModifier("Reload Speed", MomentoSystem.Instance.GetReloadSpeedMultiplier());
        AddReverseModifier("Spread", MomentoSystem.Instance.GetSpreadMultiplier());
        //AddModifier("Movement Speed", MomentoSystem.Instance.GetMoveSpeedMultiplier()); //not yet functional
        //AddModifier("Treasure Quality", MomentoSystem.Instance.GetTreasureClassMultiplier()); //not yet functional
        AddModifier("Luck", MomentoSystem.Instance.GetLuckMultiplier());
        AddModifier("Critical Chance", MomentoSystem.Instance.GetCriticalChanceMultiplier());
        AddModifier("Critical Damage", MomentoSystem.Instance.GetCriticalDamageMultiplier());
        AddModifier("Stopping Power", MomentoSystem.Instance.GetStoppingPowerMultiplier());
        Debug.Log("Luck" + MomentoSystem.Instance.GetLuckMultiplier());
    }

    private void AddModifier(string statName, float multiplier)
    {
        int percent = Mathf.RoundToInt(multiplier * 100f);
        string color = percent > 100 ? "green" : percent < 100 ? "red" : "white";

        AddStat(statName, $"<color={color}>{percent}%</color>");
    }

    private void AddReverseModifier(string statName, float multiplier)
    {
        int percent = Mathf.RoundToInt(100f / multiplier); // Invert the multiplier for display
        string color = percent > 100 ? "green" : percent < 100 ? "red" : "white";

        AddStat(statName, $"<color={color}>{percent}%</color>");
    }




    private void AddStat(string name, string value)
    {
        GameObject statRow = Instantiate(statRowPrefab, statsPanel);
        statRow.transform.Find("StatNameText").GetComponent<TMP_Text>().text = name;
        statRow.transform.Find("StatValueText").GetComponent<TMP_Text>().text = value;

        // Optional: Add tooltips later
    }
}
