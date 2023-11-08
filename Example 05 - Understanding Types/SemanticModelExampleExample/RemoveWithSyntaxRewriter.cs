using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SemanticModelExampleExample
{
    internal class RemoveWithSyntaxRewriter : SyntaxRewriter
    {

        public SemanticModel SemanticModel { get; set; }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            IOperation? operation = this.SemanticModel.GetOperation(node);
            if (operation != null)
            {
                IOperation? operationInstance = GetOperationInstance(operation);
                if ((operationInstance != null) && (operationInstance.Syntax != null))
                {
                    //part of with?
                    if ((operationInstance.Syntax.Parent != null) && (operationInstance.Syntax.Parent.Kind == SyntaxKind.WithStatement))
                    {
                        return SyntaxFactory.MemberAccessExpression(
                            (CodeExpressionSyntax)operationInstance.Syntax.WithoutTrivia(),
                            node.WithoutTrivia()).WithTriviaFrom(node);
                    }
                }
            }

            return base.VisitIdentifierName(node);
        }

        private IOperation? GetOperationInstance(IOperation operation)
        {
            switch (operation)
            {
                case IFieldAccess fieldAccess:
                    return fieldAccess.Instance;
                case IInvocationExpression invocationExpression:
                    return invocationExpression.Instance;
            }
            return null;
        }

    }
}
