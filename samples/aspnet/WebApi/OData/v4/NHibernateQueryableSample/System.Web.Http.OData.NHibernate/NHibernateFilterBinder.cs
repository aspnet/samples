using System.Collections.Generic;
using System.Linq;
using System.Web.OData.Query;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;
using Microsoft.OData.Edm;
using System.Web.OData.Extensions;

namespace System.Web.OData.NHibernate
{
    public class NHibernateFilterBinder
    {
        private IEdmModel _model;
        private List<object> _positionalParmeters = new List<object>();

        private static WhereClause _emptyWhereClause = new WhereClause
        {
            Clause = String.Empty,
            PositionalParameters = new object[0]
        };

        protected NHibernateFilterBinder(IEdmModel model)
        {
            _model = model;
        }

        public static WhereClause BindFilterQueryOption(FilterQueryOption filterQuery)
        {
            if (filterQuery != null)
            {
                NHibernateFilterBinder binder = new NHibernateFilterBinder(filterQuery.Context.Model);
                return new WhereClause
                {
                    Clause = "where " + binder.BindFilter(filterQuery) + Environment.NewLine,
                    PositionalParameters = binder._positionalParmeters.ToArray()
                };
            }

            return _emptyWhereClause;
        }

        protected string BindFilter(FilterQueryOption filterQuery)
        {
            return BindFilterClause(filterQuery.FilterClause);
        }

        protected string BindFilterClause(FilterClause filterClause)
        {
            return Bind(filterClause.Expression);
        }

        protected string Bind(QueryNode node)
        {
            CollectionNode collectionNode = node as CollectionNode;
            SingleValueNode singleValueNode = node as SingleValueNode;

            if (collectionNode != null)
            {
                switch (node.Kind)
                {
                    case QueryNodeKind.CollectionNavigationNode:
                        CollectionNavigationNode navigationNode = node as CollectionNavigationNode;
                        return BindNavigationPropertyNode(navigationNode.Source, navigationNode.NavigationProperty);

                    case QueryNodeKind.CollectionPropertyAccess:
                        return BindCollectionPropertyAccessNode(node as CollectionPropertyAccessNode);
                }
            }
            else if (singleValueNode != null)
            {
                switch (node.Kind)
                {
                    case QueryNodeKind.BinaryOperator:
                        return BindBinaryOperatorNode(node as BinaryOperatorNode);

                    case QueryNodeKind.Constant:
                        return BindConstantNode(node as ConstantNode);

                    case QueryNodeKind.Convert:
                        return BindConvertNode(node as ConvertNode);

                    case QueryNodeKind.EntityRangeVariableReference:
                        return BindRangeVariable((node as EntityRangeVariableReferenceNode).RangeVariable);

                    case QueryNodeKind.NonentityRangeVariableReference:
                        return BindRangeVariable((node as NonentityRangeVariableReferenceNode).RangeVariable);

                    case QueryNodeKind.SingleValuePropertyAccess:
                        return BindPropertyAccessQueryNode(node as SingleValuePropertyAccessNode);

                    case QueryNodeKind.UnaryOperator:
                        return BindUnaryOperatorNode(node as UnaryOperatorNode);

                    case QueryNodeKind.SingleValueFunctionCall:
                        return BindSingleValueFunctionCallNode(node as SingleValueFunctionCallNode);

                    case QueryNodeKind.SingleNavigationNode:
                        SingleNavigationNode navigationNode = node as SingleNavigationNode;
                        return BindNavigationPropertyNode(navigationNode.Source, navigationNode.NavigationProperty);

                    case QueryNodeKind.Any:
                        return BindAnyNode(node as AnyNode);

                    case QueryNodeKind.All:
                        return BindAllNode(node as AllNode);
                }
            }

            throw new NotSupportedException(String.Format("Nodes of type {0} are not supported", node.Kind));
        }

        private string BindCollectionPropertyAccessNode(CollectionPropertyAccessNode collectionPropertyAccessNode)
        {
            return Bind(collectionPropertyAccessNode.Source) + "." + collectionPropertyAccessNode.Property.Name;
        }

        private string BindNavigationPropertyNode(SingleValueNode singleValueNode, IEdmNavigationProperty edmNavigationProperty)
        {
            return Bind(singleValueNode) + "." + edmNavigationProperty.Name;
        }

        private string BindAllNode(AllNode allNode)
        {
            string innerQuery = "not exists ( from " + Bind(allNode.Source) + " " + allNode.RangeVariables.First().Name;
            innerQuery += " where NOT(" + Bind(allNode.Body) + ")";
            return innerQuery + ")";
        }

        private string BindAnyNode(AnyNode anyNode)
        {
            string innerQuery = "exists ( from " + Bind(anyNode.Source) + " " + anyNode.RangeVariables.First().Name;
            if (anyNode.Body != null)
            {
                innerQuery += " where " + Bind(anyNode.Body);
            }
            return innerQuery + ")";
        }

        private string BindNavigationPropertyNode(SingleEntityNode singleEntityNode, IEdmNavigationProperty edmNavigationProperty)
        {
            return Bind(singleEntityNode) + "." + edmNavigationProperty.Name;
        }

        private string BindSingleValueFunctionCallNode(SingleValueFunctionCallNode singleValueFunctionCallNode)
        {
            var arguments = singleValueFunctionCallNode.Parameters.ToList();
            switch (singleValueFunctionCallNode.Name)
            {
                case "concat":
                    return singleValueFunctionCallNode.Name + "(" + Bind(arguments[0]) + "," + Bind(arguments[1]) + ")";

                case "length":
                case "trim":
                case "year":
                case "years":
                case "month":
                case "months":
                case "day":
                case "days":
                case "hour":
                case "hours":
                case "minute":
                case "minutes":
                case "second":
                case "seconds":
                case "round":
                case "floor":
                case "ceiling":
                    return singleValueFunctionCallNode.Name + "(" + Bind(arguments[0]) + ")";
                default:
                    throw new NotImplementedException();
            }
        }

        private string BindUnaryOperatorNode(UnaryOperatorNode unaryOperatorNode)
        {
            return ToString(unaryOperatorNode.OperatorKind) + "(" + Bind(unaryOperatorNode.Operand) + ")";
        }

        private string BindPropertyAccessQueryNode(SingleValuePropertyAccessNode singleValuePropertyAccessNode)
        {
            return Bind(singleValuePropertyAccessNode.Source) + "." + singleValuePropertyAccessNode.Property.Name;
        }

        private string BindRangeVariable(NonentityRangeVariable nonentityRangeVariable)
        {
            return nonentityRangeVariable.Name.ToString();
        }

        private string BindRangeVariable(EntityRangeVariable entityRangeVariable)
        {
            return entityRangeVariable.Name.ToString();
        }

        private string BindConvertNode(ConvertNode convertNode)
        {
            return Bind(convertNode.Source);
        }

        private string BindConstantNode(ConstantNode constantNode)
        {
            _positionalParmeters.Add(constantNode.Value);
            return "?";
        }

        private string BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode)
        {
            var left = Bind(binaryOperatorNode.Left);
            var right = Bind(binaryOperatorNode.Right);
            return "(" + left + " " + ToString(binaryOperatorNode.OperatorKind) + " " + right + ")";
        }

        private string ToString(BinaryOperatorKind binaryOpertor)
        {
            switch (binaryOpertor)
            {
                case BinaryOperatorKind.Add:
                    return "+";
                case BinaryOperatorKind.And:
                    return "AND";
                case BinaryOperatorKind.Divide:
                    return "/";
                case BinaryOperatorKind.Equal:
                    return "=";
                case BinaryOperatorKind.GreaterThan:
                    return ">";
                case BinaryOperatorKind.GreaterThanOrEqual:
                    return ">=";
                case BinaryOperatorKind.LessThan:
                    return "<";
                case BinaryOperatorKind.LessThanOrEqual:
                    return "<=";
                case BinaryOperatorKind.Modulo:
                    return "%";
                case BinaryOperatorKind.Multiply:
                    return "*";
                case BinaryOperatorKind.NotEqual:
                    return "!=";
                case BinaryOperatorKind.Or:
                    return "OR";
                case BinaryOperatorKind.Subtract:
                    return "-";
                default:
                    return null;
            }
        }

        private string ToString(UnaryOperatorKind unaryOperator)
        {
            switch (unaryOperator)
            {
                case UnaryOperatorKind.Negate:
                    return "!";
                case UnaryOperatorKind.Not:
                    return "NOT";
                default:
                    return null;
            }
        }
    }
}
