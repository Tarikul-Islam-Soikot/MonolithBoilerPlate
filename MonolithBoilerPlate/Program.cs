using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Repository;
using MonolithBoilerPlate.Repository.Pagination;
using MonolithBoilerPlate.Service;
using Serilog;
using MonolithBoilerPlate.Api.Helper.Extensions;
using MonolithBoilerPlate.Api.Helper.Middleware;
using MonolithBoilerPlate.Api.Helper.MapperProfile;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;


string applicationName = "Boiler Plate Application";
var _allowApplicationSpecificOrigins = "AllowApplicationSpecificOrigins";
Console.Title = $"{applicationName} Web Api";

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

builder.Services.AddApplicationDependencies(builder.Configuration);
builder.Services.AddSqlPagination();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddCors(_allowApplicationSpecificOrigins);
builder.Services.AddOptions();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{applicationName} API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
              },
              new List<string>()
            }
        });
});
builder.Services.AddDbContextDependencies();
builder.Services.AddRepositoryDependency();
builder.Services.AddServiceDependency();
builder.Services.AddCaching(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });
builder.Services.AddRateLimiting();
builder.Services.AddJwtAuthentication();
builder.Services.AddMasstransitConfiguration();
builder.Services.AddCronJobs();

builder.Host.UseSerilog((hostingContext, configuration) =>
{
    configuration
    //.MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .ReadFrom.Configuration(hostingContext.Configuration);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = $"{applicationName} API Documentation";
    c.SwaggerEndpoint("./v1/swagger.json", $"{applicationName} API");
});
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(_allowApplicationSpecificOrigins);
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting(nameof(appSettings.RateLimit.FixedByIP));

app.Run();
