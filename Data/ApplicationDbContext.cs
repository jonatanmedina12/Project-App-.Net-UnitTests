using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace Data
{

        /// <summary>
        ///  se crea el contexto de la base de datos y se genera los indicies para cada una 
        /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {



        }
        public DbSet<Paciente> pacientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

      
            modelBuilder.Entity<Paciente>()
               .HasIndex(p => p.Nombre)
               .HasDatabaseName("IX_Paciente_Nombre");
            modelBuilder.Entity<Paciente>()
                .HasIndex(p => p.Fecha_Nacimiento)
                 .HasDatabaseName("IX_Paciente_FechaNacimiento");
        }
     

    }
}
