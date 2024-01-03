using System.Diagnostics;
using System.Dynamic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public interface ICompositeDeclarationInfo : IDeclarationInfo
    {
        public IEnumerable<IDeclarationInfo> Childrens { get; }
    }
    public abstract class CompositeDeclarationInfo<T> : DeclarationInfo<T>, ICompositeDeclarationInfo where T : SyntaxNode
    {
        public IEnumerable<IDeclarationInfo> Childrens => GetChildNodes();

        public CompositeDeclarationInfo(T synthax) : base(synthax)
        {

        }

        protected IEnumerable<IDeclarationInfo> GetChildNodes()
        {
            foreach (var node in Syntax.ChildNodes())
            {
                var nodeInfo = DeclarationInfo<SyntaxNode>.Create(node);
                if (nodeInfo is null || nodeInfo == this)
                {
                    continue;
                }
                yield return nodeInfo;
            }
        }

        protected IEnumerable<U> GetChildrens<U, V>() where U : DeclarationInfo<V> where V : SyntaxNode
        {
            foreach (var child in Childrens)
            {
                if (child is U)
                    yield return (U)child;
            }
        }
    }
}