using UnityEditor.Experimental.GraphView;

public class RootNode : GraphNode
{
    public Port outputPort;

    public RootNode() : base()
    {
        title = "Root";

        capabilities -= Capabilities.Deletable;

        outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}