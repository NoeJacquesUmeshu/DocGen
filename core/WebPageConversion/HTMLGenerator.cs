using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis.CSharp;

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
            await WriteRecursive(parser.NodeInfo);
        }
        private static async Task WriteRecursive(IDeclarationInfo info)
        {
            if (info is null)
            {
                return;
            }
            if (info as FileDeclarationInfo is FileDeclarationInfo fileInfo)
                foreach (var child in fileInfo.Childrens)
                    if (child as IObjetDeclarationInfo is IObjetDeclarationInfo objectInfo)
                    {
                        await GenerateHtmlForIObject(objectInfo);
                    }

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

            GenerateHtmlForIObjectMember("Fields", info.Fields, html);
            GenerateHtmlForIObjectMember("Properties", info.Properties, html);
            GenerateHtmlForIObjectMember("Constructors", info.Constructors, html);
            GenerateHtmlForIObjectMember("Methods", info.Methods, html);
            GenerateHtmlForIObjectMember("Destructors", info.Destructors, html);
            GenerateHtmlForIObjectMember("Nested Classes", info.NestedClasses, html);

            html.AppendLine("</div>");
        }

        private static void GenerateHtmlForIObjectMember<T>(string memberName, IEnumerable<T> members, StringBuilder html) where T : IDeclarationInfo
        {
            html.AppendLine($"<h2>{memberName}</h2>");
            foreach (var member in members)
            {
                if (member as IObjetDeclarationInfo is IObjetDeclarationInfo childObject)
                {
                    WriteObjectToExistingObjectHTML(childObject, html);
                }

                // check for type and fill HTML
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
            using (StreamWriter writer = new StreamWriter($"{fileName}.html"))
            {
                await writer.WriteAsync(html.ToString());
            }
        }
    }
}