using AngularSaleAPI.Application;
using AngularSaleAPI.Application.Validators.Products;
using AngularSaleAPI.Infrastructure;
using AngularSaleAPI.Infrastructure.Enums;
using AngularSaleAPI.Infrastructure.Filters;
using AngularSaleAPI.Infrastructure.Services.Storage.Azure;
using AngularSaleAPI.Infrastructure.Services.Storage.Local;
using AngularSaleAPI.Persistence;
using AngularSaleAPI.SignalR;
using AngularSaleAPI.WebAPI.Configurations.ColumnWriters;
using AngularSaleAPI.WebAPI.Extensions;
using AngularSaleAPI.WebAPI.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//
builder.Services.AddHttpContextAccessor();//clienttan gelen request sonucu olu�an HttpContext nesnesine katmanlardaki classlardan eri�ebilmemizi sa�lar
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
//builder.Services.AddStorage(StorageType.Local);
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MySQL(builder.Configuration.GetConnectionString("MySql"), "logs")
    //.WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    //todo docker kurulumu yap�lacak
    .Enrich.FromLogContext()
    .Enrich.With<UsernameColumnWriter>()
    //todo username kolonuna loglanm�yor
    .MinimumLevel.Information()
    .CreateLogger(); 
builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // kullanacak site/client/api
            ValidateIssuer = true, // olu�turan site/api
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true, // uygulamaya ait security key

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name
        };
    });

//
// parantez i�i manuel kontrol entegresi i�in
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<RolePermissionFilter>();

})
    // validation kontrol� yapt�k
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    
    //otomatik olan validasyon i�lemini manuel yapman� sa�l�yor
    //alttaki kod olmad��� zaman kendisi validasyon yap�yor, controllerda if(ModelState.isValid) kodunu kulland���nda
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
    //
//


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors();
app.UseAuthentication();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.MapHubs();

//
app.UseHttpsRedirection();

app.UseAuthorization();

//
app.Use(async (context, next) => 
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name",username);
    await next(); 
});

//

app.MapControllers();

app.Run();
