// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using MiniC.AbstractSyntaxTree;
using MiniC.SyntaxTree;

namespace MiniC {

	internal static class Program {
		private static void Main(string[] args) {
			var lexer = new MiniCLexer(new AntlrInputStream(new StreamReader("input/toCompile.minic")));
			var parser = new MiniCParser(new CommonTokenStream(lexer));

			var syntaxTree = parser.compileUnit();

			new SyntaxTreeGIFGenerator().Visit(syntaxTree);

			var astGenerator = new AbstractSyntaxTreeGenerator();
			astGenerator.VisitCompileUnit(syntaxTree);
			var abstractSyntaxTree = astGenerator.Root;
			
			new AbstractSyntaxTreeGIFGenerator().Visit(abstractSyntaxTree);
			new AbstractSyntaxTreeAssemblyX86Translator(astGenerator.Scope).Visit(abstractSyntaxTree);
		}
	}
	
}
