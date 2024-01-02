using Microsoft.CodeAnalysis.CSharp.Syntax;

public class MemberDeclarationInfo<T> : DeclarationInfo<T> where T : MemberDeclarationSyntax
{
    public MemberDeclarationInfo(T syntax) : base(syntax)
    {

    }

    public override string Type => type;
    protected virtual string type => "";
    
    /*  public string ReturnType
     {
         get
         {
             switch (Syntax)
             {
                 case MethodDeclarationSyntax methodDeclaration:
                     // Assuming you have a method to get the Type from the ReturnType syntax
                     return GetTypeFromSyntax(methodDeclaration.ReturnType);
                 case PropertyDeclarationSyntax propertyDeclaration:
                     // Assuming you have a method to get the Type from the Type syntax
                     return GetTypeFromSyntax(propertyDeclaration.Type);
                 case FieldDeclarationSyntax fieldDeclaration:
                     // Assuming you have a method to get the Type from the first variable's Type syntax
                     return GetTypeFromSyntax(fieldDeclaration.Declaration.Type);
                 default:
                     // Return null or throw an exception if Syntax is not a method, property, or field
                     return null;
             }
         }
     } */
}
