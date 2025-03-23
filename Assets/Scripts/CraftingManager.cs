using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public void TryCraft(CraftingRecipe recipe)
    {
        if (!HasRequirements(recipe))
        {
            Debug.Log("Not enough resources to craft.");
            return;
        }

        ConsumeIngredients(recipe);
        InventorySystem.Instance.AddAmmo(recipe.resultAmmoType, recipe.resultAmount);
        Debug.Log($"Crafted {recipe.resultAmmoType} x{recipe.resultAmount}");


        //slow method to update UI, lets hope the player doesnt break it
        FindObjectOfType<ConsumablesUIManager>()?.UpdateConsumablesUI();
        FindObjectOfType<CraftingUIManager>()?.UpdateCraftingUI();
    }

    private bool HasRequirements(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            switch (ingredient.type)
            {
                case CraftingIngredient.IngredientType.CraftingMaterial:
                    if (!InventorySystem.Instance.GetCraftingInventory().TryGetValue(ingredient.craftingType, out int matAmt) || matAmt < ingredient.amount)
                        return false;
                    break;
                case CraftingIngredient.IngredientType.Ammo:
                    if (!InventorySystem.Instance.GetAmmoInventory().TryGetValue(ingredient.ammoType, out int ammoAmt) || ammoAmt < ingredient.amount)
                        return false;
                    break;
            }
        }
        return true;
    }

    private void ConsumeIngredients(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            switch (ingredient.type)
            {
                case CraftingIngredient.IngredientType.CraftingMaterial:
                    InventorySystem.Instance.UseCraftingMaterial(ingredient.craftingType, ingredient.amount);
                    break;
                case CraftingIngredient.IngredientType.Ammo:
                    InventorySystem.Instance.UseAmmo(ingredient.ammoType, ingredient.amount);
                    break;
            }
        }
    }
}
