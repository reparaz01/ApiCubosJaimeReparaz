using ApiCubosJaimeReparaz.Helpers;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using NSwag.Generation.Processors.Security;
using NSwag;
using ApiCubosJaimeReparaz.Repositories;
using ApiCubosJaimeReparaz.Data;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

//DEBEMOS PODER RECUPERAR UN OBJETO INYECTADO EN CLASES
//QUE NO TIENEN CONSTRUCTOR 

//KAYVAULT

SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret secretSecretKey = await secretClient.GetSecretAsync("secretSecretKey");
KeyVaultSecret secretAudience = await secretClient.GetSecretAsync("secretAudience");
KeyVaultSecret secretIssuer = await secretClient.GetSecretAsync("secretIssuer");

string secretKey = secretSecretKey.Value;
string audience = secretAudience.Value;
string issuer = secretIssuer.Value;

KeyVaultSecret secret = await secretClient.GetSecretAsync("secretConnectionString"); //nombre del secret
string connectionString = secret.Value;

HelperActionServicesOAuth helper = new HelperActionServicesOAuth(secretKey, audience, issuer);

builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);


builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

//AZURE NORMAL
//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");

builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddDbContext<CubosContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// REGISTRAMOS SWAGGER COMO SERVICIO
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api Cubos Jaime Reparaz";
    document.Description = "Segunda Practica Azure";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();
app.UseOpenApi();
//app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/css/monokai_theme.css");
    //options.InjectStylesheet("/css/material3x.css");
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api Cubos");
    options.RoutePrefix = "";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();