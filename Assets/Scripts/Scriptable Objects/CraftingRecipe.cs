using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Ammo Output Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public List<CraftingIngredient> ingredients;
    public AmmoType resultAmmoType;
    public int resultAmount = 1;
    public Sprite icon;
}


[System.Serializable]
public class CraftingIngredient
{
    public enum IngredientType { CraftingMaterial, Ammo }
    public IngredientType type;
    public CraftingType craftingType;
    public AmmoType ammoType;
    public int amount;
}
