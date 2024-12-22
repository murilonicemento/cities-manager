using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Identity;
using CitiesManager.WebAPI.Services;
using CitiesManager.WebAPI.ServicesContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader =
        new UrlSegmentApiVersionReader(); // reads version number from request url at "apiVersion" constraint
    // config.ApiVersionReader = new QueryStringApiVersionReader(); // reads version number from request query string called "api-version"
    // config.ApiVersionReader = new HeaderApiVersionReader(); // reads version number from request header called "api-version"

    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Swagger
builder.Services.AddEndpointsApiExplorer(); // generates description for all endpoints

builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cities Web API", Version = "1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Cities Web API", Version = "2" });
}); // generates OpenAPI specification

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [])
            .WithHeaders("Authorization", "origin", "accept", "content-type")
            .WithMethods("GET", "POST", "PUT", "DELETE");
    });

    options.AddPolicy("4100Client", policy =>
    {
        policy
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>() ?? [])
            .WithHeaders("Authorization", "origin", "accept")
            .WithMethods("GET");
    });
});

// Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger(); // creates endpoint for swagger.json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2");
}); // creates swagger UI for testing all web api endpoints / action methods
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();