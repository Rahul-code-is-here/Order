// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
// using PizzaShop.Domain.DataContext;
// using PizzaShop.Repository.Implementation;
// using PizzaShop.Repository.Interface;
// using PizzaShop.Service.Implementaion;
// using PizzaShop.Service.Implementation;
// using PizzaShop.Service.Interface;
// using System.Security.Claims;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();

// // Add services to the container.
// var conn = builder.Configuration.GetConnectionString("RMSDbConnection");
// builder.Services.AddDbContext<PizzaShemaContext>(options =>
// {
//     options.UseNpgsql(conn);
// });

// builder.Services.AddControllersWithViews();
// // builder.Services.AddTransient<IUnitOfWork,UnitOfWork>();
// builder.Services.AddTransient<IAuthService,AuthService>();
// builder.Services.AddTransient<IEmailSender, EmailSender>();
// builder.Services.AddTransient<IUserRepository,UserRepository>();
// builder.Services.AddTransient<IAuthTokenService, AuthTokenService>();
// builder.Services.AddTransient<IUserServices,UserServices>();
// // Add these to your service configuration



// // JWT Authentication Configuration
// // var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
// // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// //     .AddJwtBearer(options =>
// //     {
// //         options.RequireHttpsMetadata = false;
// //         options.SaveToken = true;
// //         options.TokenValidationParameters = new TokenValidationParameters
// //         {
// //             ValidateIssuerSigningKey = true,
// //             IssuerSigningKey = new SymmetricSecurityKey(key),
// //             ValidateIssuer = true,
// //             ValidateAudience = true,
// //             ValidateLifetime = true,
// //             ValidIssuer = builder.Configuration["Jwt:Issuer"],
// //             ValidAudience = builder.Configuration["Jwt:Audience"],
// //             RoleClaimType = ClaimTypes.Role 
// //         };
// //     });

// // builder.Services.AddAuthorization(options =>
// // {
// //     options.AddPolicy("AdminOnly", policy => policy.RequireRole("1")); // Role ID 1 for Admins
// //     options.AddPolicy("UserOnly", policy => policy.RequireRole("2")); // Role ID 2 for Normal Users
// // });

// // Configure JWT authentication
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Issuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//     };
// });

// // Configure authorization policies
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireRole("1")); // Role ID 1 for Admins
//     options.AddPolicy("UserOnly", policy => policy.RequireRole("2")); // Role ID 2 for Normal Users
// });

// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Login}/{action=Login}/{id?}");

// app.Run();

// using PizzaShop.Repository.Implementation;
// using PizzaShop.Repository.Interface;
// using PizzaShop.Service.Implementation;
// using PizzaShop.Service.Interface;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserServices, UserServices>();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }
// else
// {
//     app.UseDeveloperExceptionPage();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();

using BussinessLogicLayer.Interface;
using BussinessLogicLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PizzaShop.Domain.DataContext;
using PizzaShop.Repository.Implementation;
using PizzaShop.Repository.Implementations;
using PizzaShop.Repository.Interface;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Implementaion;
using PizzaShop.Service.Implementation;
using PizzaShop.Service.Interface;
using PizzaShop.Services.Implementations;
using PizzaShop.Services.Interfaces;
using PizzaShop.Web.Middlewares;
using PizzaShop.Repository.Implementations;
using PIZZASHOP.Services.Implementations;
using System.Text;

// using PizzaShop.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the PizzaShemaContext with a connection string
builder.Services.AddDbContext<PizzaShemaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RMSDbConnection")));

// Register the repositories and services

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
// builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
// builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService,RolePermissionService>();
builder.Services.AddScoped<IRolePermissionRepository,RolePermissionRepository>();
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITableAndSectionRepository, TableAndSectionRepository>();
builder.Services.AddScoped<ITableAndSectionService,TableAndSectionService>();
builder.Services.AddScoped<ISuperAdminRepository,SuperAdminRepository>();
builder.Services.AddScoped<ITaxAndFeeRepository,TaxAndFeeRepository>();
builder.Services.AddScoped<ITaxAndFeeService,TaxAndFeeService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
// builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("1")); // Role ID 1 for Admins
    options.AddPolicy("UserOnly", policy => policy.RequireRole("2")); // Role ID 2 for Normal Users
});

var app = builder.Build();

app.UseMiddleware<RedirectIfFirstLoginMiddleware>(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();


// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using PizzaShop.Domain.DataContext;
// using PizzaShop.Repository.Implementation;
// using PizzaShop.Repository.Interface;
// using PizzaShop.Service.Implementaion;
// using PizzaShop.Service.Implementation;
// using PizzaShop.Service.Interface;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllersWithViews();

// // Register the PizzaShemaContext with a connection string
// builder.Services.AddDbContext<PizzaShemaContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("RMSDbConnection")));

// // Register the repositories and services
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserServices, UserServices>();
// builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IEmailSender, EmailSender>();

// // Configure JWT authentication
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Issuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//     };
// });

// // Configure authorization policies
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireRole("1")); // Role ID 1 for Admins
//     options.AddPolicy("UserOnly", policy => policy.RequireRole("2")); // Role ID 2 for Normal Users
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }
// else
// {
//     app.UseDeveloperExceptionPage();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Login}/{action=Login}/{id?}");

// app.Run();