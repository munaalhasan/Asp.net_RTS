using Microsoft.AspNetCore.Mvc.Rendering;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class TransactionModel 
    {
        public List<Transaction> TransactionsList { get; set; }
        public List<SelectListItem> ItemsList { get; set; }
        public string selectedItem { get; set; }
    }
}
