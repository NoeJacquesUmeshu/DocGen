using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class PropertyDeclarationInfo : MemberDeclarationInfo<PropertyDeclarationSyntax>
    {
        public PropertyDeclarationInfo(PropertyDeclarationSyntax syntax) : base(syntax)
        {
        }
        public override string ReturnType => "Property";
    }
}