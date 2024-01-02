using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EventDeclarationInfo : MemberDeclarationInfo<EventDeclarationSyntax>
{
    public EventDeclarationInfo(EventDeclarationSyntax syntax) : base(syntax)
    {
    }
    public override string Type => "Event";
}


