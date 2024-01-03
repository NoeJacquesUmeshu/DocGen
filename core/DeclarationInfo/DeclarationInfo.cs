using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public interface IDeclarationInfo
    {
        public Accessibility Accessibility { get; }
        public string Name { get; }
        public string FullName { get; }
        public bool IsStatic { get; }
        public string? XML_Summary { get; }
        public string? XML_Remarks { get; }
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
            this.Accessibility = GetAccessibility();
        }
        public T Syntax { get; private set; }
        public IEnumerable<SyntaxTrivia> Trivia { get; private set; }
        public Accessibility Accessibility { get; private set; }
        public string Name { get; private set; }
        public abstract string MemberType { get; }
        public virtual string FullName => $"{AccessAndModifier} {MemberType} {Name}";
        public string AccessAndModifier => $"{Accessibility}{(IsStatic ? " static" : "")}";
        public bool IsStatic => GetModifiers().Any(m => m.IsKind(SyntaxKind.StaticKeyword));
        public string? XML_Summary => GetXmlDocumentation("summary");
        public string? XML_Remarks => GetXmlDocumentation("remarks");

        protected IEnumerable<U> GetChilds<U>() where U : SyntaxNode => Syntax.ChildNodes().OfType<U>();
        protected IEnumerable<U> GetDescendants<U>() where U : SyntaxNode => Syntax.DescendantNodes().OfType<U>();
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
            if (Syntax as MemberDeclarationSyntax is MemberDeclarationSyntax member) return member.Modifiers;
            return new();
        }
        public string[] GetXMLDocumentations(string tagName)
        {
            var trivia = this.Trivia.FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
            var documentationElements = new List<string>();
            if (trivia != default)
            {
                var structure = trivia.GetStructure();

                if (structure != null)
                {
                    var xmlElements = structure.ChildNodes().Where(n => n is XmlElementSyntax element && element.StartTag.Name.ToString() == tagName);

                    if (xmlElements != null)
                    {
                        foreach (var xmlElement in xmlElements)
                        {
                            Console.WriteLine((xmlElement as XmlElementSyntax)?.GetText());
                            documentationElements.Add(string.Join(" ", xmlElement.ChildNodes().OfType<XmlTextSyntax>().SelectMany(x => x.TextTokens).Select(t => t.ToString())));
                        }
                    }
                }
            }

            return documentationElements.ToArray();
        }

        public string? GetXmlDocumentation(string tagName)
        {
            var documentationElements = GetXMLDocumentations(tagName);
            if (documentationElements.Length > 0)
            {
                return documentationElements[0];
            }
            return null;
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
}