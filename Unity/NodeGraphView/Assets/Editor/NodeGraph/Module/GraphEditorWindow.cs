using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphEditorWindow : EditorWindow {
    public NodeGraphViewAsset m_nodeGraphViewAsset;
    public NodeGraphView m_graphView;
    private string AssetFileName { get; set; }

    [MenuItem ("Window/Open NodeGraphView")]
    public static void Open () {
        GraphEditorWindow graphEditor = CreateInstance<GraphEditorWindow> ();
        GetWindow<GraphEditorWindow> ("NodeGraphView");

        graphEditor.Initialize ();
    }
    public static void Open (string assetFileName) {
        GraphEditorWindow graphEditor = CreateInstance<GraphEditorWindow> ();

        if (Selection.activeObject is NodeGraphViewAsset nodeGraphViewAsset) {
            graphEditor.Initialize (nodeGraphViewAsset, assetFileName);
        }

        GetWindow<GraphEditorWindow> ("NodeGraphView");
    }
    void OnEnable () { }

    [OnOpenAsset ()]
    static bool OnOpenAsset (int instanceID, int line) {
        if (EditorUtility.InstanceIDToObject (instanceID) is NodeGraphViewAsset) {
            string assetFileName = EditorUtility.InstanceIDToObject (instanceID).name;
            Open (assetFileName);
            return true;
        }

        return false;
    }
    public void Initialize () {
        InitGraphView ();
    }
    public void Initialize (NodeGraphViewAsset nodeGraphViewAsset, string assetFileName) {
        // アセットファイル名設定
        AssetFileName = assetFileName;

        InitGraphView ();
    }
    private void InitGraphView () {
        m_graphView = new NodeGraphView () {
            style = { flexGrow = 1 }
        };
        // アセットファイル名設定
        m_graphView.AssetFileName = AssetFileName;

        if (!string.IsNullOrEmpty (AssetFileName)) {
            m_graphView.LoadNode ();
        }
        var root = rootVisualElement;

        // GraphViewの要素追加
        root.Add (m_graphView);

        // UXMLファイルを読み込む
        var visualTree = Resources.Load<VisualTreeAsset> ("UXML/graphEditor");
        root.styleSheets.Add (Resources.Load<StyleSheet> ("USS/graphEditor"));

        // UXMLファイルで定義した階層構造を生成
        visualTree.CloneTree (root);

        // UQuery 要素取得 コールバック割り当て
        root.Q<Button> ("Invoke").RegisterCallback<MouseUpEvent> (evt => {
            m_graphView.Invoke ();
        });
        root.Q<Button> ("Clear").RegisterCallback<MouseUpEvent> (evt => {
            m_graphView.ClearNode ();
        });
        root.Q<Button> ("Save").RegisterCallback<MouseUpEvent> (evt => {
            m_graphView.SaveNode ();
        });
        root.Q<Button> ("Load").RegisterCallback<MouseUpEvent> (evt => {
            m_graphView.LoadNode ();
        });
    }
}