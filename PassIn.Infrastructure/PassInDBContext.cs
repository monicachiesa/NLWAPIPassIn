using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Infrastructure
{
    public class PassInDBContext : DbContext
    {
        //tabela eventos no banco de dados
        public DbSet<Event> Events { get; set; }
        //função que configura o contexto com a base de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\monic\\OneDrive\\Documentos\\PassInDb.db");
        }
    }
}
