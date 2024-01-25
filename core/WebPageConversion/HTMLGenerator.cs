using System.Text;

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
            if (info is NamespaceDeclarationInfo namespaceDeclarationInfo)
            {
                await Write(namespaceDeclarationInfo);
            }
            else if (info as IObjetDeclarationInfo is IObjetDeclarationInfo objectInfo)
            {
                await WriteObjectHTML(objectInfo);
            }
            else if (info is EnumDeclarationInfo enumInfo)
            {
                await WriteEnumHTML(enumInfo);
            }
        }

        private static async Task WriteEnumHTML(EnumDeclarationInfo info) => await WriteHTMLContent(info, GenerateHTMLContentForEnum);
        private static async Task WriteObjectHTML(IObjetDeclarationInfo info)
        {
            await WriteHTMLContent(info, GenerateHTMLContentForObject);
            List<Task> tasks = new();
            foreach (var member in info.NestedDeclaration)
            {
                tasks.Add(WriteCompositeHTML(member));
            }
            await Task.WhenAll(tasks);
        }
        private static async Task WriteHTMLContent<T>(T info, Action<T, StringBuilder> contentWriter) where T : IDeclarationInfo
        {
            StringBuilder html = new();
            contentWriter.Invoke(info, html);
            await WriteHtmlToFile(html, info.Name);
        }

        #endregion


        #region Content
        private static void GenerateHTMLContentForEnum(EnumDeclarationInfo info, StringBuilder html)
        {
            html.AppendLine("<div>");
            html.AppendLine($"<h1>{info.FullName}</h1>");  // Add the name of the enum
            html.AppendLine("<ul>");

            foreach (var member in info.EnumMembers)  // Loop through the enum members
            {
                html.AppendLine($"<li>{member.Name}</li>");  // Add each member to the list
            }

            html.AppendLine("</ul>");
            html.AppendLine("</div>");
        }
        private static void GenerateHTMLContentForObject(IObjetDeclarationInfo info, StringBuilder html)
        {
            bool isInterface = info as InterfaceDeclarationInfo is InterfaceDeclarationInfo;
            html.AppendLine("<div>");
            html.AppendLine("<h1>" + info.Name + "</h1>");
            html.AppendLine("<p>" + info.FullName + "</p>");
            html.AppendLine("<p>Summary: " + info.XML_Summary + "</p>");
            html.AppendLine($"<p>Remarks : {string.Join(", ", info.XML_Remarks)}</p>");

            if (!isInterface)
            {
                GenerateHtmlContentForInfos("Fields", info.Fields, html);
            }
            GenerateHtmlContentForInfos("Properties", info.Properties, html);
            if (!isInterface)
            {
                GenerateHtmlContentForInfos("Constructors", info.Constructors, html);
            }
            GenerateHtmlContentForInfos("Methods", info.Methods, html);
            if (!isInterface)
            {
                GenerateHtmlContentForInfos("Destructors", info.Destructors, html);
            }
            GenerateHtmlContentForInfos("Nested Declaration", info.NestedDeclaration, html);

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
            WriteHTMLContent("", info.FullName, "h3", html);
            WriteHTMLContent("Documented Summary", info.XML_Summary, "p", html);
            if (info as IMethodDeclarationInfo is IMethodDeclarationInfo method)
            {
                WriteHTMLContent("Parameters", method.Parameters, "p", html);
                WriteHTMLContent("Exceptions", method.Exceptions, "p", html);
                WriteHTMLContent("Documented Parameters", method.XML_Parameters, "p", html);
                WriteHTMLContent("Documented Returns", method.XML_Returns, "p", html);
                WriteHTMLContent("Documented Exceptions", method.XML_Exceptions, "p", html);
            }
        }

        private static void WriteHTMLContent(string header, string? content, string markup, StringBuilder html) => html.AppendLine(GetElementHTMLContent(content, header, markup));
        private static void WriteHTMLContent(string header, string[] content, string markup, StringBuilder html) => html.AppendLine(GetArrayHTMLContent(content, header, markup));
        private static string GetElementHTMLContent(string? content, string header, string markup) => $"<{markup}>{header}{(string.IsNullOrEmpty(header) ? "" : ": ")}{content}</{markup}>";
        private static string GetArrayHTMLContent(string[] content, string header, string markup) => GetElementHTMLContent(Join(content), header, markup);
        public static string Join(string[] content) => string.Join(", ", content);

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