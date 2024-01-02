using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface IMemberDeclarationInfo : IDeclarationInfo
{
    public string ReturnType { get; }
}
public class MemberDeclarationInfo<T> : DeclarationInfo<T> where T : MemberDeclarationSyntax
{
    public MemberDeclarationInfo(T syntax) : base(syntax)
    {
    }

    public override string FullName => $"{ReturnType} {base.FullName}";
    public override string Type => type;
    protected virtual string type => "";
    public string ReturnType => "";
}
