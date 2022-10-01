using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Controllers
{
    public class TransactionsController:Controller
    {
        private readonly TransactionBS _transactionBS;
        private readonly ItemsBS _itemsBS;
        public TransactionsController(TransactionBS transactionBS,ItemsBS itemsBS)
        {
            _transactionBS = transactionBS;
            _itemsBS = itemsBS;
        }

        /*[HttpGet]
        public IActionResult Index(DateTime startDate, DateTime endDate)
        {
            return View(_transactionBS.GetPeriodList(startDate, endDate));
        }*/



        [HttpGet]
        public IActionResult Index(DateTime startDate, DateTime endDate, 
            List<SelectListItem> ItemsSearchList,string selectedItem)
        {
            TransactionModel model = new TransactionModel();
            foreach (Item item in _itemsBS.GetItems())
            {
                SelectListItem item1 = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Name,                    
                };
                ItemsSearchList.Add(item1);
            }


            var mod = _transactionBS.GetAllTransactions();
            
            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue)
            {
                mod=_transactionBS.GetPeriodList(startDate, endDate);
            }
            if (!String.IsNullOrEmpty(selectedItem))
            {                
                mod = _transactionBS.GetItemsSelectedList(selectedItem);
            }
            model.ItemsList = ItemsSearchList;
            model.TransactionsList = mod;
            return View(model);
        }       
    }
}
