using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface IObjetDeclarationInfo : ICompositeDeclarationInfo
{
    public IReadOnlyCollection<FieldDeclarationInfo> Fields { get; }
    public IReadOnlyCollection<PropertyDeclarationInfo> Properties { get; }
    public IReadOnlyCollection<MethodDeclarationInfo<ConstructorDeclarationSyntax>> Constructors { get; }
    public IReadOnlyCollection<MethodDeclarationInfo<MethodDeclarationSyntax>> Methods { get; }
    public IReadOnlyCollection<MethodDeclarationInfo<DestructorDeclarationSyntax>> Destructors { get; }
    public IReadOnlyCollection<ICompositeDeclarationInfo> NestedDeclaration { get; }
}
public class ObjectDeclarationInfo<T> : CompositeDeclarationInfo<T>, IObjetDeclarationInfo where T : BaseTypeDeclarationSyntax
{
    public ObjectDeclarationInfo(T syntax) : base(syntax)
    {
    }
    public IReadOnlyCollection<FieldDeclarationInfo> Fields => GetChildrens<FieldDeclarationInfo, FieldDeclarationSyntax>().ToList();
    public IReadOnlyCollection<PropertyDeclarationInfo> Properties => GetChildrens<PropertyDeclarationInfo, PropertyDeclarationSyntax>().ToList();
    public IReadOnlyCollection<MethodDeclarationInfo<ConstructorDeclarationSyntax>> Constructors => GetChildrens<MethodDeclarationInfo<ConstructorDeclarationSyntax>, ConstructorDeclarationSyntax>().ToList();
    public IReadOnlyCollection<MethodDeclarationInfo<MethodDeclarationSyntax>> Methods => GetChildrens<MethodDeclarationInfo<MethodDeclarationSyntax>, MethodDeclarationSyntax>().ToList();
    public IReadOnlyCollection<MethodDeclarationInfo<DestructorDeclarationSyntax>> Destructors => GetChildrens<MethodDeclarationInfo<DestructorDeclarationSyntax>, DestructorDeclarationSyntax>().ToList();
    public IReadOnlyCollection<ICompositeDeclarationInfo> NestedDeclaration => Childrens.OfType<ICompositeDeclarationInfo>().ToList();

    public override string MemberType => "";
}
