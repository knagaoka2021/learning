using NodeUtility;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public abstract class GraphNode : Node {
    public int NodeId { get; set; }
    public NODE NodeType { get; set; }
    public TextElement textElement;
}