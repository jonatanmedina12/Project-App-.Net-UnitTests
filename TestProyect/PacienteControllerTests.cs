using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Data.Interfaces;
using Data;
using Models.Dtos;
using Models.Entidades;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestFixture]
    public class PacienteControllerTests
    {
        private PacienteController _controller;
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<ITokenServices> _tokenServicesMock;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>();
            _tokenServicesMock = new Mock<ITokenServices>();
            _controller = new PacienteController(_dbContextMock.Object, _tokenServicesMock.Object);
        }

        [Test]
        public async Task BuscarPacientePorNombre_ReturnsOk()
        {
            // Arrange
            var nombre = "nombreEjemplo";
            _dbContextMock.Setup(db => db.pacientes.FirstOrDefaultAsync(p => p.Nombre == nombre))
                .ReturnsAsync(new Paciente { Nombre = nombre });

            // Act
            var response = await _controller.BuscarPacientePorNombre(nombre) as OkObjectResult;

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task Registro_ReturnsOk()
        {
            // Arrange
            var registroDto = new RegistroPacienteDto
            {
                Nombre = "NombrePrueba",
                Edad = 30
                // Configura otras propiedades necesarias
            };

            _dbContextMock.Setup(db => db.pacientes.AnyAsync(It.IsAny<Expression<Func<Paciente, bool>>>()))
                .ReturnsAsync(false); // Simula que el paciente no existe en la base de datos

            // Act
            var response = await _controller.Registro(registroDto);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ObtenerPacientes_ReturnsOkResult()
        {
            // Arrange
            var pacientes = new List<Paciente>
            {
                new Paciente { Nombre = "Paciente1" },
                new Paciente { Nombre = "Paciente2" }
            };

            _dbContextMock.Setup(db => db.pacientes.ToListAsync())
                .ReturnsAsync(pacientes);

            // Act
            var result = await _controller.ObtenerPacientes();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(pacientes, okResult.Value);
        }

        [Test]
        public async Task ObtenerPacientePorId_ReturnsOkResult()
        {
            // Arrange
            var pacienteId = 1;
            var paciente = new Paciente { Id = pacienteId, Nombre = "Paciente1" };

            _dbContextMock.Setup(db => db.pacientes.FindAsync(pacienteId))
                .ReturnsAsync(paciente);

            // Act
            var result = await _controller.ObtenerPacientePorId(pacienteId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(paciente, okResult.Value);
        }
    }
}