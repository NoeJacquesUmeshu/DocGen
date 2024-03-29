using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class StructDeclarationInfo : ObjectDeclarationInfo<StructDeclarationSyntax>
    {
        public StructDeclarationInfo(StructDeclarationSyntax syntax) : base(syntax)
        {
        }
        public override string MemberType => "Struct";
    }
}