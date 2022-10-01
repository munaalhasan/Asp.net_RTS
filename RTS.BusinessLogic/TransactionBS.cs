using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS.BusinessLogic
{
    public class TransactionBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public TransactionBS(RTSDbContext context)
        {
            _context = context;
        }

        /*public List<Transaction> GetPeriodList(DateTime startDate, DateTime endDate)
        {
            if (startDate!=DateTime.MinValue || endDate!=DateTime.MinValue)
            {
                var searchList= _context.Transactions.Include(i => i.ItemRequest)
                .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
                .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
                .Include(w => w.Status).Include(z => z.Item)
                .Where(a => a.TransactionDate >= startDate && a.TransactionDate <= endDate)
                .ToList();
                return searchList;
            }
            else
            {
                return GetAllTransactions();
            }
        }*/




        public List<Transaction> GetPeriodList(DateTime startDate, DateTime endDate)
        {
            return _context.Transactions.Include(i => i.ItemRequest)
           .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
           .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
           .Include(w => w.Status).Include(z => z.Item)
           .Where(a => a.TransactionDate >= startDate && a.TransactionDate <= endDate)
           .ToList();
        }

        public List<Transaction> GetItemsSelectedList(string selectedItem)
        {
            return _context.Transactions.Include(i => i.ItemRequest)
           .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
           .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
           .Include(w => w.Status).Include(z => z.Item)
           .Where(a => a.ItemRequest.Item.Name==selectedItem)
           .ToList();
        }



        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions.Include(i => i.ItemRequest)
                .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
                .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
                .Include(w=>w.Status).Include(z=>z.Item).ToList();
        }
        public List<ItemRequest> GetTransactionToEmployee(int id)
        {
            return _context.Transactions.Include(i => i.ItemRequest)
                .Include(x=>x.ItemRequest.Item).Include(s=>s.ItemRequest.Status)
                .Include(e=>e.ItemRequest.Employee).Include(e => e.Employee)                
                .Where(c=>c.RequestToEmployeeID==id && c.ItemRequest.Status.Id == 2)
                .Select(z => z.ItemRequest).Distinct().ToList();
        }
        
        public List<Transaction> GetTransactionFromEmployee(int id)
        {
            return _context.Transactions.Include(i => i.ItemRequest)
                .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
                .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
                .Include(w => w.Status).Include(z => z.Item)
                .Where(c => c.ItemRequest.Employee.Id == id && c.ItemRequest.Status.Id == 2)
                .ToList();
                
        }

        /*public int GetTransactionToEmployee(int id)
        {
            return _context.Transactions.Include(i => i.ItemRequest)
                .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status)
                .Include(e => e.ItemRequest.Employee).Include(e => e.Employee)
                .Where(c => c.ItemRequest.Id == id)
                .Select(Convert.ToInt32(v=>v.RequestToEmployeeID));
        }*/
        public Transaction GetTransReq(int reqID)
        {
            _context.Transactions.Include(e => e.ItemRequest).SingleOrDefault(x => x.ItemRequest.Id == reqID);
            return _context.Transactions.Include(c => c.Employee).SingleOrDefault(x => x.ItemRequest.Id == reqID);
        }

        public void CreateTransaction(Transaction transaction)
        {
            //.Include(e => e.ItemRequest.Employee.Id)
            //    .Include(x => x.ItemRequest.Item.Id).Include(s => s.ItemRequest.Status.Id)
            _context.Transactions.Include(i => i.ItemRequest.Id)
                .Include(c => c.Employee.Id);
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public Transaction GetData(int id)
        {
            _context.Transactions.Include(e => e.Employee).SingleOrDefault(x => x.Id == id);
            return _context.Transactions.Include(c => c.ItemRequest)
                .Include(x => x.ItemRequest.Item).Include(s => s.ItemRequest.Status).SingleOrDefault(x => x.Id == id);
        }

        
    }
}
