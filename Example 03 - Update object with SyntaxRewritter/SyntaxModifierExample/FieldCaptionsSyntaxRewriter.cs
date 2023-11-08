using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxModifierExample
{
    internal class FieldCaptionsSyntaxRewriter : SyntaxRewriter
    {

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            //get field name
            string name = node.GetNameStringValue();

            //create string property value
            StringPropertyValueSyntax captionPropertyValue = SyntaxFactory.StringPropertyValue(
                SyntaxFactory.StringLiteralValue(
                    SyntaxFactory.Literal(name)));
            
            //create caption property with value created above
            PropertySyntax captionProperty = SyntaxFactory.Property(PropertyKind.Caption, captionPropertyValue);

            //create new properties by adding caption property to the existing list of properties
            var newProperties = node.PropertyList.Properties.Add(captionProperty);
            var newPropertyList = node.PropertyList.WithProperties(newProperties);

            //create new field node based on the old one but with new list of properties
            node = node.WithPropertyList(newPropertyList);

            //call base method and return the value
            return base.VisitField(node);
        }

    }
}
