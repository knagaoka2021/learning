using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class LogNode : BaseNode {
    private Port inputString;

    public LogNode () : base () {
        title = "Log";

        inputString = Port.Create<Edge> (Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof (string));
        inputContainer.Add (inputString);
    }

    public override void Invoke () {
        var edge = inputString.connections.FirstOrDefault ();
         if (edge == null) return;

        var node = edge.output.node as StringNode;

        if (node == null) return;

        Debug.Log (node.Text);
    }
}