using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FileDeclarationInfo : CompositeDeclarationInfo<CompilationUnitSyntax>
{
    public FileDeclarationInfo(CompilationUnitSyntax syntax) : base(syntax)
    {
    }

}
