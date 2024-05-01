using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Interfaces;
using Models.Dtos;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Models.Entidades;
using WebApplication1.Controllers;
using System.Linq.Expressions;

namespace WebApplication1.Tests
{
    [TestFixture]
    public class PacienteControllerTests
    {
        private ApplicationDbContext _dbContext;
        private Mock<ITokenServices> _mockTokenServices;
        private PacienteController _pacienteController;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _mockTokenServices = new Mock<ITokenServices>();
            _pacienteController = new PacienteController(_dbContext, _mockTokenServices.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }


       

        [Test, Order(1)]
        public async Task Registro_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var registroDto = new RegistroPacienteDto
            {
                Nombre = "johndoeasdasdasdasd",
                Correo_Electronico = "johndosadasdasdasdasdasdasdasde@example.com",
                Genero = "Male",
                Direccion = "123 Main St. #456asdasdasdasdasdasd",
                Fecha_Nacimiento = "1990-01-01",
                NumeroTelefono = "123456789012211212"
            };

            // Act
            var result = await _pacienteController.Registro(registroDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }
        [Test, Order(2)]
        public async Task BuscarPacientePorNombre_ExistingPatient_ReturnsOkResult()
        {
            // Arrange
            var pacienteName = "johndoeasdasdasdasd";


            // Act
            var result = await _pacienteController.BuscarPacientePorNombre(pacienteName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test, Order(3)]
        public async Task ObtenerPaciente_ExistingPatient_ReturnsOkResult()
        {
            // Arrange
            var patientId = 1;
        

            // Act
            var result = await _pacienteController.ObtenerPaciente(patientId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

        }




        [Test, Order(4)]
        public async Task DesactivarPaciente_ExistingPatient_ReturnsOkResult()
        {
            // Arrange
            var patientId = 1;
      

            // Act
            var result = await _pacienteController.DesactivarPaciente(patientId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test, Order(5)]
        public async Task ActualizarPaciente_ExistingPatient_ReturnsOkResult()
        {
            // Arrange
            var patientId = 1;
     

            var updateDto = new RegistroPacienteDto
            {
                Nombre = "updatedname",
                Correo_Electronico = "updated@example.com",
                Genero = "Female",
                Direccion = "321 Main St. #789",
                Fecha_Nacimiento = "1995-05-15",
                NumeroTelefono = "9876543210"
            };

            // Act
            var result = await _pacienteController.ActualizarPaciente(patientId, updateDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
         
        }
        [Test, Order(6)]
        public async Task EliminarPaciente_ExistingPatient_ReturnsOkResult()
        {
            // Arrange
            var patientId = 1;


            // Act
            var result = await _pacienteController.EliminarPaciente(patientId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
