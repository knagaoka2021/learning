using UnityEngine.UIElements;
using UnityEditor;

public class GraphEditorWindow : EditorWindow {
    [MenuItem ("Window/Open NodeGraphView")]
    public static void Open () {
        GetWindow<GraphEditorWindow> ("NodeGraphView");
    }
    void OnEnable () {
        var graphView = new NodeGraphView () {
            style = { flexGrow = 1 }
        };
        
        rootVisualElement.Add(new Button(graphView.Invoke) { text = "Invoke" });
        rootVisualElement.Add (graphView);
    }
}