
using DocGen;

/// <summary>
/// this is the program summary
/// </summary>
public static class Program
{
    /// <summary>
    /// thats the path
    /// </summary>
    static string path = "C:\\Users\\Noé Jacques\\Desktop\\PureC#\\--DocGen\\Program.cs";

    static async Task Main(string[] args)
    {

        var parser = await CodeParser.Create(path);
        if (parser.NodeInfo is null)
        {
            Console.WriteLine("NodeInfo is null");
            return;
        }

        await HtmlGenerator.ToHTML(parser);
    }

    public class NestedClass
    {
        public int a;
        public string b;
        public NestedClass(int a, string b)
        {
            this.a = a;
            this.b = b;
        }
    }
}
