using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
namespace DocGen
{
    public interface IMethodDeclarationInfo : IMemberDeclarationInfo
    {
        public string? XML_Returns { get; }
        public string[] Parameters { get; }
        public string[] Exceptions { get; }
        public string[] XML_Parameters { get; }
        public string[] XML_Exceptions { get; }
    }
    public class MethodDeclarationInfo<T> : MemberDeclarationInfo<T>, IMethodDeclarationInfo where T : BaseMethodDeclarationSyntax
    {
        public MethodDeclarationInfo(T syntax) : base(syntax)
        {
        }
        public override string FullName => $"{base.FullName} ({HtmlGenerator.Join(Parameters)})";
        public override string MemberType => Syntax is ConstructorDeclarationSyntax ? "Constructor" : Syntax is DestructorDeclarationSyntax ? "Destructor" : "Method";
        public string? XML_Returns => GetXmlDocumentation("returns");
        public string[] XML_Parameters => GetXMLDocumentations("param");
        public string[] XML_Exceptions => GetXMLDocumentations("exception");
        public string[] Parameters => GetDescendants<ParameterSyntax>().Select(s => s.GetText().ToString()).ToArray();
        public string[] Exceptions => GetDescendants<ThrowStatementSyntax>().Select(s => s.GetText().ToString()).ToArray();
    }
}