using AiDesign;
using NodeUtility;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public abstract class GraphNode : Node {
    public enum STATE {
        NONE,
        RUNNING,
        SUCCSESS,
        FAILURE,
        COMPLETE
    }
    public Port inputPort;
    public Port outputPort;
    public const float nodeSize = 140.0f;

    public GraphNode () { }

    public GraphNode (bool portIn, bool portOut, Port.Capacity capacity) {

        if (portIn) {
            inputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Input, capacity, typeof (Port));
            inputPort.portName = "In";
            inputContainer.Add (inputPort);
        }
        if (portOut) {
            outputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Output, capacity, typeof (Port));
            outputPort.portName = "Out";
            outputContainer.Add (outputPort);
        }
    }
    public GraphNode (bool portIn, bool portOut, Port.Capacity capacityIn, Port.Capacity capacityOut) {

        if (portIn) {
            inputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Input, capacityIn, typeof (Port));
            inputPort.portName = "In";
            inputContainer.Add (inputPort);
        }
        if (portOut) {
            outputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Output, capacityOut, typeof (Port));
            outputPort.portName = "Out";
            outputContainer.Add (outputPort);
        }
    }

    public int NodeId { get; set; }
    public NODE NodeType { get; set; }
    public STATE NodeState { get; set; }
    public BEHAVIOR NodeBehavior { get; set; }
    public BehaviorNodeData BehaviorNodeData { get; set; }
    public TextField textField;
    public TextElement textElement;
    public string NodeAnnotation { get; set; }
    protected STATE ResultNodeState { get; set; }

    public abstract void Invoke ();
    public abstract void Reset ();
    public abstract void Init ();
    protected abstract void OnInit ();
    protected abstract void OnUpdate ();
    protected abstract void OnEnd ();

}