using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeGraphView : GraphView {
    public RootNode root;
    public NodeGraphView () : base () {

        // 背景設定
        styleSheets.Add (Resources.Load<StyleSheet> ("GridBackGround"));
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
        var y = 0f;

        root = new RootNode ();
        Rect r = new Rect (new Vector2 (x, y), Vector2.one * BaseNode.nodeSize);
        root.SetPosition (r);
        AddElement (root);

        var searchNodeWindow = new SearchNodeWindow ();
        searchNodeWindow.Initialize (this);

        // 右クリック操作で新規Node作成機能追加
        nodeCreationRequest += context => {
            Debug.Log (context.screenMousePosition);
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
    public void Invoke () {
        var rootEdge = root.outputPort.connections.FirstOrDefault ();
        if (rootEdge == null) return;

        var currentNode = rootEdge.input.node as BaseNode;

        while (true) {
            currentNode.Invoke ();

            var edge = currentNode.outputPort.connections.FirstOrDefault ();
            if (edge == null) break;

            currentNode = edge.input.node as BaseNode;
        }
    }
}