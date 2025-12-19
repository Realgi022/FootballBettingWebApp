using DAL;
using BLL.Services;
using BLL.Interfaces;
using DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DatabaseConnection>();

// Get connection string from configuration
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException("Connection string is null");

// Repositories (pass connection string via factory)
builder.Services.AddScoped<IMatchRepository>(sp => new MatchRepository(connectionString));
builder.Services.AddScoped<IBetRepository>(sp => new BetRepository(connectionString));
builder.Services.AddScoped<IWalletRepository>(sp => new WalletRepository(connectionString));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));

// Services
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IUserService>(sp =>
{
    var repo = sp.GetRequiredService<IUserRepository>();
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
