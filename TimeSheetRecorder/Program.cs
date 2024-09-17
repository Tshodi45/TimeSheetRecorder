using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;
using TimeSheetRecorder.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//string connString = @"DATA SOURCE=localhost:1521-XEPDB1; PERSIST SECURITY INFO=True;USER ID=DEMOUSER; password=demouser123; Pooling = False; ";

//builder.Services.AddDbContext<TimeSheetRecorder.Data.timeSheetRecorderContext>(options => options.UseOracle(
//builder.Configuration.GetConnectionString("DefaultConn")


builder.Services.AddDbContext<TimeSheetRecorder.Data.timeSheetRecorderContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConn")

));

builder.Services.AddDbContext<UserRecorderContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

builder.Services.AddDefaultIdentity<TimeSheetRecorderUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserRecorderContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
