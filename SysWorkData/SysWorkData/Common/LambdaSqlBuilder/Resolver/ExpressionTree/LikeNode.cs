using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.Common.LambdaSqlBuilder.Resolver.ExpressionTree
{
    class LikeNode : Node
    {
        public LikeMethod Method { get; set; }
        public MemberNode MemberNode { get; set; }
        public string Value { get; set; }
    }
}
