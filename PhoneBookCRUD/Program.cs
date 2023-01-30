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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/clients/", async (Client e, ClientsDb db) =>
{
    db.Clients.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"/client/{e.PhoneNumber}", e);
}
);

app.MapGet("/clients/{PhoneNumber:int}", async (int PhoneNumber, ClientsDb db) =>
{
    return await db.Clients.FindAsync(PhoneNumber)
        is Client e
        ? Results.Ok(e)
        : Results.NotFound(PhoneNumber);
});

app.MapGet("/clients", async (ClientsDb db) => await db.Clients.ToListAsync());

app.MapPut("/clients/{PhoneNumber:int}", async (int PhoneNumber, Client e, ClientsDb db) =>
{
    if (e.PhoneNumber != PhoneNumber)
        return Results.BadRequest();

    var client = await db.Clients.FindAsync(PhoneNumber);

    if (client is null) return Results.NotFound();

    client.FirstName = e.FirstName;
    client.LastName = e.LastName;
    client.Comments = e.Comments;

    await db.SaveChangesAsync();

    return Results.Ok(client);


});

app.MapDelete("/clients/{PhoneNumber:int}", async (int PhoneNumber, ClientsDb db) =>
{
    var client = await db.Clients.FindAsync(PhoneNumber);

    if (client is null) return Results.NotFound();
    
    db.Clients.Remove(client);
    await db.SaveChangesAsync();
    

    return Results.NoContent();
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}