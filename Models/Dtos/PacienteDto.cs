using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class PacienteDto
    {
        
            public string Nombre { get; set; }
            public DateTime Fecha_Nacimiento { get; set; }
            public string Correo_Electronico { get; set; }
            public string Genero { get; set; }
            public string Direccion { get; set; }
            public string NumeroTelefono { get; set; }

              public string token { get; set; }
    }
}
