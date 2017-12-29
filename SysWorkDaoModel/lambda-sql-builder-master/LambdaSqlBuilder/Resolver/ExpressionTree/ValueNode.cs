using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaoModel.LambdaSqlBuilder.Resolver.ExpressionTree
{
    class ValueNode : Node
    {
        public object Value { get; set; }
    }
}
