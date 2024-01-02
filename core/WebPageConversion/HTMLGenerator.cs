using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DocGen
{
    public class HtmlGenerator
    {
        public readonly string path;
        private HtmlGenerator(string path)
        {
            this.path = path;
        }

        public static async Task ToHTML(CodeParser parser)
        {
            await Write(parser.NodeInfo);
        }
        private static async Task Write(ICompositeDeclarationInfo info)
        {
            if (info is null)
            {
                return;
            }
            foreach (var child in info.Childrens)
                if (child as IObjetDeclarationInfo is IObjetDeclarationInfo objectInfo)
                {
                    await GenerateHtmlForIObject(objectInfo);
                }
                else if (child is EnumDeclarationInfo enumInfo)
                {
                    await GenerateHTMLForEnum(enumInfo);
                }
        }
        
        private static async Task GenerateHTMLForEnum(EnumDeclarationInfo info)
        {
            StringBuilder html = new();
            WriteEnumToHTML(info, html);
            await WriteHtmlToFile(html, info.Name);
        }
        private static void WriteEnumToHTML(EnumDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            GenerateHtmlForMember("Enum Members", info.EnumMembers, html);
            html.AppendLine("</div>");
        }

        private static async Task GenerateHtmlForIObject(IObjetDeclarationInfo info)
        {
            StringBuilder html = new();
            WriteNewObjectToHTML(info, html);
            await WriteHtmlToFile(html!, info.Name);
            foreach (var member in info.NestedClasses)
            {
                await GenerateHtmlForIObject(member);
            }
        }

        private static void WriteNewObjectToHTML(IObjetDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            html.AppendLine("<h1>" + info.GetType().Name + "</h1>");
            html.AppendLine("<p>Accessibility: " + info.Accessibility + "</p>");
            html.AppendLine("<p>Name: " + info.Name + "</p>");
            html.AppendLine("<p>Summary: " + info.Summary + "</p>");

            GenerateHtmlForMember("Fields", info.Fields, html);
            GenerateHtmlForMember("Properties", info.Properties, html);
            GenerateHtmlForMember("Constructors", info.Constructors, html);
            GenerateHtmlForMember("Methods", info.Methods, html);
            GenerateHtmlForMember("Destructors", info.Destructors, html);
            GenerateHtmlForMember("Nested Classes", info.NestedClasses, html);

            html.AppendLine("</div>");
        }

        private static void GenerateHtmlForMember<T>(string memberName, IEnumerable<T> members, StringBuilder html) where T : IDeclarationInfo
        {
            html.AppendLine($"<h2>{memberName}</h2>");
            foreach (var member in members)
            {
                if (member as IObjetDeclarationInfo is IObjetDeclarationInfo childObject)
                {
                    WriteObjectToExistingObjectHTML(childObject, html);
                }
            }
        }
        private static void WriteObjectToExistingObjectHTML(IObjetDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            html.AppendLine("<h1>" + info.GetType().Name + "</h1>");
            html.AppendLine("<p>Accessibility: " + info.Accessibility + "</p>");
            html.AppendLine($"<p><a href=\"{info.Name}.html\">{info.Name}</a></p>");
            html.AppendLine("<p>Summary: " + info.Summary + "</p>");

            html.AppendLine("</div>");
        }



        private static async Task WriteHtmlToFile(StringBuilder html, string fileName)
        {
            Console.WriteLine(fileName);
            using (StreamWriter writer = new StreamWriter($"{Program.OutputFolder}/{fileName}.html"))
            {
                await writer.WriteAsync(html.ToString());
            }
        }
    }
}