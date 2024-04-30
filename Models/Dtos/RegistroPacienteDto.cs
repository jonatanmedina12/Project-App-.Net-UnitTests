using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class RegistroPacienteDto
    {


        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "¡El nombre  debe ser minimo de 10 caracteres!")]
        [DefaultValue("")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

        public string Nombre { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

        public DateTime Fecha_Nacimiento { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "El correo es muy corto")]
        [DefaultValue("")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]


        public string correo_Electronico { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue("")]

        public string Genero { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue("")]

        public string Direccion { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "¡El telefono debe ser minimo de 10 caracteres!")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue("")]

        public string NumeroTelefono { get; set; }




    }
}
