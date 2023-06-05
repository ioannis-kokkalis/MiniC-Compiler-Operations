using System.Net.Sockets;
using System.Text;
using MiniC.BaseAbstractSyntaxTree;

namespace MiniC.AbstractSyntaxTree;

public class AbstractSyntaxTreeAssemblyX86Translator: MiniCASTVisitor<string, int>
{
   private StringBuilder Header = new StringBuilder();
   private StringBuilder Data = new StringBuilder();
   private StringBuilder Code = new StringBuilder();
   private Scope scope;
   private int nestingLevel = 0;

   private static readonly string outputFileName = "output/ASTassemblyX86Traslator";
   private static readonly string dotFileName = outputFileName + ".asm";
   
   public AbstractSyntaxTreeAssemblyX86Translator(Scope scope)
   {
      this.scope = scope;
   }

   private void UpdateCode(string code)
   {
      Code.Append(new string('\t', nestingLevel));
      Code.Append(code);
   }
   
   public override string VisitCompileUnit(CompileUnitNode node, params int[] parameters)
   {
      Header.Append("INCLUDE Irvine32.inc\n");
      
      Data.Append(".DATA\n");
      var enumerator = node.sb.GetEnumarator();
      while (enumerator.MoveNext())
      {
         Data.Append(enumerator.Current.Key).Append(" DWORD ? \n");
      }

      Code.Append(".CODE\n").Append("MAIN PROC\n");
      
      nestingLevel++;
      
      VisitChildrenInContext(node,(int)CompileUnitNode.Context.STATEMENTS,parameters);
      
      nestingLevel--;

      Code.Append("EXIT \n").Append("MAIN ENDP\n\n");

      VisitChildrenInContext(node,(int)CompileUnitNode.Context.FUNCTION_DEFINITIONS,parameters);

      Code.Append("END MAIN\n");
      
      
      StreamWriter dotFile = new StreamWriter(dotFileName);
      dotFile.WriteLine(Header);
      dotFile.WriteLine(Data);
      dotFile.WriteLine(Code);
      dotFile.Close();


      return base.VisitCompileUnit(node, parameters);
   }

   public override string VisitFunctionDefinition(FunctionDefinitionNode node, params int[] parameters)
   {
      string identifier = Visit(node.GetChild((int)FunctionDefinitionNode.Context.IDENTIFIER));
      UpdateCode(identifier +" PROC\n");
      
      //TODO put in comment the names of the variables, syscall, stdcall
      VisitChildrenInContext(node,(int)FunctionDefinitionNode.Context.FORMAL_ARGUMENTS,parameters);
      
      nestingLevel++;
      
      UpdateCode("PUSH "+Register.GetEbp()+"\n");
      UpdateCode(Mov(Register.GetEbp(),Register.GetEsp()));
      
      
      int parameterSize = 4*(node.sb.entries.Count - FunctionVariables.Size());

      if (parameterSize != 0)
      {
         UpdateCode(Sub(Register.GetEsp(),parameterSize+""));
         int ebpIndex = 4;
         foreach (var element in  node.sb.entries)
         {
            string localName = "[ebp - " + ebpIndex + "]";
            if (FunctionVariables.Add(element.Key, localName, element.Value))
            {
               UpdateCode(Mov(localName, "dword ptr 0 ; " + element.Key));
               ebpIndex = ebpIndex + 4;
            }
         }  
      }
      
      VisitChildrenInContext(node,(int)FunctionDefinitionNode.Context.COMPOUND_STATEMENT,parameters);
      
      UpdateCode(Mov(Register.GetEsp(),Register.GetEbp()));
      UpdateCode("POP "+Register.GetEbp()+"\n");
      
      nestingLevel--;

      Code.Append("EXIT \n").Append(identifier+" ENDP\n\n");
      
      FunctionVariables.Clear();
      
      return "";
      return base.VisitFunctionDefinition(node, parameters);
   }

   public override string VisitFormalArguments(FormalArgumentsNode node, params int[] parameters)
   {
      int ebpIndex = 8;
      foreach(var elements in node.GetChildren((int)FormalArgumentsNode.Context.IDENTIFIERS))
      {
         string localName = "[ebp + " + ebpIndex + "]";
         FunctionVariables.Add(((IDENTIFIERNode)elements).stringLiteral,localName,(IDENTIFIERNode)elements);
         UpdateCode("; "+localName +" is "+ ((IDENTIFIERNode)elements).stringLiteral+"\n");
         ebpIndex = ebpIndex + 4;  
      }
   
      
      return "";
   }

   public override string VisitActualArguments(ActualArgumentsNode node, params int[] parameters)
   {
    
      foreach(var elements in node.GetChildren((int)ActualArgumentsNode.Context.EXPRESSIONS).Reverse())
      {
         UpdateCode(Push("dword ptr "+Visit(elements)));
      }

      return "";
      return base.VisitActualArguments(node, parameters);
   }

   public override string VisitReturn(ReturnNode node, params int[] parameters)
   {
      return base.VisitReturn(node, parameters);
   }

   public override string VisitExpressionFunctionCall(ExpressionFunctionCallNode node, params int[] parameters)
   {
      string identifier = Visit(node.GetChild((int)ExpressionFunctionCallNode.Context.IDENTIFIER));
      Visit(node.GetChild((int)ExpressionFunctionCallNode.Context.ACTUAL_ARGUMENTS));
      
      UpdateCode("CALL "+identifier+"\n");

      return "";
   }

   public override string VisitExpressionLogicalNot(ExpressionLogicalNotNode node, params int[] parameters)
   {
      
      return base.VisitExpressionLogicalNot(node, parameters);
   }

   public override string VisitExpressionLogicalAnd(ExpressionLogicalAndNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalAnd(node, parameters);
   }

   public override string VisitExpressionLogicalOr(ExpressionLogicalOrNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalOr(node, parameters);
   }

   public override string VisitExpressionLogicalEQUAL(ExpressionLogicalEQUALNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalEQUAL(node, parameters);
   }

   public override string VisitExpressionLogicalNEQUAL(ExpressionLogicalNEQUALNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalNEQUAL(node, parameters);
   }

   public override string VisitExpressionLogicalGT(ExpressionLogicalGTNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalGT(node, parameters);
   }

   public override string VisitExpressionLogicalGTE(ExpressionLogicalGTENode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalGTE(node, parameters);
   }

   public override string VisitExpressionLogicalLT(ExpressionLogicalLTNode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalLT(node, parameters);
   }

   public override string VisitExpressionLogicalLTE(ExpressionLogicalLTENode node, params int[] parameters)
   {
      return base.VisitExpressionLogicalLTE(node, parameters);
   }

   public override string VisitExpressionAddition(ExpressionAdditionNode node, params int[] parameters)
   {
      string leftchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.LEFT_EXPRESSION));
      string rightchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.RIGHT_EXPRESSION));

      string register = Register.Get();
      UpdateCode(Mov(register, "dword ptr "+leftchild));

      UpdateCode(Add(register, rightchild));
      
      return register;
      return base.VisitExpressionAddition(node, parameters);
   }

   public override string VisitExpressionSubtraction(ExpressionSubtractionNode node, params int[] parameters)
   {
      string leftchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.LEFT_EXPRESSION));
      string rightchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.RIGHT_EXPRESSION));

      string register = Register.Get();
      UpdateCode(Mov(register, "dword ptr "+leftchild));

      UpdateCode(Sub(register, rightchild));
      
      return register;
      return base.VisitExpressionSubtraction(node, parameters);
   }

   public override string VisitExpressionMultiplication(ExpressionMultiplicationNode node, params int[] parameters)
   {
      string leftchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.LEFT_EXPRESSION));
      string rightchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.RIGHT_EXPRESSION));

      string register = Register.Get();
      UpdateCode(Mov(register, "dword ptr "+leftchild));

      UpdateCode(Mul(register, rightchild));
      
      return register;
      
      return base.VisitExpressionMultiplication(node, parameters);
   }

   public override string VisitExpressionDivision(ExpressionDivisionNode node, params int[] parameters)
   {
      string leftchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.LEFT_EXPRESSION));
      string rightchild = Visit(node.GetChild((int)ExpressionBinaryNode.Context.RIGHT_EXPRESSION));
      
      UpdateCode(Mov(Register.GetEax(), "dword ptr "+leftchild));
      
      UpdateCode("cdq\n");
      
      UpdateCode(Div(rightchild));

      return Register.GetEax();
      return base.VisitExpressionDivision(node, parameters);
   }

   public override string VisitExpressionPositive(ExpressionPositiveNode node, params int[] parameters)
   {
      string value = Visit(node.GetChild((int)ExpressionPositiveNode.Context.EXPRESSION));
      
      return value;
      
   }

   public override string VisitExpressionNegative(ExpressionNegativeNode node, params int[] parameters)
   {
      string value = Visit(node.GetChild((int)ExpressionPositiveNode.Context.EXPRESSION));
      
      string register = Register.Get();

      UpdateCode(Mov(register,"dword ptr "+value));
      
      UpdateCode("NEG " + register +"\n");
      
      return register;
      
      return base.VisitExpressionNegative(node, parameters);
   }

   public override string VisitExpressionAssignment(ExpressionAssignmentNode node, params int[] parameters)
   {
      string identifier = Visit(node.GetChild((int)ExpressionAssignmentNode.Context.IDENTIFIER));

      string expressionResult = Visit(node.GetChild((int)ExpressionAssignmentNode.Context.EXPRESSION));
      
      UpdateCode(Mov(identifier,"dword ptr "+expressionResult));
      

      return "";
      return base.VisitExpressionAssignment(node, parameters);
   }

   public override string VisitIDENTIFIER(IDENTIFIERNode node, params int[] parameters)
   {
      string tmpNode = FunctionVariables.Get(node);
      
      if(tmpNode == null)
         return node.stringLiteral;
      
      return tmpNode;
   }

   public override string VisitNUMBER(NUMBERNode node, params int[] parameters)
   {
      return node.stringLiteral;
   }

   public override string VisitStatementCondition(StatementConditionNode node, params int[] parameters)
   {
      return base.VisitStatementCondition(node, parameters);
   }

   public override string VisitStatementRepetition(StatementRepetitionNode node, params int[] parameters)
   {
      return base.VisitStatementRepetition(node, parameters);
   }

   public override string VisitStatementCompoundEmpty(StatementCompoundEmptyNode node, params int[] parameters)
   {
      return "";
   }

   public override string VisitStatementCompoundNotEmpty(StatementCompoundNotEmptyNode node, params int[] parameters)
   {
      nestingLevel++;
      UpdateCode("\n");
      
      VisitChildrenInContext(node,(int)StatementCompoundNotEmptyNode.Context.STATEMENTS,parameters);
      
      UpdateCode("\n");
      nestingLevel--;
      return "";
      return base.VisitStatementCompoundNotEmpty(node, parameters);
   }

   public override string VisitBreak(BreakNode node, params int[] parameters)
   {
      return base.VisitBreak(node, parameters);
   }


   private string Mov(string left, string right)
   {
      return "MOV " + left + " , " + right+"\n";
   }
   
   private string Push(string value)
   {
      return "PUSH " + value + "\n";
   }
   
   private string Add(string left, string right)
   {
      return "ADD " + left + " , " + right+"\n";
   }
   
   private string Sub(string left, string right)
   {
      return "SUB " + left + " , " + right+"\n";
   }
   
   private string Mul(string left, string right)
   {
      return "IMUL " + left + " , " + right+"\n";
   }
   
   private string Div(string value)
   {
      return "IDIV "+ value + "\n";
   }


   // http://www.ctoassembly.com/

   
}

public static class FunctionVariables
{
   private static Dictionary<string, Tuple<string, IDENTIFIERNode>> entries = new();

   public static bool Add(string name, string localName, IDENTIFIERNode node)
   {
     return entries.TryAdd(name,new Tuple<string, IDENTIFIERNode>(localName,node)); //to not allow duplicates
   }

   public static string Get(IDENTIFIERNode node)
   {
      Tuple<string, IDENTIFIERNode> tuple;

      if (entries.TryGetValue(node.stringLiteral, out tuple))
      {
         if (tuple.Item2.Equals(node))
            return tuple.Item1;
      }
      
      return null;
   }

   public static int Size()
   {
      return entries.Count();
   }

   public static void Clear()
   {
      entries.Clear();
   }
}

public static class Register
{
   private static int counter = 0;

   public static string Get()
   {
      return "r" + counter++;
   }

   public static string GetEax()
   {
      return "eax";
   }
   
   public static string GetEdx()
   {
      return "edx";
   }
   
   public static string GetEbp()
   {
      return "ebp";
   }
   
   public static string GetEsp()
   {
      return "esp";
   }
}
