using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using OversightService.Repositories;
using Mzeey.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//My Configurations
builder.Services.AddSwaggerGen(
    options => options.SwaggerDoc(
        name: "v1", info: new OpenApiInfo{Title = "Oversight Service Api", Version= "v1"}
    )
);

// builder.Services.AddCors();
// builder.WebHost.UseUrls(
//     "https://localhost/:5001"
// );
string dbPath = builder.Configuration.GetConnectionString("OversightDb");
builder.Services.AddDbContext<Oversight>(options =>
    options.UseSqlServer(dbPath)
);

builder.Services.AddHealthChecks().AddDbContextCheck<Oversight>();

//Injected Services
builder.Services.AddScoped<IResidentialStatusRepository, ResidentialStatusRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IFeesRepository, FeesRepository>();
builder.Services.AddScoped<IEstateAddressesRepository, EstateAddressesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//My configurations
app.UseSwagger();
app.UseSwaggerUI(
    options =>{
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Oversight Service Api Version 1");
        options.SupportedSubmitMethods(new[]{
            SubmitMethod.Get, SubmitMethod.Post,
            SubmitMethod.Put, SubmitMethod.Delete
        });
    }
);

// app.UseCors(
//     options => {
//         options.WithMethods("GET", "POST", "PUT", "DELETE");
//         options.WithOrigins(
//             "https://localhost:6002" // port number of client
//         );
//     }
// );

app.UseHealthChecks(path: "/OversightDBCheck");

app.Run();
