using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using NAS.Model;

namespace NAS.DAL
{

    public class DBContext : DbContext
    {
        public DBContext() : base("name=DB") //DefaultConnection
        {
            Database.CreateIfNotExists();
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Admin> AdminTable { get; set; }
        public DbSet<Token> TokenTable { get; set; }

    }
}