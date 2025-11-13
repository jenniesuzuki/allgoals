using AllGoals.Application.Interfaces;
using AllGoals.Application.Services;
using AllGoals.Domain.Interfaces;
using AllGoals.Infrastructure.Data;
using AllGoals.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
        
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString, oraOptions => 
    {

    })
);

builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStoreItemRepository, StoreItemRepository>();

builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStoreItemService, StoreItemService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
        
app.UseAuthorization();

app.MapControllers();
app.Run();