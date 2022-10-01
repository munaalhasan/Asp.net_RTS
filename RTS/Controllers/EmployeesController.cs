using Microsoft.AspNetCore.Mvc;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTS.Controllers
{
    public class EmployeesController :Controller
    {
        private readonly EmployeesBS _employeesBS;
        public EmployeesController(EmployeesBS employeesBS)
        {
            _employeesBS = employeesBS;
        }
        public IActionResult Index()
        {
            return View(_employeesBS.GetAllEmployees());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeesBS.CreateEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return View();
            }

            var employee = _employeesBS.GetEmployee(id);
            if (employee == null)
            {
                return View();
            }
            return View(employee);
        }


        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeesBS.EditEmployee(employee);
                }
                catch (Exception)
                {
                    if (!_employeesBS.EmployeeExists(employee.Id))
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
            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return View();
            }

            Employee employee= _employeesBS.GetEmployee(id);
            if (employee == null)
            {
                return View();
            }

            return View(employee);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeesBS.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
