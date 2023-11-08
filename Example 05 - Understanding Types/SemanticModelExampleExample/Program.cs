using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.EditorServices.Protocol;
using SemanticModelExampleExample;

string alFilePath = "C:\\Conferences\\Directions EMEA 2023\\Examples\\ALProjects\\ALProject03\\Demo.Codeunit.al";

//load project using our helper class
ProjectCompilationHelper compilationHelper = new ProjectCompilationHelper(
    "C:\\Conferences\\Directions EMEA 2023\\Examples\\ALProjects\\ALProject03");

//create syntax rewriter
RemoveWithSyntaxRewriter syntaxRewriter = new RemoveWithSyntaxRewriter();

//Find file in the syntax trees loaded by the compilation helper
//(we cannot load file again as it would produce new SyntaxTree)
SyntaxTree syntaxTree = 
    compilationHelper.SyntaxTrees
    .Where(p => (p.FilePath == alFilePath))
    .First();

//get semantic model for the syntax tree
syntaxRewriter.SemanticModel = compilationHelper.Compilation.GetSemanticModel(syntaxTree);

//process syntax tree
SyntaxNode newRootNode = syntaxRewriter.Visit(syntaxTree.GetRoot());

//Format and save AL Code
VsCodeWorkspace workspace = new VsCodeWorkspace();
SyntaxNode formattedNode = Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting.Formatter.Format(newRootNode, workspace);

File.WriteAllText(alFilePath, formattedNode.ToFullString());
