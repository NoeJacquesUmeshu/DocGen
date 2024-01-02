
using DocGen;

public static class Program
{
    public static string OutputFolder = "";
    private static async Task Main(string[] args)
    {
        foreach (string item in args) Console.WriteLine(item);
        if (!ValidatePathFromUserArguments(args, out string? path))
        {
            path = GetPathFromUserInput();
        }
        if (path == null)
        {
            Console.WriteLine("Cancelling operation.");
            return;
        }
        OutputFolder = $"output/Documentation_{new DirectoryInfo(path).Name}";
        string fullPath = Path.Combine(System.Environment.CurrentDirectory, OutputFolder);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        Console.WriteLine($"Output folder : {fullPath}");
        Console.WriteLine($"Starting operation for {path}...");
        Console.WriteLine("--------------------------------");
        await GenerateForFolder(path);
        Console.WriteLine("--------------------------------");
        Console.WriteLine("Operation complete !");
    }

    private static async Task GenerateForFolder(string path)
    {
        Console.WriteLine($"Generating documentation for {path}...");
        var tasks = new List<Task>();
        foreach (var csFilePath in GetCSFilesInPath(path))
        {
            tasks.Add(GenerateFileDocumentation(csFilePath));
        }
        await Task.WhenAll(tasks);
        foreach (var folderPath in GetSubFolders(path))
        {
            await GenerateForFolder(folderPath);
        }
    }

    static async Task GenerateFileDocumentation(string filePath)
    {
        var parser = await CodeParser.Create(filePath);
        await HtmlGenerator.ToHTML(parser);
    }
    private static string[] GetCSFilesInPath(string path) => Directory.GetFiles(path, "*.cs");
    private static string[] GetSubFolders(string path) => Directory.GetDirectories(path);
    private static string? GetPathFromUserInput()
    {
        string? path = null;
        while (!Directory.Exists(path) && path?.ToLower() != "cancel")
        {
            Console.WriteLine("Please enter a valid directory path. Enter \"Cancel\" to exit the process.");
            path = Console.ReadLine();
        }
        Console.WriteLine(path + Directory.Exists(path));
        return path;
    }

    private static bool ValidatePathFromUserArguments(string[] args, out string? path)
    {
        if (args.Length > 0 && Directory.Exists(args[0]))
        {
            path = args[0];
            return true;
        }

        path = null;
        return false;
    }
}
