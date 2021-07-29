using System;
using System.Collections;
using AiDesign;
using CharacterDesign;
using CUtil;
using NodeUtility;
using UIDesign;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionNode : GraphNode {
    private const string ussPath = "Assets/Editor/Resources/USS/ActionNode.uss";

    public ActionNode (int id) : base (true, false, Port.Capacity.Single) {

        // ノードインデックス設定
        textElement = new TextElement ();
        textElement.text = "index" + (id);

        // ノードIDを設定
        NodeId = id;

        outputContainer.RemoveFromHierarchy ();
    }

    public override void Invoke () {
        OnUpdate ();
    }
    public override void Reset () {
        this.ResultNodeState = STATE.NONE;
        this.NodeState = STATE.NONE;
    }
    public override void Init () {

        // ノード種別名設定
        title = "Action";
        // ノード種別設定
        NodeType = NODE.ACTION;
        // Uss読み込み
        styleSheets.Add (AssetDatabase.LoadAssetAtPath<StyleSheet> (ussPath));
        // ノード注釈設定
        textField = new TextField ();
        textField.RegisterCallback<FocusInEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.On; });
        textField.RegisterCallback<FocusOutEvent> (evt => { Input.imeCompositionMode = IMECompositionMode.Auto; });
        textField.multiline = true;
        textField.value = NodeAnnotation;
        textField.tooltip = "ノード注釈";

        if (BehaviorNodeData == null) BehaviorNodeData = new BehaviorNodeData ();

        titleButtonContainer.Clear ();

        // 要素をノードに追加
        ContainerAdd ();
    }
    protected override void OnInit () {

        // ノードステータスを実行中に変更
        NodeState = STATE.RUNNING;

        // 選択されている振る舞い種別を取得
        var select = mainContainer.Query<EnumField> ().First ();

        switch (select.value) {
            case BEHAVIOR.NONE:
                NodeState = STATE.SUCCSESS;
                break;
            case BEHAVIOR.TIME:
                RunBehaviorTime ();
                break;
            case BEHAVIOR.MOVE:
                RunBehaviorMove ();
                break;
            case BEHAVIOR.STATUS:
                RunBehaviorStatus ();
                break;
        }
    }
    protected override void OnUpdate () {
        if (NodeState == STATE.NONE) {
            OnInit ();
        }

        if (NodeState != STATE.RUNNING) {
            OnEnd ();
        }
    }
    protected override void OnEnd () {
        this.NodeState = ResultNodeState;
        Debug.Log (this.textElement.text + "の結果：" + this.NodeState);
    }

    public void ContainerAdd () {
        // UIフィールド初期化
        var enumField = InitEnumField (NodeBehavior);
        var timeDetectField = InitTimeDetectField ();
        var moveDetectField = InitMoveDetectField ();
        var statusDetectField = InitStatusDetectField ();

        // UIフィールドコールバック処理設定
        enumField.RegisterCallback<ChangeEvent<Enum>> ((evt) => {
            RemoveContainar (mainContainer, timeDetectField);
            RemoveContainar (mainContainer, moveDetectField);
            RemoveContainar (mainContainer, statusDetectField);

            switch (evt.newValue) {
                case BEHAVIOR.NONE:
                    break;
                case BEHAVIOR.TIME:
                    mainContainer.Add (timeDetectField);
                    break;
                case BEHAVIOR.MOVE:
                    mainContainer.Add (moveDetectField);
                    break;
                case BEHAVIOR.STATUS:
                    mainContainer.Add (statusDetectField);
                    break;
            }
            NodeBehavior = (BEHAVIOR) evt.newValue;
        });
        textField.RegisterCallback<ChangeEvent<string>> ((evt) => {
            NodeAnnotation = evt.newValue;
        });

        mainContainer.Add (textField);
        mainContainer.Add (enumField);
        titleButtonContainer.Add (textElement);

        // 　初期化によるUI生成
        switch (NodeBehavior) {
            case BEHAVIOR.NONE:
                break;
            case BEHAVIOR.TIME:
                mainContainer.Add (timeDetectField);
                break;
            case BEHAVIOR.MOVE:
                mainContainer.Add (moveDetectField);
                break;
            case BEHAVIOR.STATUS:
                mainContainer.Add (statusDetectField);
                break;
        }

        // ダミーデータUI生成
        InitCharacterField ();
    }

    private void RemoveContainar (VisualElement target, VisualElement template) {
        if (!target.Contains (template)) return;
        target.Remove (template);
    }

    private IEnumerator Wait (float time) {
        yield return new EditorWaitForSeconds (time);
        ResultNodeState = STATE.SUCCSESS;
        this.NodeState = STATE.COMPLETE;
    }

    private FloatField InitFloatField (BEHAVIOR behavior) {
        var cache = UiContainer.FloatFieldCache.CloneTree ();
        var floatField = cache.Query<VisualElement> ("FloatField").Children<FloatField> ().First ();

        floatField.label = "floatField：";
        // 　振る舞いによる要素設定
        switch (behavior) {
            case BEHAVIOR.NONE:
                break;
            case BEHAVIOR.TIME:
                floatField.value = BehaviorNodeData.waitTime;
                floatField.RegisterCallback<ChangeEvent<float>> ((evt) => { BehaviorNodeData.waitTime = evt.newValue; });
                break;
            case BEHAVIOR.MOVE:
                floatField.value = BehaviorNodeData.distance;
                floatField.RegisterCallback<ChangeEvent<float>> ((evt) => { BehaviorNodeData.distance = evt.newValue; });
                break;
            case BEHAVIOR.STATUS:
                break;
        }

        return floatField;
    }

    private EnumField InitEnumField (Enum enumData) {
        var cache = UiContainer.EnumFieldCache.CloneTree ();
        var enumField = cache.Query<VisualElement> ("EnumField").Children<EnumField> ().First ();

        // 列挙型設定
        enumField.Init (enumData);
        enumField.label = "振る舞い種別：";

        return enumField;
    }

    private void InitCharacterField () {
        var cache = UiContainer.CharacterFieldCache.CloneTree ();
        var uss = (StyleSheet) AssetDatabase.LoadAssetAtPath ("Assets/Editor/Resources/USS/CharacterField.uss", typeof (StyleSheet));
        cache.styleSheets.Add (uss);

        // UXMLバインド
        var posVec3Field = cache.Query<Foldout> ().Children<Vector3Field> ("PosXYZ").First ();
        var hpField = cache.Query<Foldout> ().Children<IntegerField> ("HP").First ();
        hpField.label = "HP:";
        var totalHpField = cache.Query<Foldout> ().Children<IntegerField> ("TOTALHP").First ();
        totalHpField.label = "TotalHP:";
        var spField = cache.Query<Foldout> ().Children<IntegerField> ("SP").First ();
        spField.label = "SP:";
        var strField = cache.Query<Foldout> ().Children<IntegerField> ("STR").First ();
        strField.label = "STR:";
        var vitField = cache.Query<Foldout> ().Children<IntegerField> ("VIT").First ();
        vitField.label = "VIT:";
        var dexField = cache.Query<Foldout> ().Children<IntegerField> ("DEX").First ();
        dexField.label = "DEX:";
        var spdField = cache.Query<Foldout> ().Children<IntegerField> ("SPD").First ();
        spdField.label = "SPD:";

        contentContainer.Add (cache);
    }

    private TemplateContainer InitTimeDetectField () {

        var cache = UiContainer.TimeDetectFieldCache.CloneTree ();

        // UXMLバインド
        var timeField = cache.Query<Foldout> ().Children<FloatField> ("TIME").First ();
        timeField.value = BehaviorNodeData.waitTime;

        // UIフィールドコールバック処理設定
        timeField.RegisterCallback<ChangeEvent<float>> ((evt) => {
            BehaviorNodeData.waitTime = evt.newValue;
        });

        return cache;
    }

    private TemplateContainer InitMoveDetectField () {

        var cache = UiContainer.MoveDetectFieldCache.CloneTree ();

        // UXMLバインド
        var distanceField = cache.Query<Foldout> ().Children<FloatField> ("DISTANCE").First ();
        distanceField.value = BehaviorNodeData.distance;

        var enumFieldMove = cache.Query<Foldout> ().Children<EnumField> ("DETECT").First ();

        // 列挙型設定
        enumFieldMove.Init (BehaviorNodeData.detect);

        // UIフィールドコールバック処理設定
        distanceField.RegisterCallback<ChangeEvent<float>> ((evt) => {
            BehaviorNodeData.distance = evt.newValue;
        });
        enumFieldMove.RegisterCallback<ChangeEvent<Enum>> ((evt) => {
            BehaviorNodeData.detect = (DETECT) evt.newValue;
        });

        return cache;
    }

    private TemplateContainer InitStatusDetectField () {

        var cache = UiContainer.StatusDetectFieldCache.CloneTree ();

        // UXMLバインド
        var statusField = cache.Query<Foldout> ().Children<EnumField> ("STATUS").First ();
        var valueField = cache.Query<Foldout> ().Children<FloatField> ("VALUE").First ();
        var calculateField = cache.Query<Foldout> ().Children<EnumField> ("CALCULATE").First ();

        calculateField.Init (BehaviorNodeData.conditionData.calculate);
        statusField.Init (BehaviorNodeData.conditionData.status);

        // フィールド値初期化
        calculateField.value = BehaviorNodeData.conditionData.calculate;
        valueField.value = BehaviorNodeData.ratio;
        statusField.value = BehaviorNodeData.conditionData.status;

        // UIフィールドコールバック処理設定
        calculateField.RegisterCallback<ChangeEvent<Enum>> ((evt) => {
            BehaviorNodeData.conditionData.calculate = (CALCULATE) evt.newValue;
        });
        valueField.RegisterCallback<ChangeEvent<float>> ((evt) => {
            BehaviorNodeData.ratio = evt.newValue;
        });
        statusField.RegisterCallback<ChangeEvent<Enum>> ((evt) => {
            BehaviorNodeData.conditionData.status = (CHARASTS) evt.newValue;
        });

        return cache;
    }
    private void RunBehaviorTime () {
        var waitTime = mainContainer.Query<FloatField> ().First ().value;
        EditorCoroutineUtility.StartCoroutine (Wait (waitTime), this);
    }

    private void RunBehaviorMove () {

        // 評価パラメータ
        var distance = mainContainer.Query<FloatField> ().First ().value;
        var position = Vector3.zero;

        // Dummy start
        var dummy = contentContainer.Query<Vector3Field> ("PosXYZ").First ().value;
        // Dummy end

        var result = STATE.NONE;

        switch (BehaviorNodeData.detect) {
            case DETECT.DistanceGreater:
                result = DetectUtil.DistanceGreater (position, dummy, distance) ? STATE.SUCCSESS : STATE.FAILURE;
                break;
            case DETECT.DistanceGreaterEqual:
                result = DetectUtil.DistanceGreaterEqual (position, dummy, distance) ? STATE.SUCCSESS : STATE.FAILURE;
                break;
            case DETECT.DistanceLess:
                result = DetectUtil.DistanceLess (position, dummy, distance) ? STATE.SUCCSESS : STATE.FAILURE;
                break;
            case DETECT.DistanceLessEqual:
                result = DetectUtil.DistanceLessEqual (position, dummy, distance) ? STATE.SUCCSESS : STATE.FAILURE;
                break;
            default:
                break;
        }

        ResultNodeState = result;
        this.NodeState = STATE.COMPLETE;
    }

    private void RunBehaviorStatus () {

        // NONE(未選択の場合 失敗で返却)
        if (!CharacterUtil.CharaStsDictionary.ContainsKey (BehaviorNodeData.conditionData.status)) {
            ResultNodeState = STATE.FAILURE;
            this.NodeState = STATE.COMPLETE;
            Debug.Log ("ステータス評価のステータスが未選択です。");
            return;
        }
        if (!CalculateUtil.MethodDictionary.ContainsKey (BehaviorNodeData.conditionData.calculate)) {
            ResultNodeState = STATE.FAILURE;
            this.NodeState = STATE.COMPLETE;
            Debug.Log ("ステータス評価の判定メソッドが未選択です。");
            return;
        }

        // 評価パラメータ
        Character dummyChara = SetupCharacter ();

        var current = CharacterUtil.CharaStsDictionary[BehaviorNodeData.conditionData.status] (dummyChara);
        var total = CharacterUtil.CharaStsDictionary[CHARASTS.TOTALHP] (dummyChara);
        var threshold = mainContainer.Query<FloatField> ("VALUE").First ().value;

        var result = CalculateUtil.MethodDictionary[BehaviorNodeData.conditionData.calculate] (current, total, threshold) ? STATE.SUCCSESS : STATE.FAILURE;

        ResultNodeState = result;
        this.NodeState = STATE.COMPLETE;
    }

    private Character SetupCharacter () {
        Character dummyChara = new Character ();

        var hp = contentContainer.Query<IntegerField> ("HP").First ().value;
        var totalHp = contentContainer.Query<IntegerField> ("TOTALHP").First ().value;
        var sp = contentContainer.Query<IntegerField> ("SP").First ().value;
        var str = contentContainer.Query<IntegerField> ("STR").First ().value;
        var vit = contentContainer.Query<IntegerField> ("VIT").First ().value;
        var dex = contentContainer.Query<IntegerField> ("DEX").First ().value;
        var spd = contentContainer.Query<IntegerField> ("SPD").First ().value;

        dummyChara.Hp = hp;
        dummyChara.TotalHp = totalHp;
        dummyChara.Sp = sp;
        dummyChara.Str = str;
        dummyChara.Vit = vit;
        dummyChara.Dex = dex;
        dummyChara.Spd = spd;

        return dummyChara;
    }

}