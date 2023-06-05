namespace MiniC.AbstractSyntaxTree;

public class SymbolTable {

	public Dictionary<string, IDENTIFIERNode> entries = new();
	
	public SymbolTable? parent { get; set; }

	public IDENTIFIERNode GetNode(string name, bool elseCreate) {
		IDENTIFIERNode node = null;
		var tmpParent = this;
		while( tmpParent != null ) {
			if( tmpParent.entries.TryGetValue(name, out node) ) break;

			tmpParent = tmpParent.parent;
		}

		if( node == null ) {
			if( elseCreate ) {
				node = new IDENTIFIERNode(name);
				entries.Add(name, node);
			}
			else {
				throw new Exception($"Variable {name} is not declared.");
			}
		}

		return node;
	}

	public IDENTIFIERNode GetNodeOnlyInCurrentSymbolTable(string name) {
		IDENTIFIERNode node;

		if( !entries.TryGetValue(name, out node) ) {
			node = new IDENTIFIERNode(name);
			entries.Add(name, node);
		}

		return node;
	}

	public Dictionary<string,IDENTIFIERNode>.Enumerator GetEnumarator()
	{
		return entries.GetEnumerator();
	}
	
}

public class Scope {

	public Scope() {
		fuctionDefinitionSymbolTable = new SymbolTable();
	}

	public SymbolTable? currentsymbolTable { get; private set; }
	public SymbolTable fuctionDefinitionSymbolTable { get; }

	public void EnterScope(SymbolTable incoming) {
		incoming.parent = currentsymbolTable;
		currentsymbolTable = incoming;
	}

	public void LeaveScope() {
		currentsymbolTable = currentsymbolTable.parent;
	}
	
}