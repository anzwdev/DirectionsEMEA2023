using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.EditorServices.Protocol;

//create list of fields
List<FieldSyntax> fieldsList = new List<FieldSyntax>();

//create table field syntax node and add to the list
fieldsList.Add(
    SyntaxFactory.Field(
        1, "Entry No.",
        SyntaxFactory.SimpleNamedDataType(
            SyntaxFactory.Identifier("Integer")
        )
    )
);

//create table field syntax node and add to the list
fieldsList.Add(
    SyntaxFactory.Field(
        2, 
        "Name",
        SyntaxFactory.LengthDataType(
            SyntaxFactory.Identifier("Text"),
            SyntaxFactory.Literal(250)
        )
    )
);

//create table syntax node
var table = SyntaxFactory.Table(50000, "Directions Table");

//create new table syntax node with fields
table = table.WithFields(
    SyntaxFactory.FieldList(
        SyntaxFactory.List(fieldsList)
    )
);

//format and display table syntax node
var workspace = new VsCodeWorkspace();
var formattedNode = Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(table, workspace);

Console.WriteLine(formattedNode.ToFullString());
Console.WriteLine();
