
using Microsoft.EntityFrameworkCore;
using APIPrueba.Models;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MoviesDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}
);
var misReglasCors = "ReglasCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: misReglasCors,
                      builder =>
                      {
                          builder.AllowAnyOrigin() // permite cualquier origen
                                 .AllowAnyMethod() // permite cualquier método
                                 .AllowAnyHeader(); // permite cualquier header
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(misReglasCors); // Make sure CORS is configured before UseRouting if you are using it
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
