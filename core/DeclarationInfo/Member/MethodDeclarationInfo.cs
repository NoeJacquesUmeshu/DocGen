using Microsoft.CodeAnalysis.CSharp.Syntax;
public interface IMethodDeclarationInfo : IMemberDeclarationInfo
{
    public string? Returns { get; }
    public string[] Parameters { get; }
    public string[] Exceptions { get; }
}
public class MethodDeclarationInfo<T> : MemberDeclarationInfo<T>, IMethodDeclarationInfo where T : BaseMethodDeclarationSyntax
{
    public MethodDeclarationInfo(T syntax) : base(syntax)
    {
    }

    public override string MemberType => Syntax is ConstructorDeclarationSyntax ? "Constructor" : Syntax is DestructorDeclarationSyntax ? "Destructor" : "Method";
    public override string ReturnType => GetReturnType();
    public string? Returns => GetXmlDocumentation("returns");
    public string[] Parameters => GetXMLDocumentations("param");
    public string[] Exceptions => GetXMLDocumentations("exception");

    private string GetReturnType()
    {
        // if (Syntax is MethodDeclarationSyntax method)
        // {
        //     return method.ReturnType.ToString();
        // }
        // else if (Syntax is ConstructorDeclarationSyntax constructor)
        // {
        //     return constructor.Identifier.ToString();
        // }
        // else
        {
            return "";
        }
    }
}