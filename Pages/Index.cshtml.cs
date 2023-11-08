using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics;
namespace Orquestror.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ExecutePythonScript();
    }
    public void ExecutePythonScript()
{
        // Caminho para o interpretador Python
        string pythonPath = @"C:\Windows\py.exe";
        _logger.LogInformation("Python path: {pythonPath}", pythonPath);
        // Caminho para o script Python 
        string scriptPath = @"C:\Users\jagjferr\Documents\test.py";

        // Cria um novo processo
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        // Inicia o processo e aguarda até que ele termine
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Imprime a saída do script Python
        Console.WriteLine(output);
}
}
