using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CUtil;
using NodeUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeGraphView : GraphView {
    private const string path = "Assets/Editor/Resources/NodeGraph/";
    private const string ussPath = "Assets/Editor/Resources/USS/GridBackGround.uss";
    private const int UI_ContentViewContainerID = 1;
    public string AssetFileName { get; set; }
    public int NodeNum { get; set; }
    public RootNode rootNode;
    public List<int> sortNodeIdList = new List<int> ();
    public bool isInvoke = false;
    private Vector2 position = Vector2.zero;

    public NodeGraphView () : base () {

        // 背景設定
        styleSheets.Add (AssetDatabase.LoadAssetAtPath<StyleSheet> (ussPath));
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
        var x = (graphEditorWindow.position.width / 2);
        var y = (graphEditorWindow.position.height / 2);

        // ノード数初期化
        NodeNum = 0;

        // Rootノード生成
        rootNode = new RootNode ();
        rootNode.Init ();
        NodeNum++;
        Rect r = new Rect (new Vector2 (x, y), Vector2.one * GraphNode.nodeSize);
        rootNode.SetPosition (r);
        this.AddElement (rootNode);

        var searchNodeWindow = ScriptableObject.CreateInstance<SearchNodeWindow> ();
        searchNodeWindow.Initialize (this);

        // 右クリック操作で新規Node作成機能追加
        nodeCreationRequest += context => {
            SearchWindow.Open (new SearchWindowContext (context.screenMousePosition), searchNodeWindow);
        };

        // ノードIDソート用リスト初期化
        sortNodeIdList = new List<int> ();
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
        // ノードIDソート用リストの初期化
        sortNodeIdList.Clear ();
    }
    public void Update () {
        if (isInvoke) {
            var rootEdge = rootNode.outputPort.connections.FirstOrDefault ();

            if (rootEdge == null) {
                isInvoke = false;
                return;
            };

            var mainNode = rootEdge.input.node as GraphNode;
            mainNode.Invoke ();

            if (mainNode.NodeState == GraphNode.STATE.SUCCSESS ||
                mainNode.NodeState == GraphNode.STATE.FAILURE) {
                Debug.Log ("全体の結果：" + mainNode.NodeState);
                Debug.Log ("===Rootノード終了===");

                // 全ノード 評価結果初期化
                mainNode.Reset ();

                // 実行フラグOFF
                isInvoke = false;
            }
        }
    }
    public void Invoke () {
        if (isInvoke) return;

        Debug.Log ("===Rootノード開始===");
        isInvoke = true;
    }
    public void ClearNode () {
        List<VisualElement> deleteList = new List<VisualElement> ();

        // contentViewContainer内のRootNode以外の要素を全削除
        foreach (var layer in ElementAt (UI_ContentViewContainerID).Children ()) {
            bool isRootNode = false;

            foreach (var node in layer.Children ()) {
                if (node.GetType ().Name != "RootNode") {
                    deleteList.Add (node);
                } else { // RootNodeの場合、紐づくPortを切り離す
                    isRootNode = true;
                    rootNode.outputPort.DisconnectAll ();
                    rootNode.textField.value = "";
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

        var filePath = path;

        var nodeGraphViewAsset = ScriptableObject.CreateInstance<NodeGraphViewAsset> ();

        if (AssetFileName == "default") {
            AssetFileName = "NodeGraphView_" + DateTimeUtil.GetDateTime ();
            filePath += AssetFileName;
        } else {
            filePath += AssetFileName;
        }

        // エッジ情報を保存
        foreach (var edge in edges) {

            var outputNode = edge.output.node as GraphNode;
            var inputNode = edge.input.node as GraphNode;

            nodeGraphViewAsset.nodeGraph.edges.Add (new EdgeData () {
                outputNodeId = outputNode.NodeId,
                    inputNodeId = inputNode.NodeId
            });
        }
        // ノード情報を保存
        var nodes = this.nodes.ToList ().Cast<GraphNode> ().ToList ();

        foreach (var node in nodes) {
            nodeGraphViewAsset.nodeGraph.nodes.Add (new NodeData () {
                nodeId = node.NodeId,
                    name = node.textElement.text,
                    annotation = node.textField.value,
                    type = node.NodeType,
                    posWH = node.GetPosition (),
                    behavior = node.NodeBehavior,
                    behaviorNodeData = node.BehaviorNodeData
            });
        }

        AssetDatabase.CreateAsset (nodeGraphViewAsset, filePath + ".asset");
    }
    public void LoadNode () {
        var nodeData = AssetDatabase.LoadAssetAtPath<NodeGraphViewAsset> (path + AssetFileName + ".asset");

        if (nodeData == null) return;

        // GraphView初期化
        ClearGraph ();
        // Node生成
        CreateNode (nodeData.nodeGraph.nodes);
        // Edge生成
        CreateEdge (nodeData.nodeGraph.edges);
        // ノードID採番
        NumberNodeId ();
    }

    private void ClearGraph () {
        this.NodeNum = 0;
        this.nodes.ToList ().ForEach (this.RemoveElement);
        this.edges.ToList ().ForEach (this.RemoveElement);

        // ノード種類別数の初期化
        UninitNodeTypeNum ();
    }
    private void CreateNode (List<NodeData> nodes) {

        foreach (var nodeData in nodes) {
            var type = nodeData.type;
            var args = new object[] { nodeData.nodeId };
            var node = Activator.CreateInstance (NodeType.GetNodeTypeClass (type), args) as GraphNode;
            node.NodeBehavior = nodeData.behavior;
            node.BehaviorNodeData = nodeData.behaviorNodeData;
            this.NodeNum++;

            // ノード初期化
            node.Init ();

            node.textField.value = nodeData.annotation;
            sortNodeIdList.Add (nodeData.nodeId);

            var position = nodeData.posWH;
            node.SetPosition (position);

            this.AddElement (node);

            if (type == NODE.ROOT) {
                rootNode = node as RootNode;
            }
        }

    }
    private void CreateEdge (List<EdgeData> edges) {
        var nodes = this.nodes.ToList ();
        foreach (var nodeData in nodes) {
            var graphNode = nodeData as GraphNode;
            var edgeList = edges.Where (edge => edge.outputNodeId == graphNode.NodeId).ToList ();

            foreach (var edgeData in edgeList) {
                var targetNode = nodes.Cast<GraphNode> ().First (node => node.NodeId == edgeData.inputNodeId);
                var inputPort = targetNode.inputContainer.Q<Port> ();
                var outputPort = graphNode.outputContainer.Q<Port> ();
                var edge = ConectPort (inputPort, outputPort);
                this.Add (edge);
            }
        }

    }
    private Edge ConectPort (Port inputPort, Port outputPort) {
        var temp = new Edge {
            input = inputPort,
            output = outputPort
        };
        temp.input.Connect (temp);
        temp.output.Connect (temp);

        return temp;
    }
    private void NumberNodeId () {
        // 昇順ソート
        sortNodeIdList.Sort ();

        var nodes = this.nodes.ToList ();
        int count = 0;
        foreach (var id in sortNodeIdList) {
            foreach (var nodeData in nodes) {
                var graphNode = nodeData as GraphNode;
                if (graphNode.NodeType != NODE.ROOT && graphNode.NodeId == id) {
                    graphNode.NodeId = count;
                    graphNode.textElement.text = "index" + graphNode.NodeId;
                }
            }
            count++;
        }
    }

    public override void BuildContextualMenu (ContextualMenuPopulateEvent evt) {

        // 選択対象がノード
        if ((evt.target is Node)) {
            var node = evt.target as GraphNode;
            evt.menu.AppendAction (
                "Copy",
                copy => { CopyAction (node); },
                (Func<DropdownMenuAction, DropdownMenuAction.Status>) (copy => (this.canCopySelection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled)),
                (object) null);
        }
        if (evt.target is UnityEditor.Experimental.GraphView.GraphView) {
            var pos = evt.mousePosition;
            evt.menu.AppendAction (
                "Paste",
                paste => { PasteAction (pos); },
                (Func<DropdownMenuAction, DropdownMenuAction.Status>) (paste => (this.canPaste ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled)),
                (object) null);
        }
        // 基底のメソッド実行
        base.BuildContextualMenu (evt);
    }
    private void CopyAction (GraphNode target) {
        if (target.NodeType == NODE.ROOT) return;

        // 1.コピー用シリアライズ登録 重要
        serializeGraphElements = new SerializeGraphElementsDelegate (Copy);

        // 2.コピー用コールバック呼び出し 重要
        // シリアライズ成功した場合、コピーノードを複製
        CopySelectionCallback ();

    }
    private void PasteAction (Vector2 pos) {
        position = pos;
        // 1.貼り付け用デシリアライズ登録
        unserializeAndPaste = new UnserializeAndPasteDelegate (Paste);
        // 2.コピー用コールバック呼び出し
        // デリアライズ成功した場合、canPasteのフラグがたつ
        PasteCallback ();

    }
    // コピー用ノードをstringにシリアライズ
    string Copy (IEnumerable<GraphElement> elements) {
        string data = "";
        CopyNodeData copyNodeData = new CopyNodeData ();

        foreach (GraphElement ge in elements) {
            if (ge is Node) {
                var target = ge as GraphNode;

                // コピーするノード情報を一時保存
                var nodeData = new NodeData ();
                nodeData.type = target.NodeType;
                nodeData.behavior = target.NodeBehavior;
                nodeData.behaviorNodeData = target.BehaviorNodeData;
                copyNodeData.nodeDataList.Add (nodeData);
            }
        }
        data = JsonUtility.ToJson (copyNodeData);

        return data;
    }
    void Paste (string operationName, string data) {
        if (operationName != "Paste") return;

        CopyNodeData copyNodeData = new CopyNodeData ();
        copyNodeData = JsonUtility.FromJson<CopyNodeData> (data);

        int count = 0;
        foreach (var nodeData in copyNodeData.nodeDataList) {
            // ノードインスタンス生成
            var args = new object[] { NodeNum };
            var node = Activator.CreateInstance (NodeType.GetNodeTypeClass (nodeData.type), args) as GraphNode;
            this.NodeNum++;

            node.NodeBehavior = nodeData.behavior;
            node.BehaviorNodeData = nodeData.behaviorNodeData;
            node.NodeAnnotation = nodeData.annotation;
            node.Init ();

            // ノード注釈設定
            node.textField.value = nodeData.annotation;

            // ノード生成座標設定
            var clientPosition = contentViewContainer.WorldToLocal (position);
            Vector2 ofset = Vector2.one * (count * GraphNode.nodeSize);
            Rect r = new Rect (clientPosition + ofset, Vector2.one * GraphNode.nodeSize);
            node.SetPosition (r);

            this.AddElement (node);
            count++;
        }

    }
}