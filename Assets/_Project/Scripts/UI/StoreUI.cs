using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.Services;
using Unity.Services.Economy.Model;

namespace Project.UI
{
    /// <summary>
    /// Store UI stub for demonstrating EconomyService functionality.
    /// Displays catalog items and handles purchases.
    /// Store items configured in Dashboard:
    /// - BoardSkin_Classic (free)
    /// - BoardSkin_Adinkra (real-money)
    /// - RemoveAds (non-consumable)
    /// </summary>
    public class StoreUI : MonoBehaviour
    {
        [Header("Service References")]
        [SerializeField] private UgsInitializer ugsInitializer;

        [Header("UI Elements (Optional - for visual feedback)")]
        [SerializeField] private Text statusText;
        [SerializeField] private Text catalogText;

        private EconomyService economyService;

        private void Start()
        {
            // Get service references from UgsInitializer if available
            if (ugsInitializer != null)
            {
                // Get already-initialized services from UgsInitializer
                economyService = ugsInitializer.GetEconomyService();
            }
            else
            {
                Debug.LogWarning("[StoreUI] UgsInitializer not assigned. Services may not be initialized.");
            }

            UpdateStatus("Store UI Ready");
        }

        #region Store Methods

        /// <summary>
        /// Call this from a UI button to list all catalog items.
        /// </summary>
        public async void OnListCatalogButton()
        {
            Debug.Log("[StoreUI] List Catalog button clicked");
            UpdateStatus("Loading catalog...");

            try
            {
                if (economyService == null)
                {
                    UpdateStatus("Economy service not available. Assign UgsInitializer.");
                    return;
                }

                var catalog = await economyService.ListCatalogAsync();
                
                if (catalog.Count == 0)
                {
                    UpdateStatus("No items in catalog");
                    UpdateCatalog("No items found");
                    return;
                }

                UpdateStatus($"Loaded {catalog.Count} items");
                
                // Display catalog items
                string catalogDisplay = "=== CATALOG ===\n";
                foreach (var item in catalog)
                {
                    catalogDisplay += $"\n{item.Name} ({item.Id})";
                    
                    // Display costs if available
                    if (item.Costs != null && item.Costs.Count > 0)
                    {
                        catalogDisplay += "\n  Costs:";
                        foreach (var cost in item.Costs)
                        {
                            catalogDisplay += $"\n    - {cost.Amount} {cost.Item.GetReferencedConfigurationItem().Id}";
                        }
                    }
                    
                    catalogDisplay += "\n";
                }
                
                UpdateCatalog(catalogDisplay);
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to load catalog: {e.Message}");
                Debug.LogError($"[StoreUI] Failed to load catalog: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to purchase BoardSkin_Classic (free skin).
        /// </summary>
        public async void OnPurchaseClassicSkinButton()
        {
            await PurchaseItem("BoardSkin_Classic");
        }

        /// <summary>
        /// Call this from a UI button to purchase BoardSkin_Adinkra.
        /// </summary>
        public async void OnPurchaseAdinkraSkinButton()
        {
            await PurchaseItem("BoardSkin_Adinkra");
        }

        /// <summary>
        /// Call this from a UI button to purchase RemoveAds.
        /// </summary>
        public async void OnPurchaseRemoveAdsButton()
        {
            await PurchaseItem("RemoveAds");
        }

        /// <summary>
        /// Helper method to purchase an item by ID.
        /// </summary>
        private async System.Threading.Tasks.Task PurchaseItem(string itemId)
        {
            Debug.Log($"[StoreUI] Purchasing {itemId}");
            UpdateStatus($"Purchasing {itemId}...");

            try
            {
                if (economyService == null)
                {
                    UpdateStatus("Economy service not available. Assign UgsInitializer.");
                    return;
                }

                var result = await economyService.PurchaseVirtualAsync(itemId);
                UpdateStatus($"Successfully purchased {itemId}!");
                
                // Display what was received
                if (result.Rewards != null && result.Rewards.Inventory != null)
                {
                    string rewards = $"Received {result.Rewards.Inventory.Count} item(s)";
                    Debug.Log($"[StoreUI] {rewards}");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Purchase failed: {e.Message}");
                Debug.LogError($"[StoreUI] Purchase failed: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to check ownership of BoardSkin_Classic.
        /// </summary>
        public async void OnCheckClassicSkinOwnershipButton()
        {
            await CheckOwnership("BoardSkin_Classic");
        }

        /// <summary>
        /// Call this from a UI button to check ownership of BoardSkin_Adinkra.
        /// </summary>
        public async void OnCheckAdinkraSkinOwnershipButton()
        {
            await CheckOwnership("BoardSkin_Adinkra");
        }

        /// <summary>
        /// Call this from a UI button to check ownership of RemoveAds.
        /// </summary>
        public async void OnCheckRemoveAdsOwnershipButton()
        {
            await CheckOwnership("RemoveAds");
        }

        /// <summary>
        /// Helper method to check ownership of an item by ID.
        /// </summary>
        private async System.Threading.Tasks.Task CheckOwnership(string itemId)
        {
            Debug.Log($"[StoreUI] Checking ownership of {itemId}");
            UpdateStatus($"Checking ownership of {itemId}...");

            try
            {
                if (economyService == null)
                {
                    UpdateStatus("Economy service not available. Assign UgsInitializer.");
                    return;
                }

                bool owns = await economyService.Owns(itemId);
                UpdateStatus(owns 
                    ? $"You own {itemId}" 
                    : $"You do not own {itemId}");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Ownership check failed: {e.Message}");
                Debug.LogError($"[StoreUI] Ownership check failed: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to display player's inventory.
        /// </summary>
        public async void OnShowInventoryButton()
        {
            Debug.Log("[StoreUI] Show Inventory button clicked");
            UpdateStatus("Loading inventory...");

            try
            {
                if (economyService == null)
                {
                    UpdateStatus("Economy service not available. Assign UgsInitializer.");
                    return;
                }

                var inventoryItems = await economyService.GetInventoryItemsAsync();
                
                if (inventoryItems.Count == 0)
                {
                    UpdateStatus("Inventory is empty");
                    UpdateCatalog("No items in inventory");
                    return;
                }

                UpdateStatus($"Loaded {inventoryItems.Count} inventory items");
                
                // Display inventory items
                string inventoryDisplay = "=== YOUR INVENTORY ===\n";
                foreach (var item in inventoryItems)
                {
                    inventoryDisplay += $"\n{item.Name} ({item.Id})";
                }
                
                UpdateCatalog(inventoryDisplay);
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to load inventory: {e.Message}");
                Debug.LogError($"[StoreUI] Failed to load inventory: {e}");
            }
        }

        /// <summary>
        /// Call this from a UI button to display player's balances.
        /// </summary>
        public async void OnShowBalancesButton()
        {
            Debug.Log("[StoreUI] Show Balances button clicked");
            UpdateStatus("Loading balances...");

            try
            {
                if (economyService == null)
                {
                    UpdateStatus("Economy service not available. Assign UgsInitializer.");
                    return;
                }

                var balances = await economyService.GetPlayerBalancesAsync();
                
                if (balances.Count == 0)
                {
                    UpdateStatus("No balances found");
                    UpdateCatalog("No currency balances");
                    return;
                }

                UpdateStatus($"Loaded {balances.Count} balances");
                
                // Display balances
                string balancesDisplay = "=== YOUR BALANCES ===\n";
                foreach (var balance in balances)
                {
                    balancesDisplay += $"\n{balance.CurrencyId}: {balance.Balance}";
                }
                
                UpdateCatalog(balancesDisplay);
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Failed to load balances: {e.Message}");
                Debug.LogError($"[StoreUI] Failed to load balances: {e}");
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[StoreUI] {message}");
        }

        private void UpdateCatalog(string catalogInfo)
        {
            if (catalogText != null)
            {
                catalogText.text = catalogInfo;
            }
            Debug.Log($"[StoreUI] Catalog:\n{catalogInfo}");
        }

        #endregion
    }
}
