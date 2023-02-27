namespace MiniC.BaseAbstractSyntaxTree; 

public interface IASTVisitableNode {
	public TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters);
}

public interface IASTParentNode {
	public IEnumerable<IASTVisitableNode> GetChildren();
}

public interface IASTBaseVisitor<TReturn, TParameters> {
	public TReturn Visit(IASTVisitableNode node, params TParameters[] parameters);
	public TReturn VisitChildren(IASTParentNode node, params TParameters[] parameters);
}
