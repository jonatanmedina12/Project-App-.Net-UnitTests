using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Models.Entidades
{

    /*  add.migration "inicial" / update-database para generar la base de datos y las tablas   */
    public class Paciente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Nombre del paciente
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "¡El nombre debe ser mínimo de 10 caracteres!")]
        public string Nombre { get; set; }

        [Required]
        // Fecha de nacimiento del paciente
        public string Fecha_Nacimiento { get; set; }

        // Correo electrónico del paciente
        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "El correo es muy corto")]
        public string Correo_Electronico { get; set; }

        // Género del paciente
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El género es muy corto")]
        public string Genero { get; set; }

        // Dirección del paciente
        [StringLength(100, MinimumLength = 25, ErrorMessage = "La dirección es muy corta")]
        public string Direccion { get; set; }

        // Número de teléfono del paciente
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El número de teléfono es muy corto")]
        public string NumeroTelefono { get; set; }

        // Fecha de admisión del paciente
        public DateTime Fecha_Admision { get; set; }

        // Hash de la contraseña del paciente
        public byte[] passwordHash { get; set; }

        // Salt de la contraseña del paciente
        public byte[] passwordSalt { get; set; }

        //para no eliminar al paciente solo desactivarlo de la base de datos 
        public int Is_Active {  get; set; }

        // Constructor de la clase Paciente
        public Paciente()
        {
            // Establecer valores predeterminados para propiedades requeridas
            Correo_Electronico = "";
            Direccion = "";
            Fecha_Nacimiento = "";
            Genero = "";
            NumeroTelefono = "";
            passwordHash = new byte[0];
            passwordSalt = new byte[0];
            // Establecer la fecha de admisión como la fecha actual al crear un nuevo Paciente
            Fecha_Admision = DateTime.Now;
        }
    

    }
}
