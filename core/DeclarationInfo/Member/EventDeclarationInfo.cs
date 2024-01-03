using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace DocGen
{
    public class EventDeclarationInfo : MemberDeclarationInfo<EventDeclarationSyntax>
    {
        public EventDeclarationInfo(EventDeclarationSyntax syntax) : base(syntax)
        {
        }
        public override string MemberType => "Event";
    }
}