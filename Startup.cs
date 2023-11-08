using Hangfire;
using Hangfire.SqlServer;
using Orquestror.Filter;
namespace Orquestror;
public class Startup{
    public IConfiguration Configuration { get; }
     private const string NeverExecute = "0 0 5 31 2 ?";
    public Startup(IConfiguration configuration){
        Configuration = configuration;
    }

    private IEnumerable<IDisposable> GetHandfireServers()
    {
        GlobalConfiguration.Configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(Configuration.GetConnectionString("TestDB"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        });
        yield return new BackgroundJobServer();
    }


    public void ConfigureServices(IServiceCollection services)
    {
        string? connection = Configuration.GetConnectionString("TestDB");

        if (string.IsNullOrEmpty(connection))
        {
            throw new Exception("Missing connection string 'TestDB'. Please check your configuration.");
        }

        services.AddControllers();
        services.AddControllersWithViews();
        services.AddRazorPages();

        GetHandfireServers();
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connection, new SqlServerStorageOptions()
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            });
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
        });
    }
    [Obsolete]
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        
    
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
            IgnoreAntiforgeryToken = true
        });

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseCookiePolicy();

        app.UseAuthorization();

        app.UseRouting();
        // ConfigureJobs();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        }
            );

         app.UseHangfireServer();
    }
}