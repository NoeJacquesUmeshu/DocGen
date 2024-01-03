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
}