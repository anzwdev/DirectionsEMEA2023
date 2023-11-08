using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.EditorServices.Protocol;
using SyntaxModifierExample;

string alFilePath = "C:\\Conferences\\Directions EMEA 2023\\Examples\\ALProjects\\ALProject02\\TableExample.Table.al";

//load source code
string sourceCode = File.ReadAllText(alFilePath);

//parse syntax tree
SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceCode);

//create syntax rewriter instance
FieldCaptionsSyntaxRewriter syntaxRewriter = new FieldCaptionsSyntaxRewriter();

//use syntax rewriter to process root node of the syntax tree
SyntaxNode newRootNode = syntaxRewriter.Visit(syntaxTree.GetRoot());

//Format and save AL Code
VsCodeWorkspace workspace = new VsCodeWorkspace();
SyntaxNode formattedNode = Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(newRootNode, workspace);

File.WriteAllText(alFilePath, formattedNode.ToString());
