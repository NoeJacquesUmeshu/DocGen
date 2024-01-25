using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class FileDeclarationInfo : CompositeDeclarationInfo<CompilationUnitSyntax>
    {
        public FileDeclarationInfo(CompilationUnitSyntax syntax) : base(syntax)
        {
        }

        public override string MemberType => "File";
    }

    public class NamespaceDeclarationInfo : CompositeDeclarationInfo<BaseNamespaceDeclarationSyntax>
    {
        public NamespaceDeclarationInfo(BaseNamespaceDeclarationSyntax syntax) : base(syntax)
        {
        }

        public override string MemberType => "Namespace Declaration";
    }
}