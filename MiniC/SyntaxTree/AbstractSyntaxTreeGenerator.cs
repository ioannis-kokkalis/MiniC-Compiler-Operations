using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MiniC.AbstractSyntaxTree;
using MiniC.BaseAbstractSyntaxTree;

namespace MiniC.SyntaxTree; 

public class AbstractSyntaxTreeGenerator : MiniCBaseVisitor<int> {

	public ASTNode Root { get; private set; }

	private readonly Stack<ASTCompositeNode> parentNodeStack = new();
	private readonly Stack<int> contextStack = new();

	private Scope scope;

	private readonly Queue<Tuple<StatementCompoundNotEmptyNode, MiniCParser.CompoundStatementNotEmptyContext>> functionImlementations = new();

	/* ||| Private Methods (START) */

	private int VisitChildrenInContext(IEnumerable<IParseTree> children, ASTCompositeNode parent, int contextIndex) {
	
		parentNodeStack.Push(parent);
		contextStack.Push(contextIndex);
		
		int res = 0;
		foreach (IParseTree node in children)
			res = Visit(node);
			
		contextStack.Pop();
		parentNodeStack.Pop();

		return res;
	}
	
	private int VisitChildInContext(ParserRuleContext child, ASTCompositeNode parent, int contextIndex) {
	
		parentNodeStack.Push(parent);
		contextStack.Push(contextIndex);
		
		int res = Visit(child);
		
		contextStack.Pop();
		parentNodeStack.Pop();

		return res;
	}
	
	private int VisitTerminalInContext(ParserRuleContext tokenParent, IToken node, ASTCompositeNode parent, int contextIndex) {
	
		parentNodeStack.Push(parent);
		contextStack.Push(contextIndex);
		
		int res = Visit(GetTerminalNode(tokenParent, node));
		
		contextStack.Pop();
		parentNodeStack.Pop();
		
		return res;
	}

	private ITerminalNode GetTerminalNode(ParserRuleContext node, IToken terminal) {
		ITerminalNode child = null;
		for (int i = 0; i < node.ChildCount; i++) {
			child = node.GetChild(i) as ITerminalNode;
			if (child != null)
				if (child.Symbol == terminal)
					return child;
		}
		return child;
	}
	
	private void VisitFunctionImplementations() {
		foreach( var fi in functionImlementations ) {
			var abstractSyntaxTreeNode = fi.Item1;
			var syntaxTreeNode = fi.Item2;

			scope.EnterScope(abstractSyntaxTreeNode.sb);
			VisitChildrenInContext(syntaxTreeNode.statement(), abstractSyntaxTreeNode, (int)StatementCompoundNotEmptyNode.Context.STATEMENTS);
			scope.LeaveScope();
		}
	}

	/* Private Methods (END) */

	/* ||| Overrides [MiniCBaseVisitor<int>] (START) */

	public override int VisitCompileUnit(MiniCParser.CompileUnitContext context) {
		CompileUnitNode node = new();

		scope = new Scope();

		VisitChildrenInContext(context.functionDefinition(), node, (int) CompileUnitNode.Context.FUNCTION_DEFINITIONS);

		scope.EnterScope(node.sb);

		VisitChildrenInContext(context.statement(), node, (int) CompileUnitNode.Context.STATEMENTS); // function main()

		VisitFunctionImplementations();

		scope.LeaveScope();

		Root = node;

		return 0;
	}

	public override int VisitStatementBreak(MiniCParser.StatementBreakContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		BreakNode node = new();

		parent.AddChild(node, contextIndex);
		
		return 0;
	}

	public override int VisitStatementReturn(MiniCParser.StatementReturnContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ReturnNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) ReturnNode.Context.EXPRESSION);
		
		return 0;
	}

	public override int VisitFunctionDefinition(MiniCParser.FunctionDefinitionContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		FunctionDefinitionNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitTerminalInContext(context, context.IDENTIFIER().Symbol, node, (int) FunctionDefinitionNode.Context.IDENTIFIER);

		scope.EnterScope(node.sb);

		VisitChildInContext(context.formalArguments(), node, (int) FunctionDefinitionNode.Context.FORMAL_ARGUMENTS);
		
		VisitChildInContext(context.compoundStatement(), node, (int) FunctionDefinitionNode.Context.COMPOUND_STATEMENT);

		scope.LeaveScope();

		return 0;
	}

	public override int VisitActualArguments(MiniCParser.ActualArgumentsContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ActualArgumentsNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildrenInContext(context.expression(), node, (int) ActualArgumentsNode.Context.EXPRESSIONS);

		return 0;
	}

	public override int VisitFormalArguments(MiniCParser.FormalArgumentsContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		FormalArgumentsNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildrenInContext(context.IDENTIFIER(), node, (int) FormalArgumentsNode.Context.IDENTIFIERS);

		return 0;
	}

	public override int VisitExpressionFunctionCall(MiniCParser.ExpressionFunctionCallContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionFunctionCallNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitTerminalInContext(context, context.IDENTIFIER().Symbol, node, (int) ExpressionFunctionCallNode.Context.IDENTIFIER);
		
		VisitChildInContext(context.actualArguments(), node, (int) ExpressionFunctionCallNode.Context.ACTUAL_ARGUMENTS);
		
		return 0;
	}

	public override int VisitExpressionLogicalNot(MiniCParser.ExpressionLogicalNotContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionLogicalNotNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) ExpressionLogicalNotNode.Context.EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionLogicalAnd(MiniCParser.ExpressionLogicalAndContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionBinaryNode node = new ExpressionLogicalAndNode();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(0), node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION);
		
		VisitChildInContext(context.expression(1), node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionLogicalOr(MiniCParser.ExpressionLogicalOrContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionBinaryNode node = new ExpressionLogicalOrNode();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(0), node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION);
		
		VisitChildInContext(context.expression(1), node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionLogicalComparative(MiniCParser.ExpressionLogicalComparativeContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionBinaryNode node;

		var operatorType = context.@operator.Type;
		
		if (operatorType == MiniCLexer.EQUAL)
			node = new ExpressionLogicalEQUALNode();
		else if (operatorType == MiniCLexer.NEQUAL)
			node = new ExpressionLogicalNEQUALNode();
		else if (operatorType == MiniCLexer.GT)
			node = new ExpressionLogicalGTNode();
		else if (operatorType == MiniCLexer.GTE)
			node = new ExpressionLogicalGTENode();
		else if (operatorType == MiniCLexer.LT)
			node = new ExpressionLogicalLTNode();
		else // if (operatorType == MiniCLexer.LTE)
			node = new ExpressionLogicalLTENode();
		
		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(0), node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION);
		
		VisitChildInContext(context.expression(1), node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionMultiplicative(MiniCParser.ExpressionMultiplicativeContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionBinaryNode node;

		var operatorType = context.@operator.Type;
		
		if (operatorType == MiniCLexer.MUL)
			node = new ExpressionMultiplicationNode();
		else // if (operatorType == MiniCLexer.DIV)
			node = new ExpressionDivisionNode();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(0), node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION);
		
		VisitChildInContext(context.expression(1), node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionAdditive(MiniCParser.ExpressionAdditiveContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionBinaryNode node;

		var operatorType = context.@operator.Type;
		
		if (operatorType == MiniCLexer.PLUS)
			node = new ExpressionAdditionNode();
		else // if (operatorType == MiniCLexer.MINUS)
			node = new ExpressionSubtractionNode();
		
		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(0), node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION);
		
		VisitChildInContext(context.expression(1), node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionPositive(MiniCParser.ExpressionPositiveContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionPositiveNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) ExpressionPositiveNode.Context.EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionNegative(MiniCParser.ExpressionNegativeContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionNegativeNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) ExpressionNegativeNode.Context.EXPRESSION);
		
		return 0;
	}

	public override int VisitExpressionAssignment(MiniCParser.ExpressionAssignmentContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		ExpressionAssignmentNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitTerminalInContext(context, context.IDENTIFIER().Symbol, node, (int) ExpressionAssignmentNode.Context.IDENTIFIER);

		VisitChildInContext(context.expression(), node, (int) ExpressionAssignmentNode.Context.EXPRESSION);
		
		return 0;
	}

	public override int VisitConditionStatement(MiniCParser.ConditionStatementContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		StatementConditionNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) StatementConditionNode.Context.EXPRESSION);
		
		VisitChildInContext(context.statement(0), node, (int) StatementConditionNode.Context.STATEMENT);
		
		if( context.statement().Length == 2 )
			VisitChildInContext(context.statement(1), node, (int) StatementConditionNode.Context.STATEMENT_ELSE);

		return 0;
	}

	public override int VisitRepetitionStatement(MiniCParser.RepetitionStatementContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		StatementRepetitionNode node = new();

		parent.AddChild(node, contextIndex);
		
		VisitChildInContext(context.expression(), node, (int) StatementRepetitionNode.Context.EXPRESSION);
		
		VisitChildInContext(context.compoundStatement(), node, (int) StatementRepetitionNode.Context.COMPOUND_STATEMENT);
		
		return 0;
	}

	public override int VisitCompoundStatementEmpty(MiniCParser.CompoundStatementEmptyContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		StatementCompoundEmptyNode node = new();

		parent.AddChild(node, contextIndex);
		
		return 0;
	}

	public override int VisitCompoundStatementNotEmpty(MiniCParser.CompoundStatementNotEmptyContext context) {
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		StatementCompoundNotEmptyNode node = new();

		parent.AddChild(node, contextIndex);
		
		if( parent is FunctionDefinitionNode ) {
			node.sb = ((FunctionDefinitionNode) parent).sb;
			functionImlementations.Enqueue(new Tuple<StatementCompoundNotEmptyNode, MiniCParser.CompoundStatementNotEmptyContext>(node, context));
		}
		else {
			scope.EnterScope(node.sb);
			VisitChildrenInContext(context.statement(), node, (int)  StatementCompoundNotEmptyNode.Context.STATEMENTS);
			scope.LeaveScope();
		}
		
		return 0;
	}

	public override int VisitTerminal(ITerminalNode node) {
		ASTLeafNode nnode;

		var symbolType = node.Symbol.Type;
		var symbolText = node.Symbol.Text;
		
		var parent = parentNodeStack.Peek();
		var contextIndex = contextStack.Peek();

		if( symbolType == MiniCLexer.IDENTIFIER ) {
			if( parent is FunctionDefinitionNode )
				nnode = scope.fuctionDefinitionSymbolTable.GetNodeOnlyInCurrentSymbolTable(symbolText);
			else if( parent is FormalArgumentsNode )
				nnode = scope.currentsymbolTable.GetNodeOnlyInCurrentSymbolTable(symbolText);
			else if( parent is ExpressionFunctionCallNode )
				nnode = scope.fuctionDefinitionSymbolTable.GetNode(symbolText, false);
			else if( parent is ExpressionAssignmentNode )
				nnode = scope.currentsymbolTable.GetNode(symbolText, true);
			else
				nnode = scope.currentsymbolTable.GetNode(symbolText, false);
		}
		else if (symbolType == MiniCLexer.NUMBER)
			nnode = new NUMBERNode(symbolText);
		else return 0;

		parent.AddChild(nnode, contextIndex);

		return 0;
	}
	
	/* Overrides [MiniCBaseVisitor<int>] (END) */
	
}
