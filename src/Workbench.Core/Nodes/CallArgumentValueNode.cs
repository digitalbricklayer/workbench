using System;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Workbench.Core.Nodes
{
    public class CallArgumentValueNode : AstNode
    {
        public AstNode Inner { get; private set; }

        public string GetValue()
        {
            if (Inner is CallArgumentNumberValueNode)
            {
                var numberValue = Inner as CallArgumentNumberValueNode;
                return Convert.ToString(numberValue.Value);
            }
            else if (Inner is CallArgumentStringValueNode)
            {
                var stringValue = Inner as CallArgumentStringValueNode;
                return stringValue.Value;
            }

            return String.Empty;
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Inner = AddChild("value", treeNode.ChildNodes[0]);
        }
    }
}
