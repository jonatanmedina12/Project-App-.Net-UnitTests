using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Models.Dtos.RegistroPacienteDto;

namespace Models.Dtos
{
    public class RespuestaDto
    {
        
            public string Nombre { get; set; }

            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
             public string Fecha_Nacimiento { get; set; }
            public string Correo_Electronico { get; set; }
            public string Genero { get; set; }
            public string Direccion { get; set; }
            public string NumeroTelefono { get; set; }

            public string Token { get; set; }

            public string Mensaje { get; set; }
     
    }

}
