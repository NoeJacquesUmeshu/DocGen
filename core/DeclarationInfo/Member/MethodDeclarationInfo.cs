using Microsoft.CodeAnalysis.CSharp.Syntax;

public class MethodDeclarationInfo<T> : DeclarationInfo<T> where T : BaseMethodDeclarationSyntax
{
    public MethodDeclarationInfo(T syntax) : base(syntax)
    {
    }
}
