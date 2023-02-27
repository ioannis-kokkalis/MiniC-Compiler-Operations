namespace MiniC.BaseAbstractSyntaxTree; 

public abstract class ASTBaseVisitor<TReturn, TParameters> : IASTBaseVisitor<TReturn, TParameters> {

	public virtual TReturn Visit(IASTVisitableNode node, params TParameters[] parameters) {
		return node.Accept<TReturn, TParameters>(this, parameters);
	}

	public virtual TReturn VisitChildren(IASTParentNode node, params TParameters[] parameters) {
		TReturn result = default(TReturn);
		
		foreach (IASTVisitableNode child in node.GetChildren())
			result = Summarize(Visit(child, parameters), result);
			
		return result;
	}
	
	public virtual TReturn Summarize(TReturn iResult, TReturn result) {
		return iResult;
	}
}
