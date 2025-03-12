using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SisAt.API;
using SisAt.DataBase;
using SisAt.Jobs;
using SisAt.Models;
using SisAt.Repository.Persistence;
using SisAt.Repository.Persistence.Interfaces;
using SisAt.Sessao;
using SisAt.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization();

builder.Services.AddDbContext<Context>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SISAT")));

builder.Services.AddIdentity<Usuario, IdentityRole<long>>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage(builder.Configuration.GetConnectionString("SISAT")));
builder.Services.AddHangfireServer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICadastroDeHorariosPersistence, CadastroDeHorariosPersistence>();
builder.Services.AddScoped<IAgendamentoPersistence, AgendamentoPersistence>();
builder.Services.AddScoped<IImportacaoAPIService, ImportacaoAPIService>();
builder.Services.AddScoped<IUsuarioPersistence, UsuarioPersistence>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ISessaoFactory, SessaoFactory>();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});
HangFireJobs.Jobs();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
