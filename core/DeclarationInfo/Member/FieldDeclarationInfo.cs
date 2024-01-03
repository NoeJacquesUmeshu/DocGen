using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class FieldDeclarationInfo : MemberDeclarationInfo<FieldDeclarationSyntax>
    {
        public FieldDeclarationInfo(FieldDeclarationSyntax syntax) : base(syntax)
        {
        }
        public override string ReturnType => "Field";
    }
}