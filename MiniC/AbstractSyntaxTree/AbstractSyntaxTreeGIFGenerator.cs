using System.Diagnostics;
using MiniC.BaseAbstractSyntaxTree;

namespace MiniC.AbstractSyntaxTree; 

public class AbstractSyntaxTreeGIFGenerator : MiniCASTVisitor<int, ASTNode> {
	
	private static readonly string outputFileName = "output/AbstractSyntaxTree";
	private static readonly string dotFileName = outputFileName + ".dot";
	private static readonly string gifFileName = outputFileName + ".gif";

	private StreamWriter dotFile;

	private int contextClusterSerial = 0;

	/* ||| Private (START) */

	private void CreateContextSubgraph(ASTCompositeNode node, int contextIndex, string contextName) {
		dotFile.WriteLine($"\tsubgraph cluster{contextClusterSerial++} {{");
		
		bool first = true;
		foreach (ASTNode child in node.GetChildren(contextIndex)) {
			if (first) {
				dotFile.Write("\t\t");
			}
			first = false;
			dotFile.Write("\"" + child.GetLabel() + "\"");
		}
		
		dotFile.WriteLine(";");
		dotFile.WriteLine($"\t\tlabel = \"{contextName}\";");
		dotFile.WriteLine("\t}");
	}

	private void printInFile(ASTNode child, ASTNode parent) {
		var childLabel = child.GetLabel();
		var parentLabel = parent.GetLabel();

		dotFile.WriteLine($"\"{parentLabel}\"->\"{childLabel}\"");
	}

	private static void RunGraphviz() {
		var start = new ProcessStartInfo();
		
		start.Arguments = "-Tgif " + dotFileName + " -o " + gifFileName;
		start.FileName = "dot";

		int exitCode;
		using (var proc = Process.Start(start)) { // Run the external process & wait for it to finish
			proc.WaitForExit();
			exitCode = proc.ExitCode; // Retrieve the app's exit code
			if (exitCode != 0)
				throw new Exception($"Graphviz process returned exit code {exitCode}");
		}
	}

	/* Private (END) */
	
	/* ||| Overrides [MiniCASTVisitor<int, ASTNode>] (START) */

	public override int VisitCompileUnit(CompileUnitNode node, params ASTNode[] parameters) {
		dotFile = new StreamWriter(dotFileName);
		dotFile.WriteLine("digraph G{");
		dotFile.WriteLine("graph [pad=\"1\", nodesep=\"1\", ranksep=\"1\"]");

		base.VisitCompileUnit(node, node);
		
		CreateContextSubgraph(node, (int) CompileUnitNode.Context.STATEMENTS, CompileUnitNode.Context.STATEMENTS.ToString());
		CreateContextSubgraph(node, (int) CompileUnitNode.Context.FUNCTION_DEFINITIONS, CompileUnitNode.Context.FUNCTION_DEFINITIONS.ToString());
		
		dotFile.WriteLine("}");
		dotFile.Close();
	
		RunGraphviz();
	
		return 0;
	}

	public override int VisitFunctionDefinition(FunctionDefinitionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitFunctionDefinition(node, node);

		CreateContextSubgraph(node, (int) FunctionDefinitionNode.Context.IDENTIFIER, FunctionDefinitionNode.Context.IDENTIFIER.ToString());
		CreateContextSubgraph(node, (int) FunctionDefinitionNode.Context.FORMAL_ARGUMENTS, FunctionDefinitionNode.Context.FORMAL_ARGUMENTS.ToString());
		CreateContextSubgraph(node, (int) FunctionDefinitionNode.Context.COMPOUND_STATEMENT, FunctionDefinitionNode.Context.COMPOUND_STATEMENT.ToString());

		return 0;
	}

	public override int VisitFormalArguments(FormalArgumentsNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitFormalArguments(node, node);

		CreateContextSubgraph(node, (int) FormalArgumentsNode.Context.IDENTIFIERS, FormalArgumentsNode.Context.IDENTIFIERS.ToString());

		return 0;
	}

	public override int VisitActualArguments(ActualArgumentsNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitActualArguments(node, node);

		CreateContextSubgraph(node, (int) ActualArgumentsNode.Context.EXPRESSIONS, ActualArgumentsNode.Context.EXPRESSIONS.ToString());

		return 0;
	}

	public override int VisitReturn(ReturnNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitReturn(node, node);
		
		CreateContextSubgraph(node, (int) ReturnNode.Context.EXPRESSION, ReturnNode.Context.EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionFunctionCall(ExpressionFunctionCallNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionFunctionCall(node, node);

		CreateContextSubgraph(node, (int) ExpressionFunctionCallNode.Context.IDENTIFIER, ExpressionFunctionCallNode.Context.IDENTIFIER.ToString());
		CreateContextSubgraph(node, (int) ExpressionFunctionCallNode.Context.ACTUAL_ARGUMENTS, ExpressionFunctionCallNode.Context.ACTUAL_ARGUMENTS.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalNot(ExpressionLogicalNotNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalNot(node, node);

		CreateContextSubgraph(node, (int) ExpressionLogicalNotNode.Context.EXPRESSION, ExpressionLogicalNotNode.Context.EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalAnd(ExpressionLogicalAndNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalAnd(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalOr(ExpressionLogicalOrNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalOr(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalEQUAL(ExpressionLogicalEQUALNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalEQUAL(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalNEQUAL(ExpressionLogicalNEQUALNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalNEQUAL(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalGT(ExpressionLogicalGTNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalGT(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalGTE(ExpressionLogicalGTENode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalGTE(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalLT(ExpressionLogicalLTNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalLT(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionLogicalLTE(ExpressionLogicalLTENode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionLogicalLTE(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionAddition(ExpressionAdditionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionAddition(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionSubtraction(ExpressionSubtractionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionSubtraction(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionMultiplication(ExpressionMultiplicationNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionMultiplication(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionDivision(ExpressionDivisionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionDivision(node, node);

		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.LEFT_EXPRESSION, ExpressionBinaryNode.Context.LEFT_EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) ExpressionBinaryNode.Context.RIGHT_EXPRESSION, ExpressionBinaryNode.Context.RIGHT_EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionPositive(ExpressionPositiveNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionPositive(node, node);

		CreateContextSubgraph(node, (int) ExpressionPositiveNode.Context.EXPRESSION, ExpressionPositiveNode.Context.EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionNegative(ExpressionNegativeNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionNegative(node, node);

		CreateContextSubgraph(node, (int) ExpressionNegativeNode.Context.EXPRESSION, ExpressionNegativeNode.Context.EXPRESSION.ToString());

		return 0;
	}

	public override int VisitExpressionAssignment(ExpressionAssignmentNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitExpressionAssignment(node, node);

		CreateContextSubgraph(node, (int) ExpressionAssignmentNode.Context.IDENTIFIER, ExpressionAssignmentNode.Context.IDENTIFIER.ToString());
		CreateContextSubgraph(node, (int) ExpressionAssignmentNode.Context.EXPRESSION, ExpressionAssignmentNode.Context.EXPRESSION.ToString());

		return 0;
	}

	public override int VisitIDENTIFIER(IDENTIFIERNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitIDENTIFIER(node, node);

		return 0;
	}

	public override int VisitNUMBER(NUMBERNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitNUMBER(node, node);

		return 0;
	}

	public override int VisitStatementCondition(StatementConditionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitStatementCondition(node, node);

		CreateContextSubgraph(node, (int) StatementConditionNode.Context.EXPRESSION, StatementConditionNode.Context.EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) StatementConditionNode.Context.STATEMENT, StatementConditionNode.Context.STATEMENT.ToString());
		
		if( node.GetChildren((int) StatementConditionNode.Context.STATEMENT_ELSE).Count() != 0 )
			CreateContextSubgraph(node, (int) StatementConditionNode.Context.STATEMENT_ELSE, StatementConditionNode.Context.STATEMENT_ELSE.ToString());

		return 0;
	}

	public override int VisitStatementRepetition(StatementRepetitionNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitStatementRepetition(node, node);

		CreateContextSubgraph(node, (int) StatementRepetitionNode.Context.EXPRESSION, StatementRepetitionNode.Context.EXPRESSION.ToString());
		CreateContextSubgraph(node, (int) StatementRepetitionNode.Context.COMPOUND_STATEMENT, StatementRepetitionNode.Context.COMPOUND_STATEMENT.ToString());

		return 0;
	}

	public override int VisitStatementCompoundEmpty(StatementCompoundEmptyNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitStatementCompoundEmpty(node, node);

		return 0;
	}

	public override int VisitStatementCompoundNotEmpty(StatementCompoundNotEmptyNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitStatementCompoundNotEmpty(node, node);

		CreateContextSubgraph(node, (int) StatementCompoundNotEmptyNode.Context.STATEMENTS, StatementCompoundNotEmptyNode.Context.STATEMENTS.ToString());

		return 0;
	}

	public override int VisitBreak(BreakNode node, params ASTNode[] parameters) {
		printInFile(node, parameters[0]);
		
		base.VisitBreak(node, node);

		return 0;
	}

	/* Overrides [MiniCASTVisitor<int, ASTNode>] (END) */
	
}