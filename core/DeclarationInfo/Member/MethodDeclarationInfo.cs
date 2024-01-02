using Microsoft.CodeAnalysis.CSharp.Syntax;

public class MethodDeclarationInfo<T> : MemberDeclarationInfo<T> where T : BaseMethodDeclarationSyntax
{
    public MethodDeclarationInfo(T syntax) : base(syntax)
    {
    }

    public override string Type => "Method";
}
