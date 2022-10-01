using Microsoft.AspNetCore.Mvc;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Controllers
{
    public class ItemCategoriesController : Controller
    {
        private readonly ItemCategoriesBS _itemCategoriesBS;
        public ItemCategoriesController(ItemCategoriesBS itemCategoriesBS)
        {
            _itemCategoriesBS = itemCategoriesBS;
        }
        public IActionResult Index()
        {
            return View(_itemCategoriesBS.GetAllItemCategries());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Id,Type")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                _itemCategoriesBS.CreateItemCategory(itemCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);

        }
        
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return View();
            }

            var itemCategory = _itemCategoriesBS.GetItemCategory(id);
            if (itemCategory == null)
            {
                return View();
            }
            return View(itemCategory);
        }

        
        [HttpPost]        
        public IActionResult Edit(int id, [Bind("Id,Type")] ItemCategory itemCategory)
        {
            if (id != itemCategory.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _itemCategoriesBS.EditItemCategory(itemCategory);
                }
                catch (Exception)
                {
                    if (!_itemCategoriesBS.ItemCategoryExists(itemCategory.Id))
                    {
                        return View();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return View();
            }

            ItemCategory itemCategory = _itemCategoriesBS.GetItemCategory(id);
            if (itemCategory == null)
            {
                return View();
            }

            return View(itemCategory);
        }

        
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _itemCategoriesBS.DeleteItemCategory(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
