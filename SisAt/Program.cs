using Microsoft.EntityFrameworkCore;
using SisAt.API;
using SisAt.DataBase;
using SisAt.Repository.Persistence;
using SisAt.Repository.Persistence.Interfaces;
using SisAt.Sessao;
using SisAt.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SISAT")));
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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
