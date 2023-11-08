using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Diagnostics;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticModelExampleExample
{
    internal class ProjectCompilationHelper
    {

        public string ProjectPath { get; }
        public string ALPackagesPath { get; }
        public List<SyntaxTree> SyntaxTrees { get; }
        public Compilation Compilation { get; }

        public ProjectCompilationHelper(string projectPath)
        {
            ProjectPath = projectPath;
            ALPackagesPath = ".alpackages";
            SyntaxTrees = new List<SyntaxTree>();

            Compilation = LoadProject();
        }

        private Compilation LoadProject()
        {
            List<Diagnostic> diagnostics = new List<Diagnostic>();

            //load project manifest
            string appJsonPath = Path.Combine(ProjectPath, "app.json");
            ProjectManifest manifest = ProjectManifest.ReadFromString(appJsonPath, File.ReadAllText(appJsonPath), diagnostics);

            ParseOptions parseOptions = new ParseOptions(
                manifest.AppManifest.Runtime, 
                manifest.AppManifest.PreprocessorSymbols);

            //load all syntax trees
            this.LoadSyntaxTrees(ProjectPath, parseOptions);

            //initialize compilation options
            var compilationOptions = new CompilationOptions(
                target: manifest.AppManifest.Target,
                compilerFeatures: manifest.AppManifest.CompilerFeatures);

            //create compilation
            Compilation compilation = Compilation.Create("MyCompilation", 
                manifest.AppManifest.AppPublisher,
                manifest.AppManifest.AppVersion, 
                manifest.AppManifest.AppId,
                null, SyntaxTrees,
                compilationOptions);

            //create references loader
            List<string> packageCachePathList = new List<string>
            {
                Path.Combine(ProjectPath, ALPackagesPath)
            };

            LocalCacheSymbolReferenceLoader referenceLoader = 
                new LocalCacheSymbolReferenceLoader(packageCachePathList);
            
            //create new compilation with references loader
            compilation = compilation
                .WithReferenceLoader(referenceLoader)
                .WithReferences(manifest.GetAllReferences());

            return compilation;
        }

        protected void LoadSyntaxTrees(string projectPath, ParseOptions parseOptions)
        {
            SyntaxTrees.Clear();

            string[] filePathsList = Directory.GetFiles(projectPath, "*.al", SearchOption.AllDirectories);
            foreach (string filePath in filePathsList)
            {
                string sourceCode = File.ReadAllText(filePath);
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceCode, filePath, Encoding.UTF8, parseOptions);
                SyntaxTrees.Add(syntaxTree);
            }
        }

    }
}
