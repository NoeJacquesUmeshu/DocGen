using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class InterfaceDeclarationInfo : ObjectDeclarationInfo<InterfaceDeclarationSyntax>
    {
        public InterfaceDeclarationInfo(InterfaceDeclarationSyntax syntax) : base(syntax)
        {
        }

        public override string MemberType => "Interface";
    }
}
