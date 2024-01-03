using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public class DelegateDeclarationInfo : MemberDeclarationInfo<DelegateDeclarationSyntax>
{
    public DelegateDeclarationInfo(DelegateDeclarationSyntax syntax) : base(syntax)
    {
    }

    public override string MemberType => "Delegate";
    public override string ReturnType => Syntax.ReturnType.ToString();
}

