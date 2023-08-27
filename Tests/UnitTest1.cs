using Alba;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using WolverineHandlerDisco;
using Xunit.Abstractions;

namespace Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Test1()
    {
        // Can access internal members from other assembly, so InternalsVisibleTo seems to work
        SomeInternalThing.Test();
        
        var host = await AlbaHost.For<Program>(webHostBuilder =>
        {
            webHostBuilder.ConfigureServices(services =>
            {
                services.DisableAllExternalWolverineTransports();
            });
        });

        var opts = host.Services.GetService<WolverineOptions>();
        
        // ApplicationAssembly seems to be correct
        Assert.Equal("WolverineHandlerDisco", opts!.ApplicationAssembly?.GetName().Name);
        
        _testOutputHelper.WriteLine(opts.DescribeHandlerMatch(typeof(CreateIssue)));

        using var scope = host.Services.CreateScope();
        
        // IndeterminateRoutesException happens here
        await scope.ServiceProvider.GetRequiredService<IMessageBus>().InvokeAsync(new CreateIssue("Title", "Description"));
    }
}