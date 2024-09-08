using linkQuest_server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowany", p =>
                {
                    p.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

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
