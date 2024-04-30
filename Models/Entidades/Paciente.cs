using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Models.Entidades
{
    public class Paciente
    {
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50,MinimumLength =10,ErrorMessage ="¡El nombre nombre debe ser minimo de 10 caracteres!")]
        public string Nombre { get; set; }

        
        public DateTime Fecha_Nacimiento { get; set; }
        [Required]
        [StringLength(100,MinimumLength =20,ErrorMessage ="El correo es muy corto")]
        public string correo_Electronico { get; set; }

        public string Genero { get; set; }

        public string Direccion { get; set; }

        public string NumeroTelefono { get; set; }


        public DateTime Fecha_Admision {  get; set; }

        public string token { get; set; }

        public Paciente()
        {
            // Establecer la fecha de admisión como la fecha actual al crear un nuevo Paciente
            Fecha_Admision = DateTime.Now;
        }


    }
}
