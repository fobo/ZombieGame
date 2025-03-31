using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    public CraftingRecipe recipe;

    private void Start()
    {
        SetupUI();

        Button craftBtn = transform.Find("CraftButton")?.GetComponent<Button>();
        if (craftBtn != null)
        {
            craftBtn.onClick.AddListener(() =>
            {
                FindObjectOfType<CraftingManager>()?.TryCraft(recipe);
            });
        }
    }

    private void SetupUI()
    {



        for (int i = 0; i < 4; i++)
        {
            var iconHolder = transform.Find($"Material{i + 1}")?.GetComponent<Image>();
            var amountText = transform.Find($"Material{i + 1}/amountText")?.GetComponent<TMP_Text>(); // Use TMP if applicable

            if (iconHolder == null || amountText == null) continue;

            if (i < recipe.ingredients.Count)
            {
                var ingredient = recipe.ingredients[i];
                iconHolder.sprite = GetIngredientIcon(ingredient);
                iconHolder.enabled = true;

                amountText.text = $"x{ingredient.amount}";
                amountText.enabled = true;
            }
            else
            {
                iconHolder.enabled = false;
                amountText.enabled = false;
            }
        }

        var outputIcon = transform.Find("OutputIcon")?.GetComponent<Image>();
        if (outputIcon != null)
        {
            outputIcon.sprite = InventorySystem.Instance.GetAmmoIcon(recipe.resultAmmoType);
        }
    }


    private Sprite GetIngredientIcon(CraftingIngredient ingredient)
    {
        return ingredient.type switch
        {
            CraftingIngredient.IngredientType.CraftingMaterial =>
                InventorySystem.Instance.GetCraftingIcon(ingredient.craftingType),
            CraftingIngredient.IngredientType.Ammo =>
                InventorySystem.Instance.GetAmmoIcon(ingredient.ammoType),
            _ => null
        };
    }
}
