using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface IMemberDeclarationInfo : IDeclarationInfo
{
    public string ReturnType { get; }
}
public class MemberDeclarationInfo<T> : DeclarationInfo<T>, IMemberDeclarationInfo where T : MemberDeclarationSyntax
{
    public MemberDeclarationInfo(T syntax) : base(syntax)
    {
    }
    public override string FullName => $"{AccessAndModifier} {MemberType}{(!string.IsNullOrEmpty(ReturnType) ? $" {MemberType}" : "")} {Name}";
    public override string MemberType => "";
    public virtual string ReturnType => "";
}