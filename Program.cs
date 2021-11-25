using Microsoft.EntityFrameworkCore;
using jsfootball_api.Models;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TeamsContext>(options => 
	options.UseCosmos(builder.Configuration.GetValue<string>("CosmosEndpoint"),
	builder.Configuration.GetValue<string>("CosmosKey"),
	"Football"));
builder.Services.AddDbContext<FixturesContext>(options => 
	options.UseCosmos(builder.Configuration.GetValue<string>("CosmosEndpoint"),
	builder.Configuration.GetValue<string>("CosmosKey"),
	"Football"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
