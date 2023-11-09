using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
public class UniversalContext : DbContext{
    public string ConnectionString {get;private set;}
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(ConnectionString);
    }

    public UniversalContext(DbContextOptions<UniversalContext> options) : base(options)
    {
        //ConnectionString = "data source=jagnte03; initial catalog = T_DIRECTLABORCONTROLLER; persist security info = True; Integrated Security = SSPI; MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False;";
        ConnectionString = "data source=(localdb)\\MSSQLLocalDB; initial catalog = Maestro; persist security info = True; Integrated Security = SSPI; MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False;";
    }
}