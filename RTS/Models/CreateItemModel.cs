using Microsoft.AspNetCore.Mvc.Rendering;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class CreateItemModel :Item
    {       
        public List<SelectListItem> SelectCategoriesList { get; set; }
        public List<SelectListItem> SelectEmployeesList { get; set; }

        public string selectedCategory { get; set; }
        public string selectedEmployee { get; set; }
    }
}
