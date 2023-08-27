namespace WolverineHandlerDisco;

public record CreateIssue(string Title, string Description);

public static class CreateIssueHandler
{
    public static async Task Handle(CreateIssue command)
    {
        await Task.Delay(1000);
        await Console.Out.WriteLineAsync("Created issue " + command.Title);
    }
}