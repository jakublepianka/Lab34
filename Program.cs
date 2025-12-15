using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using Spectre.Console;
using System.Text;
using Lab34;

var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.Extensions.Http", LogLevel.Warning);

builder.Services.AddChatClient(sp =>
{
    IChatClient client = new OllamaApiClient("http://localhost:11434", "deepseek-v3.1:671b-cloud");
    return client.AsBuilder().UseFunctionInvocation().Build(sp);
});

builder.Services.AddHttpClient<AlphaVantageClient>(client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
});

var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

AIFunction[] functions = 
[
    AIFunctionFactory.Create(AiFunctions.GetCurrentDateTime),
    AIFunctionFactory.Create(AiFunctions.GetQuoteResponseAsync),
    AIFunctionFactory.Create(AiFunctions.GetMarketStatusResponseAsync),
    AIFunctionFactory.Create(AiFunctions.GetSymbolSearchResponseAsync)
];

var chatOptions = new ChatOptions { Tools = [..functions] };

List<ChatMessage> chatHistory = 
[
    new(ChatRole.System, """
        Provide concise, direct answers.
        Use available tools whenever they can improve correctness or completeness.
        You may call multiple tools per request and combine their results.
        For multi-step tasks (e.g., symbol lookup, pricing, market status), use all relevant tools in sequence.
        If a required tool is unavailable, answer using general knowledge without mentioning tool limitations.
        Use GetCurrentDateTime when a question depends on the current date or time.  
        State assumptions only when necessary.
    """
    )    
];

do
{
    string prompt = await AnsiConsole.AskAsync<string>(
        "You: ",
        """Find Tesla stock, show its current price and tell me whether its main market is currently open.""");

    AnsiConsole.Clear();
    UIHelpers.RenderUserPrompt(prompt);

    chatHistory.Add(new(ChatRole.User, prompt));

    AnsiConsole.MarkupLine("\n[bold]AI:[/]");

    var fullResponse = new StringBuilder();
    await foreach (var chatResponseUpdate in chatClient.GetStreamingResponseAsync(chatHistory, chatOptions))
    {
        fullResponse.Append(chatResponseUpdate.Text);
    }

    AnsiConsole.Write(UIHelpers.RenderAiPanel(fullResponse.ToString()));

    chatHistory.Add(new(ChatRole.Assistant, fullResponse.ToString()));
    Console.WriteLine();


} while (await AnsiConsole.ConfirmAsync("Continue?"));