using Spectre.Console;

namespace Lab34
{
    public static class UIHelpers
    {
        public static void RenderUserPrompt(string text)
        {
            var label = new Markup("[bold green]You:[/]");
            var panel = new Panel(text)
                .Border(BoxBorder.Rounded)
                .BorderStyle(new Style(Color.Green))
                .Padding(2, 0);

            AnsiConsole.Write(Align.Right(label));
            AnsiConsole.Write(
                Align.Right(panel)
            );
        }

        public static Panel RenderAiPanel(string text)
        {
            return new Panel(text)
                .Border(BoxBorder.Rounded)
                .BorderStyle(new Style(Color.Grey))
                .Padding(2, 0);

        }
    }
}
