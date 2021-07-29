using NodeUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class RootNode : GraphNode {
    private const string ussPath = "Assets/Editor/Resources/USS/RootNode.uss";

    public RootNode () : base () {
        NodeId = 0;
    }
    public RootNode (int id) : base () {
        NodeId = id;
    }
    public override void Init () {
        // ノード種別名設定
        title = "Root";
        // ノード種別設定
        NodeType = NODE.ROOT;
        // Uss読み込み
        styleSheets.Add (AssetDatabase.LoadAssetAtPath<StyleSheet> (ussPath));
        // ノード名設定
        textElement = new TextElement ();
        textElement.text = "Start";
        // ノード注釈設定
        textField = new TextField ();
        textField.RegisterCallback<FocusInEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.On; });
        textField.RegisterCallback<FocusOutEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });
        textField.multiline = true;
        textField.value = NodeAnnotation;
        textField.tooltip = "ノード注釈";
        

        // 削除機能の非活性
        capabilities -= Capabilities.Deletable;

        // 出力ポート追加
        outputPort = Port.Create<Edge> (Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof (Port));
        outputPort.portName = "";
        outputContainer.Add (outputPort);

        inputContainer.RemoveFromHierarchy ();
        titleButtonContainer.Clear ();

        // 要素をノードに追加
        ContainerAdd ();
    }
    public void ContainerAdd () {
        titleButtonContainer.Add (textElement);
        mainContainer.Add (textField);
    }
    public override void Invoke(){}
    public override void Reset(){}
    protected override void OnInit(){}
    protected override void OnUpdate(){}
    protected override void OnEnd(){}

}