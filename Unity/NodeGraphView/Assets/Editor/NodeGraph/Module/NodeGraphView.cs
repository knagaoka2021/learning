using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeGraphView : GraphView {
    private const int UI_ContentViewContainerID = 1;
    public int NodeNum { get; set; }
    public string AssetFileName { get; set; }
    public RootNode rootNode;

    public NodeGraphView () : base () {

        // 背景設定
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/GridBackGround"));
        GridBackground gridBackground = new GridBackground ();
        Insert (0, gridBackground);
        gridBackground.StretchToParentSize ();

        // ズーム機能 追加
        SetupZoom (ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        // Window内移動機能 追加 
        this.AddManipulator (new ContentDragger ());

        // ノード移動機能 追加
        this.AddManipulator (new SelectionDragger ());

        // ノード範囲選択機能 追加
        this.AddManipulator (new RectangleSelector ());

        // Editorウィンドウ取得
        var graphEditorWindow = Resources.FindObjectsOfTypeAll<EditorWindow> ()
            .FirstOrDefault (window => window.GetType ().Name == "GraphEditorWindow");

        // 生成位置を中心に設定
        var x = (graphEditorWindow.position.width / 2) - (BaseNode.nodeSize / 2);
        var y = (graphEditorWindow.position.height / 2) - (BaseNode.nodeSize / 2);

        // ノード数初期化
        NodeNum = 1;

        rootNode = new RootNode ();
        Rect r = new Rect (new Vector2 (x, y), Vector2.one * BaseNode.nodeSize);
        rootNode.SetPosition (r);
        this.AddElement (rootNode);

        var searchNodeWindow = ScriptableObject.CreateInstance<SearchNodeWindow> ();
        searchNodeWindow.Initialize (this);

        // 右クリック操作で新規Node作成機能追加
        nodeCreationRequest += context => {
            SearchWindow.Open (new SearchWindowContext (context.screenMousePosition), searchNodeWindow);
        };

    }
    // Node間の接続処理
    public override List<Port> GetCompatiblePorts (Port startAnchor, NodeAdapter nodeAdapter) {
        var compatiblePorts = new List<Port> ();
        foreach (var port in ports.ToList ()) {

            // 接続先ノードが自身でない
            // 接続先ポートが同入出力ポートでない
            // 接続先ポートが同種ポートである
            if (startAnchor.node == port.node ||
                startAnchor.direction == port.direction ||
                startAnchor.portType != port.portType) {
                continue;
            }

            compatiblePorts.Add (port);
        }
        return compatiblePorts;
    }
    // ノード種類別数の初期化
    private void UninitNodeTypeNum () {
        CompositeNode.NodeTypeNum = 0;
        ActionNode.NodeTypeNum = 0;
    }
    public void Invoke () {
        var rootEdge = rootNode.outputPort.connections.FirstOrDefault ();

        if (rootEdge == null) return;

        var currentNode = rootEdge.input.node as BaseNode;
        currentNode.Invoke ();
    }
    public void ClearNode () {
        List<VisualElement> deleteList = new List<VisualElement> ();

        // contentViewContainer内のRootNode以外の要素を全削除
        foreach (var layer in ElementAt (UI_ContentViewContainerID).Children ()) {
            bool isRootNode = false;

            foreach (var node in layer.Children ()) {
                if (node.GetType ().Name != "RootNode") {
                    ActionNode.NodeTypeNum = 0;
                    deleteList.Add (node);
                } else { // RootNodeの場合、紐づくPortを切り離す
                    isRootNode = true;
                    rootNode.outputPort.DisconnectAll ();
                }
            }
            // RootNodeを含まない階層を削除
            if (!isRootNode) {
                //deleteList.Add (layer);
                isRootNode = false;
            }
        }
        // 対象をEditorから削除
        foreach (var uie in deleteList) {
            uie.RemoveFromHierarchy ();
        }
        // ノード種類別数の初期化
        UninitNodeTypeNum ();

        // ノード数の初期化
        NodeNum = 1;
    }
    public void SaveNode () {

        var edges = this.edges.ToList ();

        if (!edges.Any ()) return;

        var nodeGraphViewAsset = ScriptableObject.CreateInstance<NodeGraphViewAsset> ();

        string path = "Assets/Editor/Resources/";
        if (!string.IsNullOrEmpty (AssetFileName)) {
            path += AssetFileName;
        } else {
            path += "default";
        }

        // エッジ情報を保存
        foreach (var edge in edges) {

            var outputNode = edge.output.node as GraphNode;
            var inputNode = edge.input.node as GraphNode;

            nodeGraphViewAsset.nodeGraph.Edges.Add (new EdgeData () {
                outputNodeId = outputNode.NodeId,
                    inputNodeId = inputNode.NodeId
            });
        }
        // ノード情報を保存
        var nodes = this.nodes.ToList ().Cast<GraphNode> ().ToList ();

        foreach (var node in nodes) {
            if (!(node.NodeType == NODE.ROOT)) {
                nodeGraphViewAsset.nodeGraph.Nodes.Add (new NodeData () {
                    nodeId = node.NodeId,
                        name = node.textElement.text,
                        type = node.NodeType,
                        posWH = node.GetPosition ()
                });
            }
        }

        AssetDatabase.CreateAsset (nodeGraphViewAsset, path + ".asset");
    }
    public void LoadNode () {
        var nodeData = Resources.Load<NodeGraphViewAsset> (AssetFileName);

        if(nodeData == null) return;
        
        // GraphView初期化
        ClearGraph ();
        // Node生成
        CreateNode (nodeData.nodeGraph.Nodes);
        // Edge生成
        CreateEdge(nodeData.nodeGraph.Edges);
    }

    private void ClearGraph () {
        this.NodeNum = 1;
        this.nodes.ToList ().ForEach (this.RemoveElement);
        this.edges.ToList ().ForEach (this.RemoveElement);
    }
    private void CreateNode (List<NodeData> nodes) {
        // RootNode生成
        this.AddElement (rootNode);

        foreach (var nodeData in nodes) {
            var type = nodeData.type;
            var args = new object[] { nodeData.nodeId };
            var node = Activator.CreateInstance (NodeType.GetNodeTypeClass (type), args) as GraphNode;
            this.NodeNum++;
            node.textElement.text = nodeData.name;

            var position = nodeData.posWH;
            node.SetPosition (position);

            this.AddElement (node);
        }

    }
    private void CreateEdge (List<EdgeData> edges) {
        var nodes = this.nodes.ToList ();
        foreach (var nodeData in nodes) {
            var graphNode = nodeData as GraphNode;
            var edgeList = edges.Where (edge => edge.outputNodeId == graphNode.NodeId).ToList ();

            foreach (var edgeData in edgeList) {
                var targetNode = nodes.Cast<GraphNode>().First(node => node.NodeId == edgeData.inputNodeId);
                var inputPort = targetNode.inputContainer.Q<Port> ();
                var outputPort = graphNode.outputContainer.Q<Port> ();
                var edge = ConectPort(inputPort,outputPort);
                this.Add(edge);
            }
        }

    }
    private Edge ConectPort(Port inputPort,Port outputPort){
        var temp = new Edge{
            input = inputPort,
            output = outputPort
        };
        temp.input.Connect(temp);
        temp.output.Connect(temp);

        return temp;
    }

}