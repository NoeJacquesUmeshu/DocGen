using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class EnumDeclarationInfo : CompositeDeclarationInfo<EnumDeclarationSyntax>
    {
        public EnumDeclarationInfo(EnumDeclarationSyntax syntax) : base(syntax)
        {

        }
        public IReadOnlyCollection<IDeclarationInfo> EnumMembers => GetChildrens<MemberDeclarationInfo<EnumMemberDeclarationSyntax>, EnumMemberDeclarationSyntax>().ToList();

        public override string MemberType => "Enum";
    }
}