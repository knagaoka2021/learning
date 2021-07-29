using UIDesign;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphEditorWindow : EditorWindow {
    private const string uxmlPath = "Assets/Editor/Resources/UXML/GraphEditor.uxml";
    private const string ussPath = "Assets/Editor/Resources/USS/GraphEditor.uss";
    private const string iconPath = "Assets/Editor/Resources/Image/icon.png";
    public NodeGraphViewAsset m_nodeGraphViewAsset;
    public NodeGraphView m_graphView;
    private string AssetFileName { get; set; }

    [MenuItem ("Window/Open NodeGraphView")]
    public static void Open () {
        GraphEditorWindow graphEditor = CreateInstance<GraphEditorWindow> ();
        var window = GetWindow<GraphEditorWindow> ();
        var icon = AssetDatabase.LoadAssetAtPath<Texture> (iconPath);
        window.titleContent = new GUIContent ("NodeGraphView", icon);

        graphEditor.Initialize ();
    }
    public static void Open (string assetFileName) {
        GraphEditorWindow graphEditor = CreateInstance<GraphEditorWindow> ();

        if (Selection.activeObject is NodeGraphViewAsset nodeGraphViewAsset) {
            graphEditor.Initialize (nodeGraphViewAsset, assetFileName);
        }

        var window = GetWindow<GraphEditorWindow> ();
        var icon = AssetDatabase.LoadAssetAtPath<Texture> ("Assets/Editor/Resources/Image/icon.png");
        window.titleContent = new GUIContent ("NodeGraphView：" + assetFileName, icon);
    }
    void OnEnable () {

        UiContainer.CacheClear ();
    }
    void Update () {
        if (m_graphView != null) {
            m_graphView.Update ();
        }
    }

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
        } else {
            AssetFileName = "default";
            m_graphView.AssetFileName = AssetFileName;
        }
        var root = rootVisualElement;

        // GraphViewの要素追加
        root.Add (m_graphView);

        // UXMLファイルを読み込む
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
        root.styleSheets.Add (AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath));

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