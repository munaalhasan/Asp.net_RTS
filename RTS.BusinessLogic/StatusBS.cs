using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace RTS.BusinessLogic
{
    public class StatusBS
    {
        private RTSDbContext _context = new RTSDbContext();
        public StatusBS(RTSDbContext context)
        {
            _context = context;
        }
        public List<Status> GetAllStatusTypes()
        {
            return _context.RequestStatus.ToList();
        }

    }
}
