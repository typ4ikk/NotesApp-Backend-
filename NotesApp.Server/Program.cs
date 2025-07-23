using Microsoft.EntityFrameworkCore;
using NotesApp.Server.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Npgsql;
using NotesApp.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var masterConnectionString = builder.Configuration.GetConnectionString("MasterConnection");
var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

using (var connection = new NpgsqlConnection(masterConnectionString))
{
    connection.Open();
    var dbName = new NpgsqlConnectionStringBuilder(dbConnectionString).Database;
    var checkDbExists = $"SELECT 1 FROM pg_database WHERE datname = '{dbName}'";

    using (var cmd = new NpgsqlCommand(checkDbExists, connection))
    {
        var exists = cmd.ExecuteScalar() != null;
        if (!exists)
        {
            var createDb = $"CREATE DATABASE \"{dbName}\"";
            using (var createCmd = new NpgsqlCommand(createDb, connection))
            {
                createCmd.ExecuteNonQuery();
            }


            Console.WriteLine($"Database {dbName} created successfully.");
        }
    }
}
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(dbConnectionString));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationContext>();
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while creating the database.");
    }
}    
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();
app.Run();