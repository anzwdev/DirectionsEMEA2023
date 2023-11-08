using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxModifierExample
{
    internal class IdentifierCaseSyntaxRewriter : SyntaxRewriter
    {

        public SemanticModel SemanticModel { get; set; }


        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            string name = node.GetNameStringValue();

            SymbolInfo symbolInfo = this.SemanticModel.GetSymbolInfo(node);

            if (symbolInfo.Symbol != null)
            {
                string newName = symbolInfo.Symbol.Name;
                if (newName != name)
                {
                    SyntaxToken identifier = node.Identifier;
                    SyntaxToken newIdentifier = SyntaxFactory.Identifier(newName)
                        .WithLeadingTrivia(identifier.LeadingTrivia)
                        .WithTrailingTrivia(identifier.TrailingTrivia);

                    node = node.WithIdentifier(newIdentifier);
                }
            }

            return base.VisitIdentifierName(node);
        }


    }
}
