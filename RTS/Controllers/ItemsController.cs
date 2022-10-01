using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;


namespace RTS.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ItemsBS _itemsBS;
        private readonly RequestsBS _requestsBS;
        private readonly EmployeesBS _employeesBS;
        public ItemsController(ItemsBS itemsBS, RequestsBS requestsBS,EmployeesBS employeesBS)
        {
            _itemsBS = itemsBS;
            _requestsBS = requestsBS;
            _employeesBS = employeesBS;
        }


        /*public IActionResult Index()
        {           
            return View(_itemsBS.GetAllItems());
        }*/
        
        
        [HttpGet]
        public IActionResult Index(string ItemSearch)
        {           
            ViewData["ListWithSearch"]= ItemSearch;
            var query = _itemsBS.GetWithSearch(ItemSearch);
            return View (query);
        }


        [HttpGet]
        public IActionResult Create()
        {                                
            CreateItemModel model = new CreateItemModel();
            List<SelectListItem> CategoriesList = new List<SelectListItem>();
            List<SelectListItem> EmployeesList = new List<SelectListItem>();
            foreach (ItemCategory itemCategory in _itemsBS.GetCategories())
            {
                SelectListItem category = new SelectListItem
                {
                    Text = itemCategory.Type,
                    Value = itemCategory.Id.ToString(),                    
                };
                CategoriesList.Add(category);
            }
            model.SelectCategoriesList = CategoriesList;

            //employees
            foreach (Employee employee in _itemsBS.GetEmployees())
            {
                SelectListItem employees = new SelectListItem
                {
                    Text = employee.EmployeeName,
                    Value = employee.Id.ToString(),
                };
                EmployeesList.Add(employees);
            }
            model.SelectEmployeesList = EmployeesList;
            return View(model);
            
        }


        [HttpPost]
        public ActionResult Create(CreateItemModel mod)
        { 
            Item item = new Item();            
            item.Name = mod.Name;
            item.Model = mod.Model;
            item.PurchaseDate = mod.PurchaseDate;
            item.Manufacturer = mod.Manufacturer;
            item.SerialNumber = mod.SerialNumber;
            item.Description = mod.Description;
            item.CategoryId =Convert.ToInt32(mod.selectedCategory);           
            item.AssignedToEmployeeID = Convert.ToInt32(mod.selectedEmployee);
            
            _itemsBS.CreateItem(item);
            
            return RedirectToAction(nameof(Index));
        }
        
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            CreateItemModel model = new CreateItemModel();
            List<SelectListItem> CategoriesList = new List<SelectListItem>();
            List<SelectListItem> EmployeesList = new List<SelectListItem>();
            foreach (ItemCategory itemCategory in _itemsBS.GetCategories())
            {
                SelectListItem category = new SelectListItem
                {
                    Text = itemCategory.Type,
                    Value = itemCategory.Id.ToString(),
                };
                CategoriesList.Add(category);
            }
            model.SelectCategoriesList = CategoriesList;
            var item = _itemsBS.GetItemCatEmp(id);
            model.selectedCategory = item.CategoryId.ToString();


            //employees
            foreach (Employee employee in _itemsBS.GetEmployees())
            {
                SelectListItem employees = new SelectListItem
                {
                    Text = employee.EmployeeName,
                    Value = employee.Id.ToString(),
                };
                EmployeesList.Add(employees);
            }
            model.SelectEmployeesList = EmployeesList;
            model.selectedEmployee = item.AssignedToEmployeeID.ToString();
            model.Name = item.Name;
            model.Manufacturer = item.Manufacturer;
            model.Model = model.Model;
            model.PurchaseDate = model.PurchaseDate;
            model.SerialNumber = item.SerialNumber;
            model.Description = item.Description;
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(CreateItemModel mod,int id)
        {
            Item item = _itemsBS.GetItemCatEmp(id);
            item.Name = mod.Name;
            item.Model = mod.Model;
            item.PurchaseDate = mod.PurchaseDate;
            item.Manufacturer = mod.Manufacturer;
            item.SerialNumber = mod.SerialNumber;
            item.Description = mod.Description;
            item.CategoryId = Convert.ToInt32(mod.selectedCategory);
            item.AssignedToEmployeeID = Convert.ToInt32(mod.selectedEmployee);

            _itemsBS.EditItem(item);

            return RedirectToAction(nameof(Index));
        }




        /*[HttpPost]
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                item.ItemCategory.Type = "vvvvv";
                _itemsBS.CreateItem(item);
                return RedirectToAction(nameof(Index));
            }                        
            return View(item);
        }*/

        /*[HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == 0)
        {
            return View();
        }

        //var item = _itemsBS.GetItem(id);
        var item = _itemsBS.GetItemCatEmp(id);
        if (item == null)
        {
            return View();
        }
        return View(item);
    }
    [HttpPost]
    public IActionResult Edit(Item item)
    {
        _itemsBS.EditItem(item);
        return RedirectToAction(nameof(Index));
    }*/

        /*[HttpPost]
        public IActionResult Edit(int id,Item item)
        {
            if (id != item.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _itemsBS.EditItem(item);
                }
                catch (Exception)
                {
                    if (_itemsBS.ItemExists(item.Id))
                    {
                        throw;
                    }
                    else
                    {
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }*/


        /*public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return View();
            }

            Item item = _itemsBS.GetItem(id);
            if (item == null)
            {
                return View();
            }            
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _itemsBS.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }*/

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return View();
            }

            Item item = _itemsBS.GetItem(id);
            if (item == null)
            {
                return View();
            }
            item.isDeleted = true;
            _itemsBS.SoftDeleteItem(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
