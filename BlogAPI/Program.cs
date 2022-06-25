using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

// Cors enged�lyez�se
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Az API-okat biztons�gi okokb�l alapesetben csak arr�l az URL-r�l lehet h�vni, amin a kiszolg�l� alkalmaz�s fut.
// Ha szeretn�nk el�rni ezeket m�s webhelyr�l is (ilyen mondjuk egy k�l�n szerveren, - eset�nkben localhoston - fut� frontend),
// akkor k�l�n enged�lyezn�nk kell azt. A "policy.withOrigins" nev� f�ggv�nyben lehet defini�lni olyan URL-eket, amiknek megengedj�k, hogy 
// szint�n hozz�f�rjenek az API szolg�ltat�sunkhoz. 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500");
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

// Cors enged�lyez�se
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
