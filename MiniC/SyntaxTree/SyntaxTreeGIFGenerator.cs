using System.Diagnostics;
using Antlr4.Runtime.Tree;

namespace MiniC.SyntaxTree;

/// <summary>
/// Produces GIF of the syntax tree given. Follows the visitor pattern. Use Visit() to trigger the generation.
/// </summary>
public class SyntaxTreeGIFGenerator : MiniCBaseVisitor<int> {

	private static readonly string outputFileName = "output/SyntaxTree";
	private static readonly string dotFileName = outputFileName + ".dot";
	private static readonly string gifFileName = outputFileName + ".gif";
	
	private int serialCounter = 0;
	private Stack<string> parentLabelStack = new();

	private StreamWriter dotFile;

	/* ||| Private (START) */

	private string GetLabel(string symbolName, string symbolValue) {
		return GetLabel(symbolName + "_" + symbolValue);
	}

	private string GetLabel(string symbolName) {
		return symbolName + "_S" + serialCounter++;
	}

	private static void RunGraphviz() {
		var start = new ProcessStartInfo();
		
		start.Arguments = "-Tgif " + dotFileName + " -o " + gifFileName;
		start.FileName = "dot";
		start.WindowStyle = ProcessWindowStyle.Hidden;
		start.CreateNoWindow = true;

		int exitCode;
		using (var proc = Process.Start(start)) { // Run the external process & wait for it to finish
			proc.WaitForExit();
			exitCode = proc.ExitCode; // Retrieve the app's exit code
		}
	}

	/* Private (END) */

	/* ||| Overrides [MiniCBaseVisitor] (START) */

	public override int VisitCompileUnit(MiniCParser.CompileUnitContext context) {
		string label = GetLabel("CompileUnit");
		
		dotFile = new StreamWriter(dotFileName);
		dotFile.WriteLine("digraph G{");

		parentLabelStack.Push(label);
		base.VisitCompileUnit(context);
		parentLabelStack.Pop();
		
		dotFile.WriteLine("}");
		dotFile.Close();
	
		RunGraphviz();
	
		return 0;
	}

	/* |||| Function (START) */

	public override int VisitFunctionDefinition(MiniCParser.FunctionDefinitionContext context) {
		string label = GetLabel("FunctionDefinition");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitFunctionDefinition(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitActualArguments(MiniCParser.ActualArgumentsContext context) {
		string label = GetLabel("ActualArguments");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitActualArguments(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitFormalArguments(MiniCParser.FormalArgumentsContext context) {
		string label = GetLabel("FormalArguments");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitFormalArguments(context);
		parentLabelStack.Pop();

		return 0;
	}

	/* Function (END) */

	/* |||| Statement (START) */

	public override int VisitStatementExpression(MiniCParser.StatementExpressionContext context) {
		string label = GetLabel("StatementExpression");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementExpression(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitStatementCondition(MiniCParser.StatementConditionContext context) {
		string label = GetLabel("StatementCondition");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementCondition(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitStatementRepetition(MiniCParser.StatementRepetitionContext context) {
		string label = GetLabel("StatementRepetition");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementRepetition(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitStatementBreak(MiniCParser.StatementBreakContext context) {
		string label = GetLabel("StatementBreak");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementBreak(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitStatementReturn(MiniCParser.StatementReturnContext context) {
		string label = GetLabel("StatementReturn");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementReturn(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitStatementCompound(MiniCParser.StatementCompoundContext context) {
		string label = GetLabel("StatementCompound");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitStatementCompound(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitConditionStatement(MiniCParser.ConditionStatementContext context) {
		string label = GetLabel("ConditionStatement");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitConditionStatement(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitRepetitionStatement(MiniCParser.RepetitionStatementContext context) {
		string label = GetLabel("RepetitionStatement");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitRepetitionStatement(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitCompoundStatementEmpty(MiniCParser.CompoundStatementEmptyContext context) {
		string label = GetLabel("CompoundStatementEmpty");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitCompoundStatementEmpty(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitCompoundStatementNotEmpty(MiniCParser.CompoundStatementNotEmptyContext context) {
		string label = GetLabel("CompoundStatementNotEmpty");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitCompoundStatementNotEmpty(context);
		parentLabelStack.Pop();

		return 0;
	}

	/* Statement (END) */
	
	/* |||| Expression (START) */

	 public override int VisitExpressionNUMBER(MiniCParser.ExpressionNUMBERContext context) {
		 string label = GetLabel("ExpressionNUMBER");

		 dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		 parentLabelStack.Push(label);
		 base.VisitExpressionNUMBER(context);
		 parentLabelStack.Pop();

		 return 0;
	 }

	 public override int VisitExpressionIDENTIFIER(MiniCParser.ExpressionIDENTIFIERContext context) {
		 string label = GetLabel("ExpressionIDENTIFIER");

		 dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		 parentLabelStack.Push(label);
		 base.VisitExpressionIDENTIFIER(context);
		 parentLabelStack.Pop();

		 return 0;
	 }

	public override int VisitExpressionFunctionCall(MiniCParser.ExpressionFunctionCallContext context) {
		string label = GetLabel("FunctionCall");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionFunctionCall(context);
		parentLabelStack.Pop();

		return 0;
	}
	
	public override int VisitExpressionParenthesis(MiniCParser.ExpressionParenthesisContext context) {
		string label = GetLabel("Parenthesis");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionParenthesis(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionLogicalNot(MiniCParser.ExpressionLogicalNotContext context) {
		string label = GetLabel("LogicalNOT");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionLogicalNot(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionLogicalAnd(MiniCParser.ExpressionLogicalAndContext context) {
		string label = GetLabel("LogicalAND");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionLogicalAnd(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionLogicalOr(MiniCParser.ExpressionLogicalOrContext context) {
		string label = GetLabel("LogicalOR");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionLogicalOr(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionLogicalComparative(MiniCParser.ExpressionLogicalComparativeContext context) {
		string nodeName = "Logical";
		if (context.@operator.Type.Equals(MiniCParser.EQUAL))
			nodeName += "EQUAL";
		else if (context.@operator.Type.Equals(MiniCParser.NEQUAL))
			nodeName += "NEQUAL";
		else if (context.@operator.Type.Equals(MiniCParser.GT))
			nodeName += "GT";
		else if (context.@operator.Type.Equals(MiniCParser.GTE))
			nodeName += "GTE";
		else if (context.@operator.Type.Equals(MiniCParser.LT))
			nodeName += "LT";
		else // if (context.@operator.Type.Equals(MiniCParser.LTE))
			nodeName += "LTE";
		string label = GetLabel(nodeName);

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionLogicalComparative(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionMultiplicative(MiniCParser.ExpressionMultiplicativeContext context) {
		string label = GetLabel(context.@operator.Type.Equals(MiniCParser.MUL) ? "Multiplication" : "Division");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionMultiplicative(context);
		parentLabelStack.Pop();

		return 0;
	}
	
	public override int VisitExpressionAdditive(MiniCParser.ExpressionAdditiveContext context) {
		string label = GetLabel(context.@operator.Type.Equals(MiniCParser.PLUS) ? "Addition" : "Subtraction");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionAdditive(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionPositive(MiniCParser.ExpressionPositiveContext context) {
		string label = GetLabel("Positive");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionPositive(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionNegative(MiniCParser.ExpressionNegativeContext context) {
		string label = GetLabel("Negative");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionNegative(context);
		parentLabelStack.Pop();

		return 0;
	}

	public override int VisitExpressionAssignment(MiniCParser.ExpressionAssignmentContext context) {
		string label = GetLabel("Assignment");

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");
		
		parentLabelStack.Push(label);
		base.VisitExpressionAssignment(context);
		parentLabelStack.Pop();

		return 0;
	}

	/* Expression (END) */

	public override int VisitTerminal(ITerminalNode node) {
		// terminal nodes are produced by the lexer, so the symbol comparison happens via lexer
		if (!node.Symbol.Type.Equals(MiniCLexer.NUMBER) &&
		    !node.Symbol.Type.Equals(MiniCLexer.IDENTIFIER))
			return 0;

		string label = GetLabel(node.Symbol.Type.Equals(MiniCLexer.NUMBER) ? "NUMBER" : "IDENTIFIER", node.GetText());

		dotFile.WriteLine("\"" + parentLabelStack.Peek() + "\"->\"" + label + "\"");

		return 0;
	}

	/* Overrides [MiniCBaseVisitor] (END) */
	
}