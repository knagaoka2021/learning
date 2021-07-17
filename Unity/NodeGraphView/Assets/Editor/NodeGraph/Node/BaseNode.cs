using UnityEditor.Experimental.GraphView;

public abstract class BaseNode : GraphNode
{
    public Port inputPort;
    public Port outputPort;
    public const float nodeSize = 50.0f;

    public BaseNode () {
            
        inputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof (Port));
        inputPort.portName = "In";
        inputContainer.Add (inputPort);

        outputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof (Port));
        outputPort.portName = "Out";
        outputContainer.Add (outputPort);
    }

    public abstract void Invoke();
}
