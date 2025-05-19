using SandraAlvaradoFelixPruebaTecnica.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
// Configuración la cultura e idioma
var supportedCultures = new[]
{
    new CultureInfo("es-ES"),
};
// Configura valores de appsettings
var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
builder.Configuration.AddJsonFile(appSettingsPath, optional: true, reloadOnChange: false);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var logToFileEnabled = builder.Configuration.GetValue<bool>("Logging:LogToFile:Enabled");
var logToDbEnabled = builder.Configuration.GetValue<bool>("Logging:LogToDatabase:Enabled");
if (logToFileEnabled && logToDbEnabled)
{
    Console.WriteLine("[Error de configuración] No puede activarse 'LogToFile' y 'LogToDatabase' al mismo tiempo.");
    throw new InvalidOperationException("Solo uno de los sistemas de log puede estar habilitado: 'LogToFile' o 'LogToDatabase'.");
}

if (!logToFileEnabled && !logToDbEnabled)
{
    Console.WriteLine("[Error de configuración] No se ha activado ningún sistema de logging.");
    throw new InvalidOperationException("Debe habilitarse uno de los sistemas de log: 'LogToFile' o 'LogToDatabase'.");
}


var logConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration);

if (logToFileEnabled)
{
    logConfig.WriteTo.File(
        builder.Configuration["Logging:LogToFile:Path"],
        rollingInterval: RollingInterval.Day); // Esto crea un nuevo archivo de log cada día
}

if (logToDbEnabled)
{
    var connectionString = builder.Configuration["Logging:LogToDatabase:ConnectionString"];
    var tableName = builder.Configuration["Logging:LogToDatabase:TableName"];

    var columnOptions = new ColumnOptions
    {
        Level = { ColumnName = "evento" },
        Message = { ColumnName = "evento_descripcion" },
        Exception = { ColumnName = "json_out_nv" },
        Properties = { ColumnName = "json_in_nv_request" },
        Store = new List<StandardColumn>
    {
        StandardColumn.Level,
        StandardColumn.Message,
        StandardColumn.Exception,
        StandardColumn.Properties
    },
        AdditionalColumns = new Collection<SqlColumn>
        {
            new SqlColumn("usuario", SqlDbType.Int),
            new SqlColumn("cliente_ip", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("nombre_procedimiento", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("json_out_nv_response", SqlDbType.NVarChar, dataLength: -1),
        }
    };
    logConfig.WriteTo.MSSqlServer(
        connectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = tableName, AutoCreateSqlTable = false },
        columnOptions: columnOptions
    );
}
Log.Logger = logConfig.CreateLogger();
builder.Logging.AddSerilog();

// Establecer el puerto aquí
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7048, listenOptions => 
    {
        listenOptions.UseHttps(); 
    });
    options.Limits.MaxRequestBodySize = null; 
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

//Texto del swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "JuegosOlimpicos",
        Description = "Esta api permite la integracion de juegos olimpicos",
        Extensions = new Dictionary<string, IOpenApiExtension>
    {
        { "x-deployment-time", new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")) }
    }
    });
    c.OperationFilter<SwaggerResponse>();
});

//Configuracion de idioma
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddLocalization();

//Configuración y verificacion del JWT
var key = Encoding.ASCII.GetBytes("gH@xF7$9k#LmPzR1s3VwXyZ!qTnBvCdE");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Cambia a 'true' en producción
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//Configuiraciones de cors
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if(app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCors("AllowOrigin");
app.UseMiddleware<CorsErrorMiddleware>();
app.UseMiddleware<UnauthorizedMiddleware>();
app.UseAuthorization();

app.MapControllers();

//app.Run();
try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush(); // <-- Asegura que los logs pendientes se escriban
}