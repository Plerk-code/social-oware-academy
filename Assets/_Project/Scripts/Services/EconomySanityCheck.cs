using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;

public class EconomySanityCheck : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Sync configuration first (replaces GetInventoryItemsAsync)
        await EconomyService.Instance.Configuration.SyncConfigurationAsync();
        
        // Then get inventory items synchronously from cached config
        var catalog = EconomyService.Instance.Configuration.GetInventoryItems();
        Debug.Log($"[Economy Sanity] Catalog items: {catalog.Count}");

        var inventory = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
        Debug.Log($"[Economy Sanity] Inventory items: {inventory.PlayersInventoryItems.Count}");
    }
}
