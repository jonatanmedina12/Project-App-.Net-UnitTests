using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



// Nota: Para utilizar los endpoints que requieren autorización, es necesario generar un token.
// Esto se puede hacer al crear un usuario o al buscar por el nombre, ya que devuelve el token.
// El token generado debe ser copiado y utilizado de la siguiente manera en Swagger:
// "Bearer <token_generado>"
// Se debe pegar en el campo de autorización al hacer clic en el botón "Authorize" y luego iniciar sesión.

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenServices _tokenServicio;

        public PacienteController(ApplicationDbContext db, ITokenServices tokenServicio)
        {
            _db = db;
            _tokenServicio = tokenServicio;
        }

        // Método privado para verificar si un paciente existe en la base de datos por su ID
        private bool PacienteExists(int id)
        {
            return _db.pacientes.Any(e => e.Id == id);
        }

        // Método privado para verificar si un paciente existe en la base de datos por su nombre
        private async Task<bool> PacienteExisteDb(string nombre)
        {
            return await _db.pacientes.AnyAsync(x => x.Nombre == nombre.ToLower());
        }

        // Endpoint para buscar un paciente por su nombre
        [HttpGet("buscar")]
        public async Task<ActionResult<RespuestaDto>> BuscarPacientePorNombre([FromQuery] string nombre)
        {
            var paciente = await _db.pacientes.FirstOrDefaultAsync(p => p.Nombre.ToLower() == nombre.ToLower());

            if (paciente == null)
            {
                return NotFound();
            }

            // Verificar si el nombre es válido
            using var hmac = new HMACSHA512(paciente.passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(nombre));
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != paciente.passwordHash[i])
                {
                    return Unauthorized("Nombre no válido");
                }
            }

            // Generar token si el paciente y la contraseña son válidos
            var token = _tokenServicio.CreartToken(paciente);

            return Ok(new
            {
                Message = $"Bearer {token}",
                Usuario = paciente.Nombre
            });
        }

        // Endpoint para registrar un nuevo paciente
        [HttpPost("registro")]
        public async Task<ActionResult<RegistroPacienteDto>> Registro(RegistroPacienteDto registroPacienteDto)
        {
            if (await PacienteExisteDb(registroPacienteDto.Nombre))
            {
                return BadRequest($"Usuario ya registrado: {registroPacienteDto.Nombre}");
            }
            // Validar correo electrónico
            if (!registroPacienteDto.Correo_Electronico.Contains("@"))
            {
                return BadRequest("Correo electrónico inválido");
            }

            // Validar género
            if (registroPacienteDto.Genero.ToLower() != "male" && registroPacienteDto.Genero.ToLower() != "female" && registroPacienteDto.Genero.ToLower() != "hombre" && registroPacienteDto.Genero.ToLower() != "mujer")
            {
                return BadRequest("Género inválido");
            }

            // Validar dirección
            if (!registroPacienteDto.Direccion.Contains("#"))
            {
                return BadRequest("Dirección inválida");
            }
            // Validar fecha de nacimiento
            if (!Regex.IsMatch(registroPacienteDto.Fecha_Nacimiento, @"^\d{4}[-/]\d{2}[-/]\d{2}$"))
            {
                return BadRequest("Fecha de nacimiento inválida. Debe estar en el formato YYYY-MM-DD o YYYY/MM/DD");
            }

            // Generar hash y salt para la contraseña
            using var hmac = new HMACSHA512();
            var paciente = new Paciente
            {
                Nombre = registroPacienteDto.Nombre.ToLower(),
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroPacienteDto.Nombre)),
                passwordSalt = hmac.Key,
                Fecha_Nacimiento = registroPacienteDto.Fecha_Nacimiento,
                Correo_Electronico = registroPacienteDto.Correo_Electronico,
                Genero = registroPacienteDto.Genero,
                Direccion = registroPacienteDto.Direccion,
                NumeroTelefono = registroPacienteDto.NumeroTelefono,
                Is_Active = 1
            };

            _db.pacientes.Add(paciente);
            await _db.SaveChangesAsync();
            var Id_generado = paciente.Id;
            var token = _tokenServicio.CreartToken(paciente);

            return Ok(new
            {
                Message = $"Registro de nuevo usuario correcto. ID generado: {Id_generado}",
                Token = $"Bearer {token}"

            });
        }

        // Endpoint para obtener información de un paciente por su ID
        [Authorize]
        [HttpGet("ObtenerPaciente/{id}")]
        public async Task<ActionResult<Paciente>> ObtenerPaciente(int id)
        {
            var paciente = await _db.pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(new RespuestaDto
            {
                Mensaje = "Usuario Encontrado",
                Nombre = paciente.Nombre,
                Correo_Electronico = paciente.Correo_Electronico,
                Genero = paciente.Genero,
                Direccion = paciente.Direccion,
                Fecha_Nacimiento = paciente.Fecha_Nacimiento,
                NumeroTelefono = paciente.NumeroTelefono,
                Token = _tokenServicio.CreartToken(paciente)
            });
        }

        // Endpoint para eliminar un paciente por su ID
        [Authorize]
        [HttpDelete("EliminarPaciente/{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            var paciente = await _db.pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _db.pacientes.Remove(paciente);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Usuario eliminado. ID: {paciente.Id}"
            });
        }

        // Endpoint para desactivar un paciente por su ID
        [Authorize]
        [HttpPost("DesactivarPaciente/{id}")]
        public async Task<IActionResult> DesactivarPaciente(int id)
        {
            var paciente = await _db.pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            paciente.Is_Active = 0;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Usuario desactivado. ID: {paciente.Id}"
            });
        }

        // Endpoint para actualizar la información de un paciente por su ID
        [Authorize]
        [HttpPut("ActualizarPaciente/{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, RegistroPacienteDto registroPacienteDto)
        {
            var paciente = await _db.pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            // Actualizar información del paciente
            paciente.Nombre = registroPacienteDto.Nombre;
            paciente.Fecha_Nacimiento = registroPacienteDto.Fecha_Nacimiento;
            paciente.Correo_Electronico = registroPacienteDto.Correo_Electronico;
            paciente.Genero = registroPacienteDto.Genero;
            paciente.Direccion = registroPacienteDto.Direccion;
            paciente.NumeroTelefono = registroPacienteDto.NumeroTelefono;

            _db.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    Message = $"Usuario actualizado. ID: {paciente.Id}"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
