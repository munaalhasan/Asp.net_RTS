using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS.BusinessLogic
{
    public class EmployeesBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public EmployeesBS(RTSDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }
        public Employee GetEmployee(int id)
        {
            Employee employee = _context.Employees.Find(id);
            return employee;
        }
        public Employee GetEmployeeByEmail(string email)
        {
            Employee employee = _context.Employees.SingleOrDefault(e => e.Email == email);
            return employee;
        }
        public void CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }
        public void EditEmployee(Employee employee)
        {
            _context.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEmployee(int id)
        {
            Employee employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
        public bool EmployeeExists(int id)
        {
            return _context.Employees.Count(e => e.Id == id) > 0;
        }
    }
}
