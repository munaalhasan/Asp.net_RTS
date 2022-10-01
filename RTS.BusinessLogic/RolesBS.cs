using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTS.BusinessLogic
{
    class RolesBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public RolesBS(RTSDbContext context)
        {
            _context = context;
        }



    }
}
