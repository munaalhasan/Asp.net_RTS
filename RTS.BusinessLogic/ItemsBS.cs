using RTS.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;

namespace RTS.BusinessLogic
{
    public class ItemsBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public ItemsBS(RTSDbContext context)
        {
            _context = context;
        }

        public List<Item> GetItems()
        {
            return _context.Items.ToList();
        }

        public List<Item> GetAllItems()
        {
            return _context.Items.Include(i => i.ItemCategory).Include(i => i.Employee).Where(i => i.isDeleted == false).ToList();
        }
        
        //public Item GetSearch(string ItemSearch)
        //{
        //    return _context.Items.Include(i => i.ItemCategory).Include(i => i.Employee)
        //        .Where(x => x.Name.Contains(ItemSearch) || x.ItemCategory.Type.Contains(ItemSearch));
        //}
        
        /*public string GetSearch(string ItemSearch)
        {
            return _context.Items.Find(x => x.Name.Contains(ItemSearch));
        }*/
        
        public List<Item> GetWithSearch(string ItemSearch)
        {
            //var query = from item in _context.Items select item;
            //var query = _itemsBS.GetAllItems();
            if (!string.IsNullOrEmpty(ItemSearch))
            {
                return _context.Items.Include(i => i.ItemCategory).Include(i => i.Employee)
                    .Where(i => i.isDeleted == false)
                    .Where(x => x.Name.Contains(ItemSearch) || x.ItemCategory.Type.Contains(ItemSearch))
                    .ToList();
            }
            else
            {
                return _context.Items.Include(i => i.ItemCategory).Include(i => i.Employee).Where(i => i.isDeleted == false).ToList();
            }
        }
        
        
        public List<ItemCategory> GetCategories()
        {
            return _context.ItemCategories.ToList();
        }
        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }
        

        public Item GetItemCatEmp(int id)
        {
            _context.Items.Include(e => e.Employee).SingleOrDefault(x => x.Id == id);
            return _context.Items.Include(c => c.ItemCategory).SingleOrDefault(x => x.Id == id);
        }
        public Item GetItemEmp(int id)
        {
            return _context.Items.Include(e => e.Employee).SingleOrDefault(x => x.Id == id);            
        }

        public Item GetItem(int id)
        {
            Item item = _context.Items.Find(id);
            return item;
        }
        
        public void CreateItem(Item item)
        {
            _context.Items.Include(c => c.ItemCategory.Id).Include(e=>e.Employee.Id);
            _context.Items.Add(item);
            _context.SaveChanges();
        }
        public void EditItem(Item item)
        {
            _context.Items.Include(c => c.ItemCategory.Id).Include(e => e.Employee.Id);
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();           

        }

        public void DeleteItem(int id)
        {
            Item item = _context.Items.Find(id);
            _context.Items.Remove(item);
            _context.SaveChanges();
        }
        public void SoftDeleteItem(int id)
        {
            Item item = _context.Items.Find(id);
            _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
        public bool ItemExists(int id)
        {
            return _context.Items.Count(e => e.Id == id) > 0;
        }
    }
}
