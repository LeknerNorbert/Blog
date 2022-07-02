using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

// Cors engedélyezése
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Az API-okat biztonsági okokból alapesetben csak arról az URL-rõl lehet hívni, amin a kiszolgáló alkalmazás fut.
// Ha szeretnénk elérni ezeket más webhelyrõl is (ilyen mondjuk egy külön szerveren, - esetünkben localhoston - futó frontend),
// akkor külön engedélyeznünk kell azt. A "policy.withOrigins" nevü függvényben lehet definiálni olyan URL-eket, amiknek megengedjük, hogy 
// szintén hozzáférjenek az API szolgáltatásunkhoz. 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddControllers()
    .AddNewtonsoftJson();

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

// Cors engedélyezése
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
