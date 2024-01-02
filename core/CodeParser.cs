using System.Drawing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DocGen
{
    public class CodeParser
    {
        private CodeParser(ICompositeDeclarationInfo nodeInfo)
        {
            NodeInfo = nodeInfo;
        }

        public ICompositeDeclarationInfo NodeInfo { get; private set; }




        public static async Task<CodeParser> Create(string filePath)
        {
            var rootInfo = await CreateRootInfo(filePath);
            var parser = new CodeParser(rootInfo);
            return parser;
        }

        private static async Task<ICompositeDeclarationInfo> CreateRootInfo(string filePath)
        {
            var text = await File.ReadAllTextAsync(filePath);
            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetRoot();
            var declaration = (CompositeDeclarationInfo<CompilationUnitSyntax>.Create((root as CompilationUnitSyntax)!) as ICompositeDeclarationInfo)!;
            return declaration;
        }
    }
}
