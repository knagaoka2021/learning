using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchNodeWindow : ScriptableObject, ISearchWindowProvider {
    private NodeGraphView graphView;

    public void Initialize (NodeGraphView graphView) {
        this.graphView = graphView;
    }

    List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree (SearchWindowContext context) {
        var entries = new List<SearchTreeEntry> ();
        entries.Add (new SearchTreeGroupEntry (new GUIContent ("Create Node")));

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
            foreach (var type in assembly.GetTypes ()) {
                if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf (typeof (GraphNode))) &&
                    type != typeof (RootNode)) {
                    entries.Add (new SearchTreeEntry (new GUIContent (type.Name)) { level = 1, userData = type });
                }
            }
        }

        return entries;
    }

    bool ISearchWindowProvider.OnSelectEntry (SearchTreeEntry searchTreeEntry, SearchWindowContext context) {

        // Editorウィンドウ取得
        var graphEditorWindow = Resources.FindObjectsOfTypeAll<EditorWindow> ()
            .FirstOrDefault (window => window.GetType ().Name == "GraphEditorWindow");

        var type = searchTreeEntry.userData as System.Type;
        var node = Activator.CreateInstance (type) as GraphNode;

        // スクリーンマウス座標からクライアント座標変換
        VisualElement ve = graphEditorWindow.rootVisualElement;
        var localPosition = ve.ChangeCoordinatesTo (ve.parent, context.screenMousePosition - graphEditorWindow.position.position);
        var clientPosition = this.graphView.contentViewContainer.WorldToLocal (localPosition);

        Rect r = new Rect (clientPosition, Vector2.one * BaseNode.nodeSize);
        node.SetPosition (r);

        graphView.AddElement (node);
        return true;
    }
}