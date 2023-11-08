using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.EditorServices.Protocol;
using SyntaxModifierExample;
using System.Text;

string projectPath = args[0];

if (String.IsNullOrEmpty(projectPath))
{
    Console.WriteLine("Please specify project path");
    return;
}

FieldCaptionsSyntaxRewriter syntaxRewriter = new FieldCaptionsSyntaxRewriter();

string[] sourceFiles = Directory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
foreach (string sourceFile in sourceFiles)
{
    string sourceCode = File.ReadAllText(sourceFile);

    SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceCode);

    SyntaxNode newRootNode = syntaxRewriter.Visit(syntaxTree.GetRoot());

    //Format and save AL Code
    VsCodeWorkspace workspace = new VsCodeWorkspace();
    SyntaxNode formattedNode = Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(newRootNode, workspace);

    File.WriteAllText(sourceFile, formattedNode.ToFullString(), Encoding.UTF8);
}

Console.WriteLine("Project processing finished");
