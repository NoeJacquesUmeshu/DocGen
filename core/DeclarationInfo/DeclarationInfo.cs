using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface IDeclarationInfo
{
    public Accessibility Accessibility { get; }
    public string Name { get; }
    public string Summary { get; }
    public string FullName { get; }
}
/// <summary>
/// Represents an abstract class that provides information about a declaration.
/// </summary>
/// <typeparam name="T">The type of the syntax node representing the declaration.</typeparam>
public abstract class DeclarationInfo<T> : IDeclarationInfo where T : SyntaxNode
{
    protected DeclarationInfo(T syntax)
    {
        this.Syntax = syntax;
        this.Trivia = syntax.GetLeadingTrivia().ToList();
        this.Name = GetName();
        this.Summary = GetSummary();
        this.Accessibility = GetAccessibility();
    }
    public T Syntax { get; private set; }
    public IEnumerable<SyntaxTrivia> Trivia { get; private set; }
    public Accessibility Accessibility { get; private set; }
    public string Name { get; private set; }
    public string Summary { get; private set; }
    public abstract string Type { get; }
    public virtual string FullName => $"{Accessibility} {Type} {Name}";


    private Accessibility GetAccessibility()
    {
        var tokens = GetModifiers().Select(token => token.ValueText);
        if (tokens is null) return Accessibility.NotApplicable;
        foreach (var token in tokens)
        {
            switch (token)
            {
                case "public":
                    return Accessibility.Public;
                case "private":
                    return Accessibility.Private;
                case "protected":
                    if (tokens.Contains("internal"))
                        return Accessibility.ProtectedAndInternal;
                    else
                        return Accessibility.Protected;
                case "internal":
                    if (tokens.Contains("protected"))
                        return Accessibility.ProtectedAndInternal;
                    else
                        return Accessibility.Internal;
            }
        }

        return Accessibility.Private;
    }
    private SyntaxTokenList GetModifiers()
    {
        if (Syntax is MemberDeclarationSyntax member) return member.Modifiers;
        return default;
    }
    private string GetSummary()
    {
        var summaryTrivia = Trivia.ToList().Find(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        var xmlComment = summaryTrivia.GetStructure() as DocumentationCommentTriviaSyntax;

        if (xmlComment != null)
        {
            var summaryXml = xmlComment.ChildNodes().OfType<XmlElementSyntax>().FirstOrDefault(s => s.StartTag.Name.ToString() == "summary");
            if (summaryXml != null)
            {
                return string.Join(" ", summaryXml.Content.ToString());
            }
        }

        return string.Empty;
    }
    private string GetName()
    {
        switch (Syntax)
        {
            case ClassDeclarationSyntax @class:
                return @class.Identifier.ValueText;
            case MethodDeclarationSyntax method:
                return method.Identifier.ValueText;
            case PropertyDeclarationSyntax property:
                return property.Identifier.ValueText;
            case FieldDeclarationSyntax field:
                return field.Declaration.Variables.First().Identifier.ValueText;
            case EventDeclarationSyntax eventDeclaration:
                return eventDeclaration.Identifier.ValueText;
            case ConstructorDeclarationSyntax constructor:
                return constructor.Identifier.ValueText;
            case DestructorDeclarationSyntax destructor:
                return destructor.Identifier.ValueText;
            case BaseTypeDeclarationSyntax typeDeclarationSyntax:
                return typeDeclarationSyntax.Identifier.ValueText;
            case EnumMemberDeclarationSyntax member:
                return member.Identifier.ValueText;
            default:
                return "";
        }
    }

    public static IDeclarationInfo Create(SyntaxNode syntax)
    {
        switch (syntax)
        {
            case ClassDeclarationSyntax @class:
                return new ClassDeclarationInfo(@class);
            case MethodDeclarationSyntax method:
                return new MethodDeclarationInfo<MethodDeclarationSyntax>(method);
            case ConstructorDeclarationSyntax constructor:
                return new MethodDeclarationInfo<ConstructorDeclarationSyntax>(constructor);
            case DestructorDeclarationSyntax destructor:
                return new MethodDeclarationInfo<DestructorDeclarationSyntax>(destructor);
            case PropertyDeclarationSyntax property:
                return new PropertyDeclarationInfo(property);
            case FieldDeclarationSyntax field:
                return new FieldDeclarationInfo(field);
            case EventDeclarationSyntax eventDeclaration:
                return new EventDeclarationInfo(eventDeclaration);
            case StructDeclarationSyntax @struct:
                return new StructDeclarationInfo(@struct);
            case EnumDeclarationSyntax @enum:
                return new EnumDeclarationInfo(@enum);
            case EnumMemberDeclarationSyntax enumMember:
                return new MemberDeclarationInfo<EnumMemberDeclarationSyntax>(enumMember);
            case InterfaceDeclarationSyntax @interface:
                return new InterfaceDeclarationInfo(@interface);
            case DelegateDeclarationSyntax @delegate:
                return new DelegateDeclarationInfo(@delegate);
            case CompilationUnitSyntax compilationUnit:
                return new FileDeclarationInfo(compilationUnit);
            default: return null!;
        }
    }

}
