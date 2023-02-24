// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace MiniC {

	internal static class Program {
		private static void Main(string[] args) {
			var lexer = new MiniCLexer(new AntlrInputStream(new StreamReader("input/toCompile.minic")));
			var parser = new MiniCParser(new CommonTokenStream(lexer));

			IParseTree syntaxTree = parser.compileUnit();
		}
	}
	
}
