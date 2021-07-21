using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using NodeUtility;

public class RootNode : GraphNode {
    public Port outputPort;

    public RootNode () : base () {
        NodeId = 0;
        // ノード種別名設定
        title = "Root";
        // ノード種別設定
        NodeType = NODE.ROOT;
        // Uss読み込み
        styleSheets.Add (Resources.Load<StyleSheet> ("USS/RootNode"));

        // 削除機能の非活性
        capabilities -= Capabilities.Deletable;

        // 出力ポート追加
        outputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof (Port));
        outputPort.portName = "";
        outputContainer.Add (outputPort);
    }
}