using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FieldDeclarationInfo : MemberDeclarationInfo<FieldDeclarationSyntax>
{
    public FieldDeclarationInfo(FieldDeclarationSyntax syntax) : base(syntax)
    {
    }
}
