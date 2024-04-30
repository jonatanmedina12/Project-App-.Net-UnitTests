using Data;
using Data.Interfaces;
using Data.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Controllers
{

    public class PacienteController : BaseApiController
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenServices _tokenServicio;

        public PacienteController(ApplicationDbContext db, ITokenServices tokenServicio)
        {
            _db = db;
            _tokenServicio = tokenServicio;
        }
        private bool PacienteExists(int id)
        {
            return _db.pacientes.Any(e => e.Id == id);
        }
        private async Task<bool> PacienteExisteDb(string nombre)
        {


            return await _db.pacientes.AnyAsync(x => x.Nombre == nombre.ToLower());


        }
        [HttpGet]
        public async Task<ActionResult<PacienteDto>> BuscarPacientePorNombre([FromQuery] string nombre)
        {
            var paciente = await _db.pacientes
                .Where(p => p.Nombre.ToLower() == nombre.ToLower())
                .FirstOrDefaultAsync();
            using var hmac = new HMACSHA512(paciente.passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(paciente.Nombre));
            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != paciente.passwordHash[i])
                {
                    return Unauthorized("password no valido");
                }

            }
            return Ok(new tokenDto
            {
                Nombre = paciente.Nombre,
                token = _tokenServicio.CreartToken(paciente)
            });
            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(new PacienteDto
            {
                Nombre = paciente.Nombre,
                Fecha_Nacimiento = paciente.Fecha_Nacimiento,
                Correo_Electronico = paciente.correo_Electronico,
                Genero = paciente.Genero,
                Direccion = paciente.Direccion,
                NumeroTelefono = paciente.NumeroTelefono,
                token =paciente.token
            });
        }
        [HttpPost("registro")] // post api /usuario/registro
        public async Task<ActionResult<RegistroPacienteDto>> Registro(RegistroPacienteDto registroPacienteDto)
        {

            if (await PacienteExisteDb(registroPacienteDto.Nombre))
            {
                return BadRequest("USERNAME YA REGISTRADO...");
            }
            using var hmac = new HMACSHA512();

            var paciente = new Paciente
            {
                Nombre = registroPacienteDto.Nombre.ToLower(),

                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroPacienteDto.Nombre)),
                passwordSalt = hmac.Key,
                Fecha_Nacimiento =registroPacienteDto.Fecha_Nacimiento,
                correo_Electronico = registroPacienteDto.correo_Electronico,
                Genero =registroPacienteDto.Genero,
                Direccion = registroPacienteDto.Direccion,
                NumeroTelefono =registroPacienteDto.NumeroTelefono,
                token="0"

            };
            _db.pacientes.Add(paciente);
            await _db.SaveChangesAsync();

            return Ok(new RegistroPacienteDto
            {
                Nombre = registroPacienteDto.Nombre,
                Fecha_Nacimiento = registroPacienteDto.Fecha_Nacimiento,
                correo_Electronico = registroPacienteDto.correo_Electronico,
                Genero = registroPacienteDto.Genero,
                Direccion = registroPacienteDto.Direccion,
                NumeroTelefono = registroPacienteDto.NumeroTelefono,
                

            });





        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> ObtenerPaciente(int id)
        {
            var paciente = await _db.pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(new PacienteDto
            {
                Nombre = paciente.Nombre,
                Fecha_Nacimiento = paciente.Fecha_Nacimiento,
                Correo_Electronico = paciente.correo_Electronico,
                Genero = paciente.Genero,
                Direccion = paciente.Direccion,
                NumeroTelefono = paciente.NumeroTelefono,
                token = paciente.token
            });
        }


        // DELETE api/pacientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            var paciente = await _db.pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _db.pacientes.Remove(paciente);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPaciente(int id, RegistroPacienteDto registroPacienteDto)
        {
            var paciente = await _db.pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound();
            }

            paciente.Nombre = registroPacienteDto.Nombre;
            paciente.Fecha_Nacimiento = registroPacienteDto.Fecha_Nacimiento;
            paciente.correo_Electronico = registroPacienteDto.correo_Electronico;
            paciente.Genero = registroPacienteDto.Genero;
            paciente.Direccion = registroPacienteDto.Direccion;
            paciente.NumeroTelefono = registroPacienteDto.NumeroTelefono;

            _db.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
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

            return NoContent();
        }

    }
}
