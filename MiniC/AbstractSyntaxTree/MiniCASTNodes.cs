using MiniC.BaseAbstractSyntaxTree;

namespace MiniC.AbstractSyntaxTree; 

public enum NodeType {
	NA=-1, COMPILE_UNIT,
	
	FUNCTION_DEFINITION, FORMAL_ARGUMENTS, ACTUAL_ARGUMENTS, RETURN,
	
	EXPRESSION_FUNCTION_CALL,
	EXPRESSION_LOGICAL_NOT, EXPRESSION_LOGICAL_AND, EXPRESSION_LOGICAL_OR,
	EXPRESSION_LOGICAL_EQUAL, EXPRESSION_LOGICAL_NEQUAL, EXPRESSION_LOGICAL_GT, EXPRESSION_LOGICAL_GTE, EXPRESSION_LOGICAL_LT, EXPRESSION_LOGICAL_LTE,
	EXPRESSION_MULTIPLICATION, EXPRESSION_DIVISION,
	EXPRESSION_ADDITION, EXPRESSION_SUBTRACTION,
	EXPRESSION_POSITIVE, EXPRESSION_NEGATIVE,
	EXPRESSION_ASSIGNMENT,
	
	STATEMENT_CONDITION, STATEMENT_REPETITION, STATEMENT_COMPOUND_EMPTY, STATEMENT_COMPOUND_NOT_EMPTY,
	BREAK,
	
	IDENTIFIER, NUMBER
}

public class CompileUnitNode : ASTCompositeNode {
	
	public enum Context { STATEMENTS = 0, FUNCTION_DEFINITIONS }
        
	public SymbolTable sb = new();

	public CompileUnitNode() : base(NodeType.COMPILE_UNIT.ToString(), (int) NodeType.COMPILE_UNIT, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitCompileUnit(this, parameters);
	}
	
}

public class FunctionDefinitionNode : ASTCompositeNode {
	
	public enum Context { IDENTIFIER = 0, FORMAL_ARGUMENTS, COMPOUND_STATEMENT }
        
	public SymbolTable sb = new();

	public FunctionDefinitionNode() : base(NodeType.FUNCTION_DEFINITION.ToString(), (int) NodeType.FUNCTION_DEFINITION, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitFunctionDefinition(this, parameters);
	}
	
}

public class FormalArgumentsNode : ASTCompositeNode {
	
	public enum Context { IDENTIFIERS = 0 }
        
	public FormalArgumentsNode() : base(NodeType.FORMAL_ARGUMENTS.ToString(), (int) NodeType.FORMAL_ARGUMENTS, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitFormalArguments(this, parameters);
	}
	
}

public class ActualArgumentsNode : ASTCompositeNode {
	
	public enum Context { EXPRESSIONS = 0 }
        
	public ActualArgumentsNode() : base(NodeType.ACTUAL_ARGUMENTS.ToString(), (int) NodeType.ACTUAL_ARGUMENTS, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitActualArguments(this, parameters);
	}
	
}

public class ReturnNode : ASTCompositeNode{

	public enum Context { EXPRESSION = 0 }

	public ReturnNode() : base(NodeType.RETURN.ToString(), (int) NodeType.RETURN, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitReturn(this, parameters);
	}
	
}

public class ExpressionFunctionCallNode : ASTCompositeNode {

	public enum Context { IDENTIFIER = 0, ACTUAL_ARGUMENTS }
        
	public ExpressionFunctionCallNode() : base(NodeType.EXPRESSION_FUNCTION_CALL.ToString(), (int) NodeType.EXPRESSION_FUNCTION_CALL, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionFunctionCall(this, parameters);
	}

}

public class ExpressionLogicalNotNode : ASTCompositeNode {
	
	public enum Context { EXPRESSION = 0 }
        
	public ExpressionLogicalNotNode() : base(NodeType.EXPRESSION_LOGICAL_NOT.ToString(), (int) NodeType.EXPRESSION_LOGICAL_NOT, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalNot(this, parameters);
	}
	
}

public abstract class ExpressionBinaryNode : ASTCompositeNode {

	public enum Context { LEFT_EXPRESSION = 0, RIGHT_EXPRESSION }

	protected ExpressionBinaryNode(string name, int type) : base(name, type, Enum.GetNames(typeof(Context)).Length) { }
	
}

public class ExpressionLogicalAndNode : ExpressionBinaryNode {

	public ExpressionLogicalAndNode() : base(NodeType.EXPRESSION_LOGICAL_AND.ToString(), (int) NodeType.EXPRESSION_LOGICAL_AND) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalAnd(this, parameters);
	}
	
}

public class ExpressionLogicalOrNode : ExpressionBinaryNode {

	public ExpressionLogicalOrNode() : base(NodeType.EXPRESSION_LOGICAL_OR.ToString(), (int) NodeType.EXPRESSION_LOGICAL_OR) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalOr(this, parameters);
	}
	
}

public class ExpressionLogicalEQUALNode : ExpressionBinaryNode {

	public ExpressionLogicalEQUALNode() : base(NodeType.EXPRESSION_LOGICAL_EQUAL.ToString(), (int) NodeType.EXPRESSION_LOGICAL_EQUAL) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalEQUAL(this, parameters);
	}
	
}

public class ExpressionLogicalNEQUALNode : ExpressionBinaryNode {

	public ExpressionLogicalNEQUALNode() : base(NodeType.EXPRESSION_LOGICAL_NEQUAL.ToString(), (int) NodeType.EXPRESSION_LOGICAL_NEQUAL) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalNEQUAL(this, parameters);
	}
	
}

public class ExpressionLogicalGTNode : ExpressionBinaryNode {

	public ExpressionLogicalGTNode() : base(NodeType.EXPRESSION_LOGICAL_GT.ToString(), (int) NodeType.EXPRESSION_LOGICAL_GT) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalGT(this, parameters);
	}
	
}

public class ExpressionLogicalGTENode : ExpressionBinaryNode {

	public ExpressionLogicalGTENode() : base(NodeType.EXPRESSION_LOGICAL_GTE.ToString(), (int) NodeType.EXPRESSION_LOGICAL_GTE) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalGTE(this, parameters);
	}
	
}

public class ExpressionLogicalLTNode : ExpressionBinaryNode {

	public ExpressionLogicalLTNode() : base(NodeType.EXPRESSION_LOGICAL_LT.ToString(), (int) NodeType.EXPRESSION_LOGICAL_LT) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalLT(this, parameters);
	}
	
}

public class ExpressionLogicalLTENode : ExpressionBinaryNode {

	public ExpressionLogicalLTENode() : base(NodeType.EXPRESSION_LOGICAL_LTE.ToString(), (int) NodeType.EXPRESSION_LOGICAL_LTE) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionLogicalLTE(this, parameters);
	}
	
}

public class ExpressionAdditionNode : ExpressionBinaryNode {

	public ExpressionAdditionNode() : base(NodeType.EXPRESSION_ADDITION.ToString(), (int) NodeType.EXPRESSION_ADDITION) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionAddition(this, parameters);
	}
	
}

public class ExpressionSubtractionNode : ExpressionBinaryNode {

	public ExpressionSubtractionNode() : base(NodeType.EXPRESSION_SUBTRACTION.ToString(), (int) NodeType.EXPRESSION_SUBTRACTION) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionSubtraction(this, parameters);
	}
	
}

public class ExpressionMultiplicationNode : ExpressionBinaryNode {

	public ExpressionMultiplicationNode() : base(NodeType.EXPRESSION_MULTIPLICATION.ToString(), (int) NodeType.EXPRESSION_MULTIPLICATION) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionMultiplication(this, parameters);
	}
	
}

public class ExpressionDivisionNode : ExpressionBinaryNode {

	public ExpressionDivisionNode() : base(NodeType.EXPRESSION_DIVISION.ToString(), (int) NodeType.EXPRESSION_DIVISION) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionDivision(this, parameters);
	}
	
}

public class ExpressionPositiveNode : ASTCompositeNode {
	
	public enum Context { EXPRESSION = 0 }
        
	public ExpressionPositiveNode() : base(NodeType.EXPRESSION_POSITIVE.ToString(), (int) NodeType.EXPRESSION_POSITIVE, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionPositive(this, parameters);
	}
	
}

public class ExpressionNegativeNode : ASTCompositeNode {
	
	public enum Context { EXPRESSION = 0 }
        
	public ExpressionNegativeNode() : base(NodeType.EXPRESSION_NEGATIVE.ToString(), (int) NodeType.EXPRESSION_NEGATIVE, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionNegative(this, parameters);
	}
	
}

public class ExpressionAssignmentNode : ASTCompositeNode {

	public enum Context { IDENTIFIER = 0, EXPRESSION }

	public ExpressionAssignmentNode() : base(NodeType.EXPRESSION_ASSIGNMENT.ToString(), (int) NodeType.EXPRESSION_ASSIGNMENT, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitExpressionAssignment(this, parameters);
	}
	
}

public class IDENTIFIERNode : ASTLeafNode {

	public IDENTIFIERNode(string stringLiteral) : base(NodeType.IDENTIFIER.ToString(), (int) NodeType.IDENTIFIER, stringLiteral) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitIDENTIFIER(this, parameters);
	}
	
}

public class NUMBERNode : ASTLeafNode {

	public int value { get; }

	public NUMBERNode(string stringLiteral) : base(NodeType.NUMBER.ToString(), (int)NodeType.NUMBER, stringLiteral) {
		value = Int32.Parse(stringLiteral);
	}
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitNUMBER(this, parameters);
	}
	
}

public class StatementConditionNode : ASTCompositeNode {
	
	public enum Context { EXPRESSION = 0, STATEMENT, STATEMENT_ELSE }

	public StatementConditionNode() : base(NodeType.STATEMENT_CONDITION.ToString(), (int) NodeType.STATEMENT_CONDITION, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitStatementCondition(this, parameters);
	}
	
}

public class StatementRepetitionNode : ASTCompositeNode {
	
	public enum Context { EXPRESSION = 0, COMPOUND_STATEMENT }

	public StatementRepetitionNode() : base(NodeType.STATEMENT_REPETITION.ToString(), (int) NodeType.STATEMENT_REPETITION, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitStatementRepetition(this, parameters);
	}
	
}

public class StatementCompoundEmptyNode : ASTCompositeNode {

	public StatementCompoundEmptyNode() : base(NodeType.STATEMENT_COMPOUND_EMPTY.ToString(), (int) NodeType.STATEMENT_COMPOUND_EMPTY, 0) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitStatementCompoundEmpty(this, parameters);
	}
	
}

public class StatementCompoundNotEmptyNode : ASTCompositeNode {
	
	public enum Context { STATEMENTS = 0 }

	public SymbolTable sb = new();

	public StatementCompoundNotEmptyNode() : base(NodeType.STATEMENT_COMPOUND_NOT_EMPTY.ToString(), (int) NodeType.STATEMENT_COMPOUND_NOT_EMPTY, Enum.GetNames(typeof(Context)).Length) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitStatementCompoundNotEmpty(this, parameters);
	}
	
}

public class BreakNode : ASTCompositeNode {

	public BreakNode() : base(NodeType.BREAK.ToString(), (int) NodeType.BREAK, 0) { }
	
	public override TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters) {
		return (visitor as MiniCASTVisitor<TReturn, TParameters>).VisitBreak(this, parameters);
	}
	
}