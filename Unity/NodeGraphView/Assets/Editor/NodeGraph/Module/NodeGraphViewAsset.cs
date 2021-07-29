using System.Collections.Generic;
using UnityEngine;
using NodeUtility;
using AiDesign;
using CUtil;
using CharacterDesign;

[CreateAssetMenu (fileName = "nodeGraph.asset", menuName = "Node Graph Asset")]
public class NodeGraphViewAsset : ScriptableObject {
    public SerializableNodeGraph nodeGraph = new SerializableNodeGraph ();
}

[System.Serializable]
public class SerializableNodeGraph {
    public List<EdgeData> edges = new List<EdgeData> ();
    public List<NodeData> nodes = new List<NodeData> ();
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
    public string annotation;
    public NODE type;
    public Rect posWH;
    public BEHAVIOR behavior;
    public BehaviorNodeData behaviorNodeData;
}
[System.Serializable]
public class BehaviorNodeData {
    public BehaviorNodeData(){
        waitTime = 0f;
        distance = 0f;
        ratio = 0f;
        conditionData = new ConditionData();
    }
    public float waitTime;
    public float distance;
    public float ratio;
    public ConditionData conditionData;
    public DETECT detect;
}
[System.Serializable]
public class ConditionData {
    public CHARASTS status;
    public CALCULATE calculate;
}