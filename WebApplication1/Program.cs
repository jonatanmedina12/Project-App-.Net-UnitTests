using Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Extensiones;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AgregarServicioAplicacion(builder.Configuration);

builder.Services.AgregarServicioIdentidad(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
);
app.UseAuthorization();

app.MapControllers();

app.Run();
