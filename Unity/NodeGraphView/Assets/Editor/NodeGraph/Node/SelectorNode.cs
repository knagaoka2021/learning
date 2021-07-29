using System.Collections.Generic;
using System.Linq;
using NodeUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectorNode : CompositeNode {
    private const string ussPath = "Assets/Editor/Resources/USS/CompositeNode.uss";

    public SelectorNode (int id) : base (id) {

        // ノードインデックス設定
        textElement = new TextElement ();
        textElement.text = "index" + (id);

        // ノードIDを設定
        NodeId = id;
    }
    public override void Invoke () {
        OnUpdate ();
    }
    protected override void OnInit () {
        // ノードステータスを実行中に変更
        NodeState = STATE.RUNNING;

        ie = this.outputPort.connections.OrderBy (edge => ((GraphNode) edge.input.node).NodeId);
    }
    protected override void OnUpdate () {

        if (NodeState == STATE.NONE) {
            OnInit ();
        }
        if (NodeState != STATE.RUNNING) {
            OnEnd ();
            return;
        }

        // 更新処理
        var result = STATE.NONE;
        // 振る舞い実行
        foreach (var edge in ie) {
            if (edge != null) {
                var childNode = edge.input.node as GraphNode;

                // 評価がすでに終了したふるまいはスキップする
                if (childNode.NodeState == STATE.FAILURE) {
                    continue;
                }

                // 評価実施
                childNode.Invoke ();

                // 子ノードのうちどれかが成功を返した場合
                if (childNode.NodeState == STATE.SUCCSESS) {
                    result = STATE.SUCCSESS;
                    break;
                }

                if (childNode.NodeState == STATE.RUNNING ||
                    childNode.NodeState == STATE.COMPLETE) { return; }
            }
        }
        // 成功・失敗評価
        this.ResultNodeState = result;
        this.NodeState = STATE.COMPLETE;

    }
    protected override void OnEnd () {
        this.NodeState = this.ResultNodeState;
        Debug.Log (this.textElement.text + "の結果：" + this.NodeState);
    }
    public override void Init () {

        // ノード種別名設定
        title = "Selector";
        // ノード種別設定
        NodeType = NODE.SELECTOR;

        // Uss読み込み
        styleSheets.Add (AssetDatabase.LoadAssetAtPath<StyleSheet> (ussPath));

        // ノード注釈設定
        textField = new TextField ();
        textField.RegisterCallback<FocusInEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.On; });
        textField.RegisterCallback<FocusOutEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });
        textField.multiline = true;
        textField.value = NodeAnnotation;
        textField.tooltip = "ノード注釈";

        titleButtonContainer.Clear ();

        // 要素をノードに追加
        ContainerAdd ();

    }
    private void ContainerAdd () {
        // UIフィールドコールバック処理設定
        textField.RegisterCallback<ChangeEvent<string>> ((evt) => {
            NodeAnnotation = evt.newValue;
        });
        // 要素をノードに追加
        mainContainer.Add (textField);
        titleButtonContainer.Add (textElement);
    }
}