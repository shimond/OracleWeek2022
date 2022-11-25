using CatalogApi.Data;
using CatalogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("Redis"));
builder.Services.AddDbContext<CatalogDb>(x => x.UseInMemoryDatabase("CATALOG"));

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOutputCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "App v1",
        Version = "v1"
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Name = JwtBearerDefaults.AuthenticationScheme,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
});


var app = builder.Build();
var str = app.Configuration["defaultUser"];
app.UseDeveloperExceptionPage();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseOutputCache();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.MapProducts();
app.Run();


//app.UseStaticFiles();

//app.Use(async (context, next) => {
//    await context.Response.WriteAsync(" A1 ");
//    await next();

//    await context.Response.WriteAsync(" A2 ");
//});


//app.Use(async (context, next) => {
//    await context.Response.WriteAsync(" B1 ");
//    await next();
//    await context.Response.WriteAsync(" B2 ");
//});

//app.UseStaticFiles();

//app.Run(async (context) => {
//    await context.Response.WriteAsync(" RUN ");
//});
