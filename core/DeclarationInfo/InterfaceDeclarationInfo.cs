using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InterfaceDeclarationInfo : DeclarationInfo<InterfaceDeclarationSyntax>
{
    public InterfaceDeclarationInfo(InterfaceDeclarationSyntax syntax) : base(syntax)
    {
    }

    public override string Type => "Interface";
}


