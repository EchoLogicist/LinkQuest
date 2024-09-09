using linkQuest_server;
using linkQuest_server.Interfaces;
using linkQuest_server.Models;
using linkQuest_server.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowany", p =>
                {
                    p.WithOrigins(["http://localhost:61701","http://localhost:4200"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IDictionary<string, Users>>(opt => new Dictionary<string, Users>());
builder.Services.AddTransient<IRoom, RoomRepo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allowany");

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<communicationHub>("/linkquest");

app.Run();
