using System.Collections.Generic;
using System.Linq;
using NodeUtility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CompositeNode : BaseNode {
    public static int NodeTypeNum { get; set; }
    private int NodeTypeId { get; set; }

    public CompositeNode () : base (true, true, Port.Capacity.Single, Port.Capacity.Multi) {
        // ノード種別名設定
        title = "Composite";
        // ノード種別設定
        NodeType = NODE.COMPOSITE;
        // Uss読み込み
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/CompositeNode"));
        // ノード名設定
        textElement = new TextElement ();
        textElement.text = this.title + NodeTypeNum;

        // 要素をノードに追加
        mainContainer.Add (textElement);
        // ノード種別IDを設定
        NodeTypeId = NodeTypeNum;
        // ノード種別毎のノード数カウント
        NodeTypeNum++;

    }
    public CompositeNode (int id) : base (true, true, Port.Capacity.Single, Port.Capacity.Multi) {
        // ノード種別名設定
        title = "Composite";
        // ノード種別設定
        NodeType = NODE.COMPOSITE;
        // Uss読み込み
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/CompositeNode"));
        // ノード名設定
        textElement = new TextElement ();
        textElement.text = this.title + NodeTypeNum;

        // 要素をノードに追加
        mainContainer.Add (textElement);

        // ノードIDを設定
        NodeId = id;
        // ノード種別IDを設定
        NodeTypeId = NodeTypeNum;
        // ノード種別毎のノード数カウント
        NodeTypeNum++;
    }
    public override void Invoke () {
        Debug.Log (this.title + NodeTypeId);

        // ソート
        IEnumerable<Edge> ie = this.outputPort.connections.OrderBy (edge => ((BaseNode) edge.input.node).NodeId);

        foreach (var edge in ie) {
            if (edge != null) {
                var childNode = edge.input.node as BaseNode;
                childNode.Invoke ();
            }

        }
    }
}