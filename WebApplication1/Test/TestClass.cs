using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Entidades;
using Moq;
using WebApplication1.Controllers;
using Xunit;

namespace WebApplication1.Tests
{
    public class PacienteControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public PacienteControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Registro_ReturnsOkResult()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var mockTokenServicio = new Mock<ITokenServices>();
                mockTokenServicio.Setup(m => m.CreartToken(It.IsAny<Paciente>())).Returns("fake_token");

                var controller = new PacienteController(context, mockTokenServicio.Object);
                var registroDto = new RegistroPacienteDto
                {
                    Nombre = "John",
                    Fecha_Nacimiento = new DateTime(1990, 1, 1),
                    correo_Electronico = "john@example.com",
                    Genero = "Masculino",
                    Direccion = "123 Calle Principal",
                    NumeroTelefono = "1234567890"
                };

                // Act
                var result = await controller.Registro(registroDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var model = Assert.IsType<RegistroPacienteDto>(okResult.Value);
                Assert.Equal(registroDto.Nombre, model.Nombre);
            }
        }

        [Fact]
        public async Task ObtenerPaciente_ReturnsOkResult()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var paciente = new Paciente
                {
                    Id = 1,
                    Nombre = "John",
                    Fecha_Nacimiento = new DateTime(1990, 1, 1),
                    correo_Electronico = "john@example.com",
                    Genero = "Masculino",
                    Direccion = "123 Calle Principal",
                    NumeroTelefono = "1234567890"
                };
                context.pacientes.Add(paciente);
                context.SaveChanges();

                var controller = new PacienteController(context, null);

                // Act
                var result = await controller.ObtenerPaciente(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var model = Assert.IsType<PacienteDto>(okResult.Value);
                Assert.Equal(paciente.Nombre, model.Nombre);
            }
        }

        [Fact]
        public async Task EliminarPaciente_ReturnsNoContentResult()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var paciente = new Paciente
                {
                    Id = 1,
                    Nombre = "John",
                    Fecha_Nacimiento = new DateTime(1990, 1, 1),
                    correo_Electronico = "john@example.com",
                    Genero = "Masculino",
                    Direccion = "123 Calle Principal",
                    NumeroTelefono = "1234567890"
                };
                context.pacientes.Add(paciente);
                context.SaveChanges();

                var controller = new PacienteController(context, null);

                // Act
                var result = await controller.EliminarPaciente(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task ActualizarPaciente_ReturnsNoContentResult()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var paciente = new Paciente
                {
                    Id = 1,
                    Nombre = "John",
                    Fecha_Nacimiento = new DateTime(1990, 1, 1),
                    correo_Electronico = "john@example.com",
                    Genero = "Masculino",
                    Direccion = "123 Calle Principal",
                    NumeroTelefono = "1234567890"
                };
                context.pacientes.Add(paciente);
                context.SaveChanges();

                var controller = new PacienteController(context, null);
                var registroDto = new RegistroPacienteDto
                {
                    Nombre = "John Doe",
                    Fecha_Nacimiento = new DateTime(1990, 1, 1),
                    correo_Electronico = "john@example.com",
                    Genero = "Masculino",
                    Direccion = "123 Calle Principal",
                    NumeroTelefono = "1234567890"
                };

                // Act
                var result = await controller.ActualizarPaciente(1, registroDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }
    }
}
