using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteMailKit.Entities;

namespace TesteMailKit.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base(@"Server=CTS1B315442\SQLEXPRESS;Database=Importacao;Trusted_Connection=True;")
        {
            //Database.SetInitializer(new DbInitializer());
        }



        public DbSet<RegistroEntity> Registros { get; set; }
    }
}
