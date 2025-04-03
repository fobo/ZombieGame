public static class MetaStats
{
    private static int healthBonus;
    private static int apUpgradeBonus;
    private static float critChanceBonus;
    private static float critDamageBonus;
    private static int damageBonus;
    private static float fireRateBonus;
    private static int luckBonus;
    private static float reloadSpeedBonus;
    private static float spreadBonus;
    private static float stoppingPowerBonus;
    private static float moveSpeedBonus;

    private static bool isInitialized = false;

    public static void Initialize()
    {
        // This should be called once at game start (e.g., scene load or menu transition)
        isInitialized = true;

        healthBonus = MetaProgression.GetUpgradeLevel("max_health") * 5;
        apUpgradeBonus = MetaProgression.GetUpgradeLevel("ap_upgrade") * 1;
        critChanceBonus = MetaProgression.GetUpgradeLevel("crit_chance") * 0.02f;
        critDamageBonus = MetaProgression.GetUpgradeLevel("crit_damage") * 0.10f;
        damageBonus = MetaProgression.GetUpgradeLevel("damage") * 2;
        fireRateBonus = MetaProgression.GetUpgradeLevel("fire_rate") * 0.10f;
        luckBonus = MetaProgression.GetUpgradeLevel("luck") * 1;
        reloadSpeedBonus = MetaProgression.GetUpgradeLevel("reload_speed") * 0.10f;
        spreadBonus = MetaProgression.GetUpgradeLevel("spread") * 0.05f;
        stoppingPowerBonus = MetaProgression.GetUpgradeLevel("stopping_power") * 0.10f;
        moveSpeedBonus = MetaProgression.GetUpgradeLevel("movespeed") * 0.2f;
    }

    private static void EnsureInitialized()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    public static int GetMetaHealthIncrease() { EnsureInitialized(); return healthBonus; }
    public static int GetMetaAPBonus() { EnsureInitialized(); return apUpgradeBonus; }
    public static float GetMetaCritChanceBonus() { EnsureInitialized(); return critChanceBonus; }
    public static float GetMetaCritDamageBonus() { EnsureInitialized(); return critDamageBonus; }
    public static int GetMetaDamageBonus() { EnsureInitialized(); return damageBonus; }
    public static float GetMetaFireRateBonus() { EnsureInitialized(); return fireRateBonus; }
    public static int GetMetaLuckBonus() { EnsureInitialized(); return luckBonus; }
    public static float GetMetaReloadSpeedBonus() { EnsureInitialized(); return reloadSpeedBonus; }
    public static float GetMetaSpreadBonus() { EnsureInitialized(); return spreadBonus; }
    public static float GetMetaStoppingPowerBonus() { EnsureInitialized(); return stoppingPowerBonus; }
    public static float GetMetaSpeedBonus() { EnsureInitialized(); return moveSpeedBonus; }
}
