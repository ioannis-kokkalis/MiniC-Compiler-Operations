namespace MiniC.BaseAbstractSyntaxTree;

public abstract class ASTNode : IASTVisitableNode {

	private static int _serialCounter = 0;

	private readonly string name;
	private readonly int type;
	private readonly int serial;
	public ASTCompositeNode? parent { get; set; }

	protected ASTNode(string name, int type) {
		this.name = name;
		this.type = type;
		this.serial = _serialCounter++;
	}
	
	/* ||| Public Methods (START) */
	
	/// <summary> Label format: <b>[name]([type])-S[serialNumber]</b>. </summary>
	public virtual string GetLabel() {
		return name + "(" + type + ")-S" + serial;
	}
	
	/* ||| Public Methods (END) */

	public abstract TReturn Accept<TReturn, TParameters>(IASTBaseVisitor<TReturn, TParameters> visitor, params TParameters[] parameters);
}

public abstract class ASTCompositeNode : ASTNode, IASTParentNode {

	private readonly List<ASTNode>[] children;
	
	protected ASTCompositeNode(string name, int type, int totalContexts) : base(name, type) {
		children = new List<ASTNode>[totalContexts];
		for (int i = 0; i < totalContexts; i++)
			children[i] = new List<ASTNode>();
	}

	/* ||| Public Methods (START) */

	public void AddChild(ASTNode child, int context) {
		if( context >= children.Length )
			throw new ArgumentOutOfRangeException("Context index out of range.");
		
		children[context].Add(child);
		child.parent = this;
	}

	public ASTNode GetChild(int context, int index = 0) {
		if( context >= children.Length )
			throw new ArgumentOutOfRangeException("Context index out of range.");
		
		if( index >= children[context].Count )
			throw new ArgumentOutOfRangeException("Node index out of range.");
	
		return children[context][index];
	}

	public IEnumerable<ASTNode> GetChildren(int context) {
		if( context >= children.Length )
			throw new ArgumentOutOfRangeException("Context index out of range.");
	
		foreach (ASTNode node in children[context])
			yield return node;
	}
	
	/* Public Methods (END) */
	
	/* ||| Implementations [IASTCompositeNode] (START) */
	
	public IEnumerable<IASTVisitableNode> GetChildren() {
		foreach (List<ASTNode> contextList in children)
			foreach (ASTNode node in contextList)
				yield return node;
	}
	
	/* Implementations [IASTCompositeNode] (END) */

}

public abstract class ASTLeafNode : ASTNode {
	
	public string stringLiteral { get; } // for example the identifier xyz has "xyz" or number 45 has "45"

	protected ASTLeafNode(string name, int type, string stringLiteral) : base(name, type) {
		this.stringLiteral = stringLiteral;
	}
	
	public override string GetLabel() {
		return stringLiteral + "-" + base.GetLabel();
	}

}
