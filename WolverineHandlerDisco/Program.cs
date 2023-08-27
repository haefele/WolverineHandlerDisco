using System.Runtime.CompilerServices;
using Oakton;
using Wolverine;
using WolverineHandlerDisco;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseWolverine(f =>
        {
            // Took this code snippet from the documentation here (https://wolverine.netlify.app/guide/handlers/discovery.html#troubleshooting-handler-discovery)
            // Weirdly enough, it also prints MISS for everything
            Console.WriteLine(f.DescribeHandlerMatch(typeof(CreateIssue)));
        });

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapGet("/create-issue", async (IMessageBus messageBus, WolverineOptions options) =>
        {
            // Even now it still prints MISS for everything
            Console.WriteLine(options.DescribeHandlerMatch(typeof(CreateIssue)));
            
            var command = new CreateIssue("Title", "Description");
            await messageBus.InvokeAsync(command);
        });

        return await app.RunOaktonCommands(args);
    }
}