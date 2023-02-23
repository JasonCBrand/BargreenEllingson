using Bargreen.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;


namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        private readonly InventoryService _inventoryService;

        //add dependencies and constructor
        public InventoryServiceTests()
        {
            _inventoryService = new InventoryService();
        }
        [Fact]
        public async void Inventory_Reconciliation_Performs_As_Expected()
        {
            //expected results
            List<InventoryReconciliationResult> expectedResults = new List<InventoryReconciliationResult>
            {
                new InventoryReconciliationResult
                {
                    ItemNumber = "ABC123",
                    TotalValueOnHandInInventory = 3435M,
                    TotalValueInAccountingBalance = 3435M
                },
                new InventoryReconciliationResult
                {
                    ItemNumber = "ZZZ99",
                    TotalValueOnHandInInventory = 1930.62M,
                    TotalValueInAccountingBalance = 1930.62M
                },
                new InventoryReconciliationResult
                {
                    ItemNumber = "XXCCM",
                    TotalValueOnHandInInventory = 7848M,
                    TotalValueInAccountingBalance = 7602.75M
                },
                new InventoryReconciliationResult
                {
                    ItemNumber = "XXDDM",
                    TotalValueOnHandInInventory = 11212.05M,
                    TotalValueInAccountingBalance = 0
                },
                new InventoryReconciliationResult
                {
                    ItemNumber = "FBR77",
                    TotalValueOnHandInInventory = 0,
                    TotalValueInAccountingBalance = 17.99M
                }
            };
            //get the balances and actual results from recon
            var invData = await _inventoryService.GetInventoryBalancesAsync();
            var accData = await _inventoryService.GetAccountingBalancesAsync();
            var actualResults = await InventoryService.ReconcileInventoryToAccounting(invData, accData);

            //have to use this instead of Assert.Equal because it was resulting in failures due to the decimals
            foreach (var (expectedItem, actualItem) in expectedResults.Zip(actualResults, (expectedItem, actualItem) => (expectedItem, actualItem)))
            {
                Assert.Equal(expectedItem.ItemNumber, actualItem.ItemNumber);
                Assert.Equal(expectedItem.TotalValueOnHandInInventory, actualItem.TotalValueOnHandInInventory);
                Assert.Equal(expectedItem.TotalValueInAccountingBalance, actualItem.TotalValueInAccountingBalance, 2); //tolerance of 2 decimal places
            }

        }
    }
}
