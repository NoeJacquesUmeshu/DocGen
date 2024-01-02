using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public class DelegateDeclarationInfo : DeclarationInfo<DelegateDeclarationSyntax>
{
    public DelegateDeclarationInfo(DelegateDeclarationSyntax syntax) : base(syntax)
    {
    }

    public override string Type => "Delegate";
}

