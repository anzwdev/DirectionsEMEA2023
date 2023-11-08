using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.EditorServices.Protocol;

string alFilePath = "C:\\Conferences\\Directions EMEA 2023\\Examples\\ALProjects\\ALProject01\\TableExample.Table.al";

//load source code from file
string sourceCode = File.ReadAllText(alFilePath);

//parse source code and create SyntaxTree
SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceCode);

//get root SyntaxNode from SyntaxTree and cast it to CompilationUnitSyntax type
CompilationUnitSyntax rootNode = (CompilationUnitSyntax)syntaxTree.GetRoot();

//get first object from the compilation unit and cast it to the TableSyntax type
TableSyntax table = (TableSyntax)rootNode.Objects[0];

//create new field
FieldSyntax syncField = SyntaxFactory.Field(
    8000,
    "Synchronization State",
    SyntaxFactory.EnumDataType(
        SyntaxFactory.Identifier("enum"),
        SyntaxFactory.IdentifierName("Synchronization State")
    )
);

//create new list of table fields by adding new field to the table fields list 
FieldListSyntax newFields = table.Fields.AddFields(syncField);

//create new table based on previous one with the new fields list
TableSyntax newTable = table.WithFields(newFields);

//Format and save AL Code
VsCodeWorkspace workspace = new VsCodeWorkspace();
SyntaxNode formattedNode = Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(newTable, workspace);

File.WriteAllText(alFilePath, formattedNode.ToFullString());

