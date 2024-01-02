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
            var tasks = new List<Task>();
            foreach (ICompositeDeclarationInfo childInfo in info.Childrens)
               tasks.Add(WriteCompositeHTML(childInfo));
            await Task.WhenAll(tasks);
        }

        #region Writing

        private static async Task WriteCompositeHTML(ICompositeDeclarationInfo info)
        {
            if (info as IObjetDeclarationInfo is IObjetDeclarationInfo objectInfo)
            {
                await WriteObjectHTML(objectInfo);
            }
            else if (info is EnumDeclarationInfo enumInfo)
            {
                await WriteEnumHTML(enumInfo);
            }
        }

        private static async Task WriteNewHTML<T>(T info, Action<T, StringBuilder> writer) where T : IDeclarationInfo
        {
            StringBuilder html = new();
            writer.Invoke(info, html);
            await WriteHtmlToFile(html, info.Name);
        }
        private static async Task WriteEnumHTML(EnumDeclarationInfo info) => await WriteNewHTML(info, GenerateHTMLContentForEnum);

        private static async Task WriteObjectHTML(IObjetDeclarationInfo info)
        {
            await WriteNewHTML(info, GenerateHTMLContentForObject);
            foreach (var member in info.Nested)
            {
                await WriteCompositeHTML(member);
            }
        }

        #endregion


        #region Content
        private static void GenerateHTMLContentForEnum(EnumDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            GenerateHtmlContentForInfos("Enum Members", info.EnumMembers, html);
            html.AppendLine("</div>");
        }

        private static void GenerateHTMLContentForObject(IObjetDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            html.AppendLine("<h1>" + info.Name + "</h1>");
            html.AppendLine("<p>" + info.FullName + "</p>");
            html.AppendLine("<p>Summary: " + info.Summary + "</p>");

            GenerateHtmlContentForInfos("Fields", info.Fields, html);
            GenerateHtmlContentForInfos("Properties", info.Properties, html);
            GenerateHtmlContentForInfos("Constructors", info.Constructors, html);
            GenerateHtmlContentForInfos("Methods", info.Methods, html);
            GenerateHtmlContentForInfos("Destructors", info.Destructors, html);
            GenerateHtmlContentForInfos("Nested", info.Nested, html);

            html.AppendLine("</div>");
        }

        private static void GenerateHtmlContentForInfos<T>(string memberName, IEnumerable<T> infos, StringBuilder html) where T : IDeclarationInfo
        {
            html.AppendLine($"<h2>{memberName}</h2>");
            foreach (var info in infos) GenerateHTMLContentForInfo(info, html);
        }

        private static void GenerateHTMLContentForInfo(IDeclarationInfo info, StringBuilder html)
        {
            if (info as ICompositeDeclarationInfo is ICompositeDeclarationInfo composite)
            {
                GenerateHMTLContentForComposite(composite, html);
            }
            else if (info as IMemberDeclarationInfo is IMemberDeclarationInfo member)
            {
                GenerateHTMLContentForMember(member, html);
            }
        }

        private static void GenerateHTMLContentForMember(IMemberDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<h1>");
            html.AppendLine($"{info.Name}");
            html.AppendLine("</h1>");
            html.AppendLine($"<p>{info.FullName}</p>");
            html.AppendLine($"<p>Summary : {info.Summary}");
        }

        private static void GenerateHMTLContentForComposite(ICompositeDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine($"<p><a href=\"{info.Name}.html\">{info.FullName}</a></p>");
        }

        #endregion

        private static async Task WriteHtmlToFile(StringBuilder html, string fileName)
        {
            using (StreamWriter writer = new StreamWriter($"{Program.OutputFolder}/{fileName}.html"))
            {
                await writer.WriteAsync(html.ToString());
            }
        }
    }
}