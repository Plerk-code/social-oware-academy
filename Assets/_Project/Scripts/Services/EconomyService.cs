using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;

namespace Project.Services
{
    /// <summary>
    /// Handles Unity Economy service operations.
    /// Provides currency, inventory, and purchase management.
    /// </summary>
    public class EconomyService
    {
        private const string FREE_SKIN_GRANTED_KEY = "FreeSkinGranted";
        private const string FREE_SKIN_ITEM_ID = "BoardSkin_Classic";

        public async Task Initialize()
        {
            try
            {
                Debug.Log("[Economy] Initializing Economy Service...");
                
                // Economy is automatically initialized with Unity Gaming Services
                
                Debug.Log("[Economy] Economy Service initialized");
                
                // Grant free skin on first run
                await GrantFreeSkinOnFirstRun();
                
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to initialize: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Grants the free BoardSkin_Classic item on first run
        /// </summary>
        private async Task GrantFreeSkinOnFirstRun()
        {
            try
            {
                // Check if free skin has already been granted using PlayerPrefs
                if (PlayerPrefs.HasKey(FREE_SKIN_GRANTED_KEY))
                {
                    Debug.Log("[Economy] Free skin already granted");
                    return;
                }

                // Check if player already owns the free skin
                if (await Owns(FREE_SKIN_ITEM_ID))
                {
                    Debug.Log("[Economy] Player already owns free skin");
                    PlayerPrefs.SetInt(FREE_SKIN_GRANTED_KEY, 1);
                    PlayerPrefs.Save();
                    return;
                }

                // Grant the free skin
                Debug.Log($"[Economy] Granting free skin: {FREE_SKIN_ITEM_ID}");
                await AddInventoryItemAsync(FREE_SKIN_ITEM_ID);
                
                // Mark as granted
                PlayerPrefs.SetInt(FREE_SKIN_GRANTED_KEY, 1);
                PlayerPrefs.Save();
                
                Debug.Log("[Economy] Free skin granted successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to grant free skin: {e.Message}");
                // Don't throw - allow initialization to continue
            }
        }

        public async Task<List<CurrencyDefinition>> GetCurrenciesAsync()
        {
            try
            {
                Debug.Log("[Economy] Fetching currencies...");
                
                // Sync configuration first (replaces GetCurrenciesAsync)
                await Unity.Services.Economy.EconomyService.Instance.Configuration.SyncConfigurationAsync();
                
                // Then get currencies synchronously from cached config
                var currencies = Unity.Services.Economy.EconomyService.Instance.Configuration.GetCurrencies();
                Debug.Log($"[Economy] Retrieved {currencies.Count} currencies");
                
                return currencies;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to get currencies: {e.Message}");
                throw;
            }
        }

        public async Task<List<PlayerBalance>> GetPlayerBalancesAsync()
        {
            try
            {
                Debug.Log("[Economy] Fetching player balances...");
                
                var result = await Unity.Services.Economy.EconomyService.Instance.PlayerBalances.GetBalancesAsync();
                var balances = result.Balances;
                Debug.Log($"[Economy] Retrieved {balances.Count} balances");
                
                foreach (var balance in balances)
                {
                    Debug.Log($"[Economy] Currency: {balance.CurrencyId}, Balance: {balance.Balance}");
                }
                
                return balances;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to get player balances: {e.Message}");
                throw;
            }
        }

        public async Task<List<InventoryItemDefinition>> GetInventoryItemsAsync()
        {
            try
            {
                Debug.Log("[Economy] Fetching inventory items...");
                
                // Sync configuration first (replaces GetInventoryItemsAsync)
                await Unity.Services.Economy.EconomyService.Instance.Configuration.SyncConfigurationAsync();
                
                // Then get inventory items synchronously from cached config
                var items = Unity.Services.Economy.EconomyService.Instance.Configuration.GetInventoryItems();
                Debug.Log($"[Economy] Retrieved {items.Count} inventory items");
                
                return items;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to get inventory items: {e.Message}");
                throw;
            }
        }

        public async Task<PlayersInventoryItem> AddInventoryItemAsync(string itemId)
        {
            try
            {
                Debug.Log($"[Economy] Adding inventory item: {itemId}");
                
                // AddInventoryItemAsync takes the inventory item definition ID
                // PlayersInventoryItemId is auto-generated, so don't set it in options
                var result = await Unity.Services.Economy.EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(itemId);
                Debug.Log($"[Economy] Successfully added {itemId} to inventory");
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to add inventory item: {e.Message}");
                throw;
            }
        }

        public async Task IncrementCurrencyAsync(string currencyId, int amount)
        {
            try
            {
                Debug.Log($"[Economy] Incrementing currency: {currencyId} by {amount}");
                
                await Unity.Services.Economy.EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(currencyId, amount);
                Debug.Log($"[Economy] Successfully incremented {currencyId} by {amount}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to increment currency: {e.Message}");
                throw;
            }
        }

        public async Task DecrementCurrencyAsync(string currencyId, int amount)
        {
            try
            {
                Debug.Log($"[Economy] Decrementing currency: {currencyId} by {amount}");
                
                await Unity.Services.Economy.EconomyService.Instance.PlayerBalances.DecrementBalanceAsync(currencyId, amount);
                Debug.Log($"[Economy] Successfully decremented {currencyId} by {amount}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to decrement currency: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lists all items in the Economy catalog configured in the Dashboard
        /// Store items: BoardSkin_Classic (free), BoardSkin_Adinkra (real-money), RemoveAds (non-consumable)
        /// </summary>
        public async Task<List<VirtualPurchaseDefinition>> ListCatalogAsync()
        {
            try
            {
                Debug.Log("[Economy] Fetching catalog items...");
                
                // Sync configuration first (replaces GetVirtualPurchasesAsync)
                await Unity.Services.Economy.EconomyService.Instance.Configuration.SyncConfigurationAsync();
                
                // Then get virtual purchases synchronously from cached config
                var virtualPurchases = Unity.Services.Economy.EconomyService.Instance.Configuration.GetVirtualPurchases();
                Debug.Log($"[Economy] Retrieved {virtualPurchases.Count} catalog items");
                
                foreach (var item in virtualPurchases)
                {
                    Debug.Log($"[Economy] Catalog Item: {item.Id}, Name: {item.Name}");
                }
                
                return virtualPurchases;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to list catalog: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Purchases a virtual item using in-game currency
        /// </summary>
        /// <param name="itemId">The ID of the item to purchase (e.g., "BoardSkin_Classic", "BoardSkin_Adinkra", "RemoveAds")</param>
        public async Task<MakeVirtualPurchaseResult> PurchaseVirtualAsync(string itemId)
        {
            try
            {
                Debug.Log($"[Economy] Purchasing virtual item: {itemId}");
                
                var result = await Unity.Services.Economy.EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(itemId);
                Debug.Log($"[Economy] Successfully purchased {itemId}");
                
                // Log what was received
                if (result.Rewards != null)
                {
                    Debug.Log($"[Economy] Rewards received: {result.Rewards.Inventory?.Count() ?? 0} inventory items");
                }
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to purchase virtual item {itemId}: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checks if the player owns a specific item in their inventory
        /// </summary>
        /// <param name="itemId">The ID of the item to check (e.g., "BoardSkin_Classic", "BoardSkin_Adinkra", "RemoveAds")</param>
        /// <returns>True if the player owns the item, false otherwise</returns>
        public async Task<bool> Owns(string itemId)
        {
            try
            {
                Debug.Log($"[Economy] Checking ownership of: {itemId}");
                
                var playerInventory = await Unity.Services.Economy.EconomyService.Instance.PlayerInventory.GetInventoryAsync();
                
                // Check if the item exists in the player's inventory
                var ownsItem = playerInventory.PlayersInventoryItems.Any(item => 
                    item.InventoryItemId == itemId);
                
                Debug.Log($"[Economy] Player {(ownsItem ? "owns" : "does not own")} {itemId}");
                
                return ownsItem;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Failed to check ownership of {itemId}: {e.Message}");
                return false;
            }
        }
    }
}
