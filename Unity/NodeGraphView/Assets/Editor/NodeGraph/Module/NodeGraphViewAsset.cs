using System.Collections.Generic;
using UnityEngine;
using NodeUtility;

[CreateAssetMenu (fileName = "nodeGraph.asset", menuName = "Node Graph Asset")]
public class NodeGraphViewAsset : ScriptableObject {
    public SerializableNodeGraph nodeGraph = new SerializableNodeGraph ();
}

[System.Serializable]
public class SerializableNodeGraph {
    public List<EdgeData> Edges = new List<EdgeData> ();
    public List<NodeData> Nodes = new List<NodeData> ();
}

[System.Serializable]
public class EdgeData {
    public int outputNodeId;
    public int inputNodeId;
}

[System.Serializable]
public class NodeData {
    public int nodeId;
    public string name;
    public NODE type;
    public Rect posWH;
}