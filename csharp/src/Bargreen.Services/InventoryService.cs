using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }
    }

    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
        //add property to make comparison easier
        public decimal TotalValueOnHand { get; set; }
    }

    public class AccountingBalance
    {
        public string ItemNumber { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }


    public class InventoryService
    {
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync()
        {
            //change list to variable and move return to end of method
            var inventoryBalances = new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 312,
                     WarehouseLocation = "WLA1",
                     TotalValueOnHand = 2340M
                },
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 146,
                     WarehouseLocation = "WLA2",
                     TotalValueOnHand = 1095M

                },
                new InventoryBalance()
                {
                     ItemNumber = "ZZZ99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 47,
                     WarehouseLocation = "WLA3",
                     TotalValueOnHand = 657.53M
                },
                new InventoryBalance()
                {
                     ItemNumber = "zzz99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 91,
                     WarehouseLocation = "WLA4",
                     TotalValueOnHand = 1273.09M
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxccM",
                     PricePerItem = 245.25M,
                     QuantityOnHand = 32,
                     WarehouseLocation = "WLA5",
                     TotalValueOnHand = 7848M
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxddM",
                     PricePerItem = 747.47M,
                     QuantityOnHand = 15,
                     WarehouseLocation = "WLA6",
                     TotalValueOnHand = 11212.05M
                }
            };
            //change return type to task of our above created list
            return await Task.FromResult<IEnumerable<InventoryBalance>>(inventoryBalances);
        }

        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync()
        {
            //change list to variable and move return to end of method
            var accountingBalances = new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3435M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbr77",
                     TotalInventoryValue = 17.99M
                }
            };
            //change return type to task of our above created list
            return await Task.FromResult<IEnumerable<AccountingBalance>>(accountingBalances);
        }

        public static async Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            //TODO-CHALLENGE: Compare inventory balances to accounting balances and find differences

            //create dictionaries to group the lists by item balances making sure to use ToUpper on the item numbers so case doesnt matter
            var inventoryDict = inventoryBalances.GroupBy(i => i.ItemNumber.ToUpper(),
                (k, g) => new { ItemNumber = k, Balances = g.ToList() }).ToDictionary(x => x.ItemNumber, x => x.Balances);
            var accountingDict = accountingBalances.ToDictionary(x => x.ItemNumber.ToUpper(), x => x.TotalInventoryValue);
            var result = new List<InventoryReconciliationResult>();
            //loop through the combined results
            foreach (var itemNumber in inventoryDict.Keys.Union(accountingDict.Keys))
            {
                //if the item exists in inventory list
                if (inventoryDict.TryGetValue(itemNumber, out var inventoryBalancesForItemNumber))
                {
                    var totalValueOnHandInInventory = inventoryBalancesForItemNumber.Sum(x => x.TotalValueOnHand);
                    //if the item exists in accounting list
                    if (accountingDict.TryGetValue(itemNumber, out var totalValueInAccountingBalance))
                    {
                        result.Add(new InventoryReconciliationResult
                        {
                            ItemNumber = itemNumber,
                            TotalValueOnHandInInventory = totalValueOnHandInInventory,
                            TotalValueInAccountingBalance = totalValueInAccountingBalance
                        });
                    }
                    //else it doesnt exist so accounting val is 0
                    else
                    {
                        result.Add(new InventoryReconciliationResult
                        {
                            ItemNumber = itemNumber,
                            TotalValueOnHandInInventory = totalValueOnHandInInventory,
                            TotalValueInAccountingBalance = 0M
                        });
                    }
                }
                //else it only exists in accounting list so inventory val is 0
                else if (accountingDict.TryGetValue(itemNumber, out var totalValueInAccountingBalance))
                {
                    result.Add(new InventoryReconciliationResult
                    {
                        ItemNumber = itemNumber,
                        TotalValueOnHandInInventory = 0M,
                        TotalValueInAccountingBalance = totalValueInAccountingBalance
                    });
                }
            }
            return await Task.FromResult<IEnumerable<InventoryReconciliationResult>>(result);

        }
    }
}