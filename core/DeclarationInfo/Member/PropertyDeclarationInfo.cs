using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PropertyDeclarationInfo : MemberDeclarationInfo<PropertyDeclarationSyntax>
{
    public PropertyDeclarationInfo(PropertyDeclarationSyntax syntax) : base(syntax)
    {
    }
    public override string Type => "Property";
}
