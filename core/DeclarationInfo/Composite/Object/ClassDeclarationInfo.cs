using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class ClassDeclarationInfo : ObjectDeclarationInfo<ClassDeclarationSyntax>
    {
        public ClassDeclarationInfo(ClassDeclarationSyntax syntax) : base(syntax)
        {
        }
        public override string MemberType => "Class";
    }
}