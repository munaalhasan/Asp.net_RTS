using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RTS.BusinessLogic
{
    public class ItemCategoriesBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public ItemCategoriesBS(RTSDbContext context)
        {
            _context = context;
        }

        public List<ItemCategory> GetAllItemCategries()
        {
            return _context.ItemCategories.ToList();
        }
        public ItemCategory GetItemCategory(int id)
        {
            ItemCategory itemCategory = _context.ItemCategories.Find(id);
            return itemCategory;
        }

        public void CreateItemCategory(ItemCategory itemCategory)
        {
            _context.ItemCategories.Add(itemCategory);
            _context.SaveChanges();
        }
        public void EditItemCategory(ItemCategory itemCategory)
        {
            _context.Entry(itemCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteItemCategory(int id)
        {
            ItemCategory itemCategory = _context.ItemCategories.Find(id);
            _context.ItemCategories.Remove(itemCategory);
            _context.SaveChanges();
        }        
        public bool ItemCategoryExists(int id)
        {
            return _context.ItemCategories.Count(e => e.Id == id) > 0;
        }
    }
}
