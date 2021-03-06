using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SysWork.Data.Common.LambdaSqlBuilder.Resolver.ExpressionTree
{
    class OperationNode : Node
    {
        public ExpressionType Operator { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
