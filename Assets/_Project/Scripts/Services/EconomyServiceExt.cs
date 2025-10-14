using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

public static class EconomyServiceExt
{
    /// <summary>
    /// Returns full Economy catalog items (v3 static API).
    /// </summary>
    public static async Task<List<InventoryItemDefinition>> GetCatalogAsync()
    {
        try
        {
            // Sync configuration first (replaces GetInventoryItemsAsync)
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            
            // Then get inventory items synchronously from cached config
            var items = EconomyService.Instance.Configuration.GetInventoryItems();
            return items;
        }
        catch (EconomyException e)
        {
            Debug.LogError($"[Economy] GetCatalogAsync failed: {e.Message}");
            return new List<InventoryItemDefinition>();
        }
    }

    /// <summary>
    /// Checks if player owns a catalog item by ConfigurationId.
    /// </summary>
    public static async Task<bool> OwnsAsync(string itemId)
    {
        try
        {
            var inv = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
            foreach (var i in inv.PlayersInventoryItems)
                if (i.InventoryItemId == itemId) return true;
            return false;
        }
        catch (EconomyException e)
        {
            Debug.LogError($"[Economy] OwnsAsync failed: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// MVP: grants an item directly (for test). Replace with IAP purchase in production.
    /// </summary>
    public static async Task<bool> GrantAsync(string itemId)
    {
        try
        {
            await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(itemId);
            return true;
        }
        catch (EconomyException e)
        {
            Debug.LogError($"[Economy] GrantAsync failed: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Example: remove ads flag via Cloud Save or Economy inventory pattern (stub).
    /// </summary>
    public static async Task<bool> HasRemoveAdsAsync()
    {
        return await OwnsAsync("RemoveAds");
    }
}
