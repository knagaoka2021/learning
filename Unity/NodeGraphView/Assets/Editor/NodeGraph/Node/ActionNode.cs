using NodeUtility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionNode : BaseNode {
    public static int NodeTypeNum { get; set; }
    private int NodeTypeId { get; set; }

    public ActionNode () : base (true, false, Port.Capacity.Single) {
        // ノード種別名設定
        title = "Action";
        // ノード種別設定
        NodeType = NODE.ACTION;
        // Uss読み込み
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/ActionNode"));
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
    public ActionNode (int id) : base (true, false, Port.Capacity.Single) {
        // ノード種別名設定
        title = "Action";
        // ノード種別設定
        NodeType = NODE.ACTION;
        // Uss読み込み
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/ActionNode"));
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
    }
}