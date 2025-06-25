using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/{id}", (Id id) =>
{
    if (id == null)
    {
        return Results.BadRequest("Invalid ID format.");
    }

    var value = $"Value for ID {id.Value}";

    return Results.Ok(value);
});

app.Run();

public class Id
{
    public long Value { get; set; }

    public static ValueTask<Id?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var id = context.Request.RouteValues["id"]?.ToString();
        
        if (string.IsNullOrEmpty(id) || !long.TryParse(id, out var value))
        {
            return ValueTask.FromResult<Id?>(null);
        }

        return ValueTask.FromResult<Id?>(new Id { Value = value });
    }
}