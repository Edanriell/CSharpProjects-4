#region Import namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Data;
using EntityModels;
using System.Net.Http.Headers;
using System.Net;
#endregion

#region Configure the host web server including services
var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(connectionString)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddNorthwindContext();

/*
string? sqlServerConnection = builder.Configuration
  .GetConnectionString("NorthwindConnection");

if (sqlServerConnection is null)
{
  Console.WriteLine("SQL Server database connection string is missing!");
}
else
{
  Microsoft.Data.SqlClient.SqlConnectionStringBuilder sql =
    new(sqlServerConnection);

  sql.IntegratedSecurity = false;
  sql.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
  sql.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");

  builder.Services.AddNorthwindContext(sql.ConnectionString);
}
*/

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(20);
    options.AddPolicy("views", p => p.SetVaryByQuery("alertstyle"));
});

builder.Services.AddHttpClient(
    name: "Northwind.WebApi",
    configureClient: options =>
    {
        options.DefaultRequestVersion = HttpVersion.Version30;
        options.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;

        options.BaseAddress = new Uri("https://localhost:5151/");
        options.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(mediaType: "application/json", quality: 1.0)
        );
    }
);

builder.Services.AddHttpClient(
    name: "Northwind.MinimalApi",
    configureClient: options =>
    {
        options.BaseAddress = new Uri("http://localhost:5152/");
        options.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json", 1.0)
        );
    }
);

var app = builder.Build();

#endregion

#region Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseOutputCache();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapGet("/notcached", () => DateTime.Now.ToString());
app.MapGet("/cached", () => DateTime.Now.ToString()).CacheOutput();

#endregion

#region Start the host web server listening for HTTP requests.
app.Run();
#endregion
