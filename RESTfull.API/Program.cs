using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Interfaces;
using RESTfull.Infrastructure.Data;
using RESTfull.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite("Data Source=RESTfull.db"));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    Console.WriteLine($"Origin: {context.Request.Headers["Origin"]}");
    await next();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();