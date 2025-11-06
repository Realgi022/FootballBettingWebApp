using DAL;
using BLL.Services;
using BLL.Interfaces;
using DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DAL.DatabaseConnection>();

// Match, Bet, Wallet services
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();


// User services
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException("Connection string is null");
builder.Services.AddScoped<IUserRepository, UserRepository>(sp => new UserRepository(builder.Configuration));
builder.Services.AddScoped<IUserService, UserService>(sp =>
    {
        IUserRepository repo = sp.GetRequiredService<IUserRepository>();
        return new UserService(repo);
    });

// Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use sessions before authorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
