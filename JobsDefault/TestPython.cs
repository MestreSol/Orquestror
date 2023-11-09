using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.SqlServer;
using Orquestror.Filter;
using Orquestror.JobsDefault;
using System;
using System.Diagnostics;
namespace Orquestror.JobsDefault;
public class TestPython :  IServerFilter
{
    
    public void OnPerforming(PerformingContext filterContext){
       filterContext.WriteLine("Iniciando Job");
       filterContext.WriteProgressBar(50);
    }
     public void OnPerformed(PerformedContext filterContext)
    {
        filterContext.WriteProgressBar();
        filterContext.WriteLine("Finalizado");
    }
}