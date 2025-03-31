public static class MetaStats
{
    private static int healthBonus;

    private static bool isInitialized = false;

    public static void Initialize()
    {
        // This should be called once at game start (e.g., scene load or menu transition)
        isInitialized = true;

        int healthUpgradeLevel = MetaProgression.GetUpgradeLevel("max_health");
        healthBonus = healthUpgradeLevel * 5;

        //add more
    }

    public static int GetMetaHealthIncrease()
    {
        if (!isInitialized)
        {
            Initialize(); // Optional safety auto-init
        }

        return healthBonus;
    }
}
