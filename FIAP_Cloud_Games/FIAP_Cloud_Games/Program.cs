using Application.Services;
using Core.Repository;
using FIAP_Cloud_Games;
using FIAP_Cloud_Games.Middlewares;
using InfraEstructure;
using InfraEstructure.Auth;
using InfraEstructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region Documento Swagger
builder.Services.AddSwaggerGen(c =>
{

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Insira o Token Jwt no Formato **{token}**"
    });

    c.OperationFilter<JwtBearerOperationFilter>();
});
#endregion


builder.Services.AddDbContext<AppDbContext>(
    optiions => 
        optiions.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging(false)
    ,ServiceLifetime.Scoped
    );


#region incluindo injecao de dependencia das Interfaces IRepository
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<JogoAppService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UserAppService>();
builder.Services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
builder.Services.AddScoped<BibliotecaAppService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<AdminAppService>();
#endregion


#region Token
builder.Services.AddScoped<TokenGenerate>();


#region configura jwtSettings e extrai a chave para uso imediato na configração do jwt

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettigns = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettigns.SecretKey);
#endregion

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseLogMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
