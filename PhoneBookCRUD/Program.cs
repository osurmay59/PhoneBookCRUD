using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using PhoneBookCRUD.Data;
using PhoneBookCRUD.Models;
using System.Data;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<ClientsDb>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options => options.AddPolicy("AllowOrigin",
                                    builder => builder.AllowAnyOrigin()
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod()));



builder.Services.Configure<JsonOptions>(opt => opt.SerializerOptions.PropertyNamingPolicy = null) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/v1/clients", async (Client e, ClientsDb db) =>
{
    db.Clients.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"api/v1/clients/{e.PhoneNumber}", e);
}
);

app.MapGet("api/v1/clients/{Id:int}", async (int Id, ClientsDb db) =>
{
    return await db.Clients.FindAsync(Id)
        is Client e
        ? Results.Ok(e)
        : Results.NotFound(Id);
});

app.MapGet("api/v1/clients", async (ClientsDb db) => await db.Clients.ToListAsync());

app.MapPut("api/v1/clients/{Id:int}", async (int Id, Client e, ClientsDb db) =>
{
  
       

    var client = await db.Clients.FindAsync(Id);

    if (client is null) return Results.NotFound();
    client.PhoneNumber = e.PhoneNumber;
    client.FirstName = e.FirstName;
    client.LastName = e.LastName;
    client.Comments = e.Comments;

    await db.SaveChangesAsync();

    return Results.Ok(client);


});

app.MapDelete("api/v1/clients/{Id:int}", async (int Id, ClientsDb db) =>
{
    var client = await db.Clients.FindAsync(Id);

    if (client is null) return Results.NotFound();
    
    db.Clients.Remove(client);
    await db.SaveChangesAsync();
    

    return Results.NoContent();
});



app.UseCors("AllowOrigin");

app.Run();
