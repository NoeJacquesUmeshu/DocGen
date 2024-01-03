using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class DelegateDeclarationInfo : MemberDeclarationInfo<DelegateDeclarationSyntax>
    {
        public DelegateDeclarationInfo(DelegateDeclarationSyntax syntax) : base(syntax)
        {
        }

        public override string MemberType => "Delegate";
        public override string ReturnType => Syntax.ReturnType.ToString();
    }
}