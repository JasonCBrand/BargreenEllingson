using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bargreen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bargreen.API.Controllers
{
    //TODO-CHALLENGE: Make the methods in this controller follow the async/await pattern
    //TODO-CHALLENGE: Use dotnet core dependency injection to inject the InventoryService
    
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        //add scoped dependency
        private readonly InventoryService _inventoryService;

        //constructor
        public InventoryController()
        {
            _inventoryService = new InventoryService();
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            //converted method to async Task and updated method name to return async method with await keyword
            //var _inventoryService = new InventoryService();
            return await _inventoryService.GetInventoryBalancesAsync();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            //converted method to async Task and updated method name to return async method with await keyword
            //var inventoryService = new InventoryService();
            return await _inventoryService.GetAccountingBalancesAsync();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            //var inventoryService = new InventoryService();
            //add await and cast to explicit type
            return await InventoryService.ReconcileInventoryToAccounting((IEnumerable<InventoryBalance>)_inventoryService.GetInventoryBalancesAsync(), (IEnumerable<AccountingBalance>)_inventoryService.GetAccountingBalancesAsync());
        }
    }
}