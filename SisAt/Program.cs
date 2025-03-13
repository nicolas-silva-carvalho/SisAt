using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SisAt.API;
using SisAt.DataBase;
using SisAt.Helper;
using SisAt.Jobs;
using SisAt.Models;
using SisAt.Repository.Persistence;
using SisAt.Repository.Persistence.Interfaces;
using SisAt.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "Home/Index";
    options.LogoutPath = "/Home/Logout";
    options.AccessDeniedPath = "/Home/AcessoNegado";
    options.ReturnUrlParameter = "Home";
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SISAT")));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SISAT")));
builder.Services.AddIdentity<Usuario, IdentityRole<long>>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders().AddErrorDescriber<LocalizedIdentityErrorDescriber>();

builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

builder.Services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage(builder.Configuration.GetConnectionString("SISAT")));
builder.Services.AddHangfireServer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICadastroDeHorariosPersistence, CadastroDeHorariosPersistence>();
builder.Services.AddScoped<IAgendamentoPersistence, AgendamentoPersistence>();
builder.Services.AddScoped<IImportacaoAPIService, ImportacaoAPIService>();

builder.Services.Configure<SGAApiService>(builder.Configuration.GetSection("SGAApiService"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});

HangFireJobs.Jobs();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
