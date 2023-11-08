namespace Orquestror;
public class Startup{
    public IConfigureation Configuration { get; }
    public Startup(IConfiguration configuration){
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services){
        services.AddControllers();
        services.AddDbContext<OrquestrorContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrquestrorContext")));
        services.AddRazorPages();
        
        GetHangfireServers();
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
                config.UseConsole();
            }
        );
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
        if(env.IsDevelopment()){
            app.UseDeveloperExceptionPage();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
    
}