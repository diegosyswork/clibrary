using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SysWork.Data.Common.LambdaSqlBuilder.Resolver.ExpressionTree
{
    class SingleOperationNode : Node
    {
        public ExpressionType Operator { get; set; }
        public Node Child { get; set; }
    }
}
