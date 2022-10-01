using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RTS.Controllers
{
    [Authorize]
    public class ItemRequestsController :Controller
    {
        private readonly ItemsBS _itemsBS;
        private readonly RequestsBS _requestsBS;
        private readonly EmployeesBS _employeesBS;
        private readonly StatusBS _statusBS;
        private readonly TransactionBS _transactionBS;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public ItemRequestsController(ItemsBS itemsBS, RequestsBS requestsBS, 
            EmployeesBS employeesBS,
            StatusBS statusBS,
            TransactionBS transactionBS, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> SignInManager)
        {
            _itemsBS = itemsBS;
            _requestsBS = requestsBS;
            _employeesBS = employeesBS;
            _statusBS = statusBS;
            _transactionBS = transactionBS;
            _userManager = userManager;
            _signInManager = SignInManager;
        }




        private void SendEmail(string emailTo,string message)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("muname1994@hotmail.com");
            
            mailMessage.To.Add(emailTo);
            mailMessage.Subject = "Item Requests System";
      
            mailMessage.Body = message ;
            mailMessage.IsBodyHtml = true;
            string host = mailMessage.From.Host;
            if (host == "hotmail.com" || host == "Outlook.com")
            {
                host = "live.com";
            }
            SmtpClient smtpClient = new SmtpClient("smtp.outlook.com" );
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("muname1994@hotmail.com", "Toshiba/Samsung");
            smtpClient.Send(mailMessage);

        }
        
        public IActionResult RequestReceived()
        {
            var empEmail = _userManager.GetUserName(User);
            Employee employee = _employeesBS.GetEmployeeByEmail(empEmail);
            return View(_transactionBS.GetTransactionToEmployee(employee.Id));
        }
        
        
        public IActionResult RequestSent()
        {
            var empEmail = _userManager.GetUserName(User);
            Employee employee = _employeesBS.GetEmployeeByEmail(empEmail);
            return View(_transactionBS.GetTransactionFromEmployee(employee.Id));
        }
        
        [HttpGet]
        public IActionResult RequestItem(int id)
        {
            var empEmail = _userManager.GetUserName(User);
            Employee employee = _employeesBS.GetEmployeeByEmail(empEmail);
            Item item = _itemsBS.GetItemCatEmp(id);
            List<ItemRequest> itemRequests = _requestsBS.GetList();

            if (item.AssignedToEmployeeID == employee.Id)
            {
                return RedirectToAction("Index", "Items");
            }
            foreach (ItemRequest itemRequest in itemRequests) {
                if (itemRequest.Employee.Id == employee.Id && itemRequest.Item.Id == id) {
                    return RedirectToAction("Index", "Items");
                }
            }
            return View();
        }

        

        [HttpPost]
        public IActionResult RequestItem(RequestModel model,int id)
        {
            ItemRequest itemRequest = new ItemRequest();

            var empEmail = _userManager.GetUserName(User);
            Employee employee = _employeesBS.GetEmployeeByEmail(empEmail);

            itemRequest.ItemID = id;
            itemRequest.RequestEmployeeID = employee.Id;
            itemRequest.StatusID = 2;
            
            _requestsBS.CreateRequest(itemRequest);
            Transaction transaction = new Transaction();
            transaction.RequestID = itemRequest.Id;
            transaction.RequestToEmployeeID = _itemsBS.GetItemEmp(id).Employee.Id;
            transaction.changedStatus = 2;
            transaction.changedAssigndToEmpID = transaction.RequestToEmployeeID;
            transaction.TransactionDate = DateTime.Now;
            _transactionBS.CreateTransaction(transaction);


            string email = _itemsBS.GetItemEmp(id).Employee.Email;

            var link = Url.Action(nameof(RequestReceived), "ItemRequests", new { itemRequest.Id }
            , Request.Scheme, Request.Host.ToString());
            var message = model.message + $"<a href=\"{link}\">Confirm Item Request</a>";
            SendEmail(email,message);           
            

             return RedirectToAction(nameof(RequestSent));          

        }

        [HttpGet]
        public IActionResult Approved(int id)
        {          
            ItemRequest itemRequest = _requestsBS.GetItemRequest(id);
            itemRequest.StatusID = 1;
            Item item = _itemsBS.GetItemCatEmp(itemRequest.ItemID);
            item.AssignedToEmployeeID = itemRequest.RequestEmployeeID;
            _itemsBS.EditItem(item);
            _requestsBS.EditItemRequest(itemRequest);

            Transaction transaction1 = new Transaction();
            transaction1.RequestID = itemRequest.Id;
            var empEmail = _userManager.GetUserName(User);
            transaction1.RequestToEmployeeID = _employeesBS.GetEmployeeByEmail(empEmail).Id;
            transaction1.changedStatus = itemRequest.StatusID;
            transaction1.changedAssigndToEmpID = itemRequest.Employee.Id;
            transaction1.TransactionDate = DateTime.Now;
            _transactionBS.CreateTransaction(transaction1);

            string email = itemRequest.Employee.Email;
            string message = "Your Request is Approved";
            SendEmail(email, message);

            return RedirectToAction(nameof(RequestReceived));
        }
                
        [HttpGet]
        public IActionResult Rejected()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Rejected(RejectedNote model,int id)
        {
            ItemRequest itemRequest = _requestsBS.GetItemRequest(id);
            itemRequest.StatusID = 3;
            _requestsBS.EditItemRequest(itemRequest);
            Transaction transaction1 = new Transaction();
            transaction1.RequestID = itemRequest.Id;
            var empEmail = _userManager.GetUserName(User);
            transaction1.RequestToEmployeeID = _employeesBS.GetEmployeeByEmail(empEmail).Id;
            transaction1.changedStatus = itemRequest.StatusID;
            transaction1.changedAssigndToEmpID = _itemsBS.GetItemCatEmp(itemRequest.ItemID).AssignedToEmployeeID;
            transaction1.TransactionDate = DateTime.Now;
            _transactionBS.CreateTransaction(transaction1);


            string email = itemRequest.Employee.Email;
            string message = "Your Request is Rejected"+ "note:"+model.note;
            SendEmail(email, message);

            return RedirectToAction(nameof(RequestReceived));
        }

    }
}
