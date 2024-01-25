using System.Net;
using System.Runtime.CompilerServices;
using DocGen;

/// <summary>
/// This is an example class. It's purpouse is to be documented and tested with the program.
/// </summary>
internal class Example
{
    /// <summary>
    /// This method throws 5 exceptions. Only 4 are documented ! 
    /// </summary>
    /// <param name="a">XML Summary of A</param>
    /// <param name="b">XML Summary of B</param>
    /// <param name="c">XML Summary of C</param>
    /// <param name="d">XML Summary of D</param>
    /// <exception cref="OutOfMemoryException"></exception>
    /// <exception cref="WebException"></exception>
    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="Exception"></exception>
    /// <returns>returns a + b + c + d. After throwing 5 consecutive exceptions, of course ! (2 regular exceptions).</returns>
    private static string ExampleMethod(string a, string b, string c, string d)
    {
        throw new OutOfMemoryException();
        throw new WebException();
        throw new IndexOutOfRangeException();
        throw new Exception();
        throw new Exception();
        return a + b + c + d;
    }
}
/// <summary>
/// This is the program, now documentated ! 
/// </summary>
public static class Program
{
    /// <summary>
    /// This is where the program decides where the stuff goes, on Output/InputPath/htmlStuffs 
    /// </summary>
    public static string OutputFolder = "";

    /// <summary>
    /// This is the program's main. This prompts the user for a folder path and generate documentations for each .cs files.
    /// </summary>
    /// <param name="args">By typing the path when running the program, you can bypass the prompt section.</param>
    /// <returns>An async task</returns>
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
        var csFiles = GetCSFilesInPath(path);
        Console.WriteLine($"Generating documentation for {path}");
        var tasks = new List<Task>();
        foreach (var csFilePath in csFiles)
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
