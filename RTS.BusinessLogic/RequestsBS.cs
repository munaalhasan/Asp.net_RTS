using Microsoft.EntityFrameworkCore;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS.BusinessLogic
{
    public class RequestsBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public RequestsBS(RTSDbContext context)
        {
            _context = context;
        }

        public List<ItemRequest> GetAllItemsEmployeeStatus(int employeeId)
        {
            return _context.ItemRequests.Include(e => e.Employee).Include(i => i.Item)
                .Include(s => s.Status).Where(e => e.Employee.Id==employeeId).ToList();                
        }
        public List<ItemRequest> GetList()
        {
            return _context.ItemRequests.Include(e => e.Employee).Include(i => i.Item)
                .Include(s => s.Status).ToList();
        }
        public ItemRequest GetItemRequest(int id)
        {
            _context.ItemRequests.Include(i=>i.Item).SingleOrDefault(x => x.Id == id);
            _context.ItemRequests.Include(e => e.Employee).SingleOrDefault(x => x.Id == id);
            return _context.ItemRequests.Include(c => c.Status).SingleOrDefault(x => x.Id == id);
        }
        
        public void CreateRequest(ItemRequest request)
        {
            _context.ItemRequests.Include(c => c.Employee.Id).Include(e => e.Item.Id).Include(i=>i.Status.Id);
            _context.ItemRequests.Add(request);
            _context.SaveChanges();
        }
        public void EditItemRequest(ItemRequest itemRequest)
        {
            _context.ItemRequests.Include(i => i.Item.Id).Include(e => e.Employee.Id)
                .Include(s => s.Status.Id);
            _context.Entry(itemRequest).State = EntityState.Modified;
            _context.SaveChanges();

        }
    }
}
