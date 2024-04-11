using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = "Host=localhost;Port=5432;Database=usersdb2;Username=postgres;Password=password";
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
builder.Services.AddDbContext<IDataBase, DataBase>(options =>
                options.UseNpgsql(connection), ServiceLifetime.Transient, ServiceLifetime.Transient);

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IChatsService, ChatsService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(_ => _.AllowAnyOrigin().AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();
app.Run();
