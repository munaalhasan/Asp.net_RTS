using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RTS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTS.DataAccess.AppDB
{
    //
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext()
        {
                
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
    }
}
