using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EnumDeclarationInfo : CompositeDeclarationInfo<EnumDeclarationSyntax>
{
    public EnumDeclarationInfo(EnumDeclarationSyntax syntax) : base(syntax)
    {
        
    }
    public IReadOnlyCollection<IDeclarationInfo> EnumMembers => GetChildrens<MemberDeclarationInfo<EnumMemberDeclarationSyntax>, EnumMemberDeclarationSyntax>().ToList();

    public override string Type => "Enum";
}

