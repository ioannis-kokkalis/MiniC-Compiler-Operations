using MiniC.BaseAbstractSyntaxTree;

namespace MiniC.AbstractSyntaxTree; 

public class MiniCASTVisitor<TReturn, TParameters> : ASTBaseVisitor<TReturn, TParameters> {
	
	public override TReturn VisitChildren(IASTParentNode node, params TParameters[] parameters) {
		TReturn result = default;
		foreach (var child in node.GetChildren())
			result = Summarize(child.Accept(this, parameters), result);
		return result;
	}

	public TReturn VisitChildrenInContext(IASTParentNode node, int context, params TParameters[] parameters) {
		TReturn result = default;
		foreach (var child in (node as ASTCompositeNode).GetChildren(context))
			result = Summarize(child.Accept(this, parameters), result);
		return result;
	}
	
	/* ||| MiniC Node Operations (START) */
	
	public virtual TReturn VisitCompileUnit(CompileUnitNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitFunctionDefinition(FunctionDefinitionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitFormalArguments(FormalArgumentsNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitActualArguments(ActualArgumentsNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitReturn(ReturnNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitExpressionFunctionCall(ExpressionFunctionCallNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalNot(ExpressionLogicalNotNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalAnd(ExpressionLogicalAndNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalOr(ExpressionLogicalOrNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalEQUAL(ExpressionLogicalEQUALNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalNEQUAL(ExpressionLogicalNEQUALNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalGT(ExpressionLogicalGTNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalGTE(ExpressionLogicalGTENode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalLT(ExpressionLogicalLTNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionLogicalLTE(ExpressionLogicalLTENode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitExpressionAddition(ExpressionAdditionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionSubtraction(ExpressionSubtractionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionMultiplication(ExpressionMultiplicationNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionDivision(ExpressionDivisionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitExpressionPositive(ExpressionPositiveNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitExpressionNegative(ExpressionNegativeNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitExpressionAssignment(ExpressionAssignmentNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	public virtual TReturn VisitIDENTIFIER(IDENTIFIERNode node, params TParameters[] parameters) {
		return default;
	}
	
	public virtual TReturn VisitNUMBER(NUMBERNode node, params TParameters[] parameters) {
		return default;
	}

	public virtual TReturn VisitStatementCondition(StatementConditionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitStatementRepetition(StatementRepetitionNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitStatementCompoundEmpty(StatementCompoundEmptyNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitStatementCompoundNotEmpty(StatementCompoundNotEmptyNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}
	
	public virtual TReturn VisitBreak(BreakNode node, params TParameters[] parameters) {
		return VisitChildren(node, parameters);
	}

	/* MiniC Node Operations (END) */
	
}
