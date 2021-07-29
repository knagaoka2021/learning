using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIDesign {

    public static class UiContainer {
        // resourceパス
        static readonly string ResourcesPath = "Assets/Editor/Resources/";
        // UXMLテンプレートパス
        static readonly string UxmlTemplatePath = "UXML/Template/";
        static readonly string UxmlCustomPath = "UXML/Custom/";
        // resourceUXMLテンプレートパス
        static readonly string ResourcesUxmlTemplatePath = ResourcesPath + UxmlTemplatePath;
        static readonly string ResourcesUxmlCustomPath = ResourcesPath + UxmlCustomPath;

        // UXMLファイル名
        static readonly string BoundsFieldFileName = "BoundsField.uxml";
        static readonly string BoundsIntFieldFileName = "BoundsIntField.uxml";
        static readonly string ButtonFileName = "Button.uxml";
        static readonly string ColorFieldFileName = "ColorField.uxml";
        static readonly string CurveFieldFileName = "CurveField.uxml";
        static readonly string DoubleFieldFileName = "DoubleField.uxml";
        static readonly string EnumFieldFileName = "EnumField.uxml";
        static readonly string FloatFieldFileName = "FloatField.uxml";
        static readonly string FoldOutFileName = "FoldOut.uxml";
        static readonly string GradientFieldFileName = "GradientField.uxml";
        static readonly string ImageFileName = "Image.uxml";
        static readonly string IntegerFieldFileName = "IntegerField.uxml";
        static readonly string LabelFileName = "Label.uxml";
        static readonly string LayerFieldFileName = "LayerField.uxml";
        static readonly string LayerMaskFieldFileName = "LayerMaskField.uxml";
        static readonly string LongFieldFileName = "LongField.uxml";
        static readonly string MaskFieldFileName = "MaskField.uxml";
        static readonly string RectFieldFileName = "RectField.uxml";
        static readonly string RectIntFieldFileName = "RectIntField.uxml";
        static readonly string TagFieldFileName = "TagField.uxml";
        static readonly string TextFieldFileName = "TextField.uxml";
        static readonly string ToggleFileName = "Toggle.uxml";
        static readonly string Vector2FieldFileName = "Vector2Field.uxml";
        static readonly string Vector2IntFieldFileName = "Vector2IntField.uxml";
        static readonly string Vector3FieldFileName = "vector3Field.uxml";
        static readonly string Vector3IntFieldFileName = "Vector3IntField.uxml";
        static readonly string Vector4FieldFileName = "Vector4Field.uxml";
        static readonly string Vector4IntFieldFileName = "Vector4IntField.uxml";
        static readonly string CharacterFieldFileName = "CharacterField.uxml";
        static readonly string TimeDetectFieldFileName = "TimeDetectField.uxml";
        static readonly string MoveDetectFieldFileName = "MoveDetectField.uxml";
        static readonly string StatusDetectFieldFileName = "StatusDetectField.uxml";

        // キャッシュ変数
        static VisualTreeAsset boundsFieldCache;
        static VisualTreeAsset boundsIntFieldCache;
        static VisualTreeAsset buttonCache;
        static VisualTreeAsset colorFieldCache;
        static VisualTreeAsset curveFieldCache;
        static VisualTreeAsset doubleFieldCache;
        static VisualTreeAsset enumFieldCache;
        static VisualTreeAsset floatFieldCache;
        static VisualTreeAsset foldOutCache;
        static VisualTreeAsset gradientFieldCache;
        static VisualTreeAsset imageCache;
        static VisualTreeAsset integerFieldCache;
        static VisualTreeAsset labelCache;
        static VisualTreeAsset layerFieldCache;
        static VisualTreeAsset layerMaskFieldCache;
        static VisualTreeAsset longFieldCache;
        static VisualTreeAsset maskFieldCache;
        static VisualTreeAsset rectFieldCache;
        static VisualTreeAsset rectIntFieldCache;
        static VisualTreeAsset tagFieldCache;
        static VisualTreeAsset textFieldCache;
        static VisualTreeAsset toggleCache;
        static VisualTreeAsset vector2FieldCache;
        static VisualTreeAsset vector2IntFieldCache;
        static VisualTreeAsset vector3FieldCache;
        static VisualTreeAsset vector3IntFieldCache;
        static VisualTreeAsset vector4FieldCache;
        static VisualTreeAsset vector4IntFieldCache;
        static VisualTreeAsset characterFieldCache;
        static VisualTreeAsset timeDetectFieldCache;
        static VisualTreeAsset moveDetectFieldCache;
        static VisualTreeAsset statusDetectFieldCache;

        // デリゲート
        public delegate T CacheDelegate<T> ();

        // キャッシュ取得
        public static VisualTreeAsset BoundsFieldCache { get { return GetCache (ref boundsFieldCache, BoundsFieldGetter); } }
        public static VisualTreeAsset BoundsIntFieldCache { get { return GetCache (ref boundsIntFieldCache, BoundsIntFieldGetter); } }
        public static VisualTreeAsset ButtonCache { get { return GetCache (ref buttonCache, ButtonCacheGetter); } }
        public static VisualTreeAsset ColorFieldCache { get { return GetCache (ref colorFieldCache, ColorFieldGetter); } }
        public static VisualTreeAsset CurveFieldCache { get { return GetCache (ref curveFieldCache, CurveFieldGetter); } }
        public static VisualTreeAsset DoubleFieldCache { get { return GetCache (ref doubleFieldCache, DoubleFieldGetter); } }
        public static VisualTreeAsset EnumFieldCache { get { return GetCache (ref enumFieldCache, EnumFieldGetter); } }
        public static VisualTreeAsset FloatFieldCache { get { return GetCache (ref floatFieldCache, FloatFieldGetter); } }
        public static VisualTreeAsset FoldOutCache { get { return GetCache (ref foldOutCache, FoldOutGetter); } }
        public static VisualTreeAsset GradientFieldCache { get { return GetCache (ref gradientFieldCache, GradientFieldGetter); } }
        public static VisualTreeAsset ImageCache { get { return GetCache (ref imageCache, ImageGetter); } }
        public static VisualTreeAsset IntegerFieldCache { get { return GetCache (ref integerFieldCache, IntegerFieldGetter); } }
        public static VisualTreeAsset LabelCache { get { return GetCache (ref labelCache, LabelGetter); } }
        public static VisualTreeAsset LayerFieldCache { get { return GetCache (ref layerFieldCache, LayerFieldGetter); } }
        public static VisualTreeAsset LayerMaskFieldCache { get { return GetCache (ref layerMaskFieldCache, LayerMaskFieldGetter); } }
        public static VisualTreeAsset LongFieldCache { get { return GetCache (ref longFieldCache, LongFieldGetter); } }
        public static VisualTreeAsset MaskFieldCache { get { return GetCache (ref maskFieldCache, MaskFieldGetter); } }
        public static VisualTreeAsset RectFieldCache { get { return GetCache (ref rectFieldCache, RectFieldGetter); } }
        public static VisualTreeAsset RectIntFieldCache { get { return GetCache (ref rectIntFieldCache, RectIntFieldGetter); } }
        public static VisualTreeAsset TagFieldCache { get { return GetCache (ref tagFieldCache, TagFieldGetter); } }
        public static VisualTreeAsset TextFieldCache { get { return GetCache (ref textFieldCache, TextFieldGetter); } }
        public static VisualTreeAsset ToggleCache { get { return GetCache (ref toggleCache, ToggleGetter); } }
        public static VisualTreeAsset Vector2FieldCache { get { return GetCache (ref vector2FieldCache, Vector2FieldGetter); } }
        public static VisualTreeAsset Vector2IntFieldCache { get { return GetCache (ref vector2IntFieldCache, Vector2IntFieldGetter); } }
        public static VisualTreeAsset Vector3FieldCache { get { return GetCache (ref vector3FieldCache, Vector3FieldGetter); } }
        public static VisualTreeAsset Vector3IntFieldCache { get { return GetCache (ref vector3IntFieldCache, Vector3IntFieldGetter); } }
        public static VisualTreeAsset Vector4FieldCache { get { return GetCache (ref vector4FieldCache, Vector4FieldGetter); } }
        public static VisualTreeAsset Vector4IntFieldCache { get { return GetCache (ref vector4IntFieldCache, Vector4IntFieldGetter); } }
        public static VisualTreeAsset CharacterFieldCache { get { return GetCache (ref characterFieldCache, CharacterFieldGetter); } }
        public static VisualTreeAsset TimeDetectFieldCache { get { return GetCache (ref timeDetectFieldCache, TimeDetectFieldGetter); } }
        public static VisualTreeAsset MoveDetectFieldCache { get { return GetCache (ref moveDetectFieldCache, MoveDetectFieldGetter); } }
        public static VisualTreeAsset StatusDetectFieldCache { get { return GetCache (ref statusDetectFieldCache, StatusDetectFieldGetter); } }

        // getter
        private static VisualTreeAsset BoundsFieldGetter () { return LoadAssetTemplateUxml (BoundsFieldFileName); }
        private static VisualTreeAsset BoundsIntFieldGetter () { return LoadAssetTemplateUxml (BoundsIntFieldFileName); }
        private static VisualTreeAsset ButtonCacheGetter () { return LoadAssetTemplateUxml (ButtonFileName); }
        private static VisualTreeAsset ColorFieldGetter () { return LoadAssetTemplateUxml (ColorFieldFileName); }
        private static VisualTreeAsset CurveFieldGetter () { return LoadAssetTemplateUxml (CurveFieldFileName); }
        private static VisualTreeAsset DoubleFieldGetter () { return LoadAssetTemplateUxml (DoubleFieldFileName); }
        private static VisualTreeAsset EnumFieldGetter () { return LoadAssetTemplateUxml (EnumFieldFileName); }
        private static VisualTreeAsset FloatFieldGetter () { return LoadAssetTemplateUxml (FloatFieldFileName); }
        private static VisualTreeAsset FoldOutGetter () { return LoadAssetTemplateUxml (FoldOutFileName); }
        private static VisualTreeAsset GradientFieldGetter () { return LoadAssetTemplateUxml (GradientFieldFileName); }
        private static VisualTreeAsset ImageGetter () { return LoadAssetTemplateUxml (ImageFileName); }
        private static VisualTreeAsset IntegerFieldGetter () { return LoadAssetTemplateUxml (IntegerFieldFileName); }
        private static VisualTreeAsset LabelGetter () { return LoadAssetTemplateUxml (LabelFileName); }
        private static VisualTreeAsset LayerFieldGetter () { return LoadAssetTemplateUxml (LayerFieldFileName); }
        private static VisualTreeAsset LayerMaskFieldGetter () { return LoadAssetTemplateUxml (LayerMaskFieldFileName); }
        private static VisualTreeAsset LongFieldGetter () { return LoadAssetTemplateUxml (LongFieldFileName); }
        private static VisualTreeAsset MaskFieldGetter () { return LoadAssetTemplateUxml (MaskFieldFileName); }
        private static VisualTreeAsset RectFieldGetter () { return LoadAssetTemplateUxml (RectFieldFileName); }
        private static VisualTreeAsset RectIntFieldGetter () { return LoadAssetTemplateUxml (RectIntFieldFileName); }
        private static VisualTreeAsset TagFieldGetter () { return LoadAssetTemplateUxml (TagFieldFileName); }
        private static VisualTreeAsset TextFieldGetter () { return LoadAssetTemplateUxml (TextFieldFileName); }
        private static VisualTreeAsset ToggleGetter () { return LoadAssetTemplateUxml (ToggleFileName); }
        private static VisualTreeAsset Vector2FieldGetter () { return LoadAssetTemplateUxml (Vector2FieldFileName); }
        private static VisualTreeAsset Vector2IntFieldGetter () { return LoadAssetTemplateUxml (Vector2IntFieldFileName); }
        private static VisualTreeAsset Vector3FieldGetter () { return LoadAssetTemplateUxml (Vector3FieldFileName); }
        private static VisualTreeAsset Vector3IntFieldGetter () { return LoadAssetTemplateUxml (Vector3IntFieldFileName); }
        private static VisualTreeAsset Vector4FieldGetter () { return LoadAssetTemplateUxml (Vector4FieldFileName); }
        private static VisualTreeAsset Vector4IntFieldGetter () { return LoadAssetTemplateUxml (Vector4IntFieldFileName); }
        private static VisualTreeAsset CharacterFieldGetter () { return LoadAssetCustomUxml (CharacterFieldFileName); }
        private static VisualTreeAsset TimeDetectFieldGetter () { return LoadAssetCustomUxml (TimeDetectFieldFileName); }
        private static VisualTreeAsset MoveDetectFieldGetter () { return LoadAssetCustomUxml (MoveDetectFieldFileName); }
        private static VisualTreeAsset StatusDetectFieldGetter () { return LoadAssetCustomUxml (StatusDetectFieldFileName); }


        public static VisualTreeAsset LoadAssetTemplateUxml (string fileName) {

            VisualTreeAsset vta = null;
            try {
                vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> (ResourcesUxmlTemplatePath + fileName);

                if (vta == null) {
                    throw new ArgumentNullException ("Asset not found path " + ResourcesUxmlTemplatePath + fileName);
                }

            } catch (ArgumentNullException e) {
                Debug.Log (e.Message);
            }

            return vta;
        }
        public static VisualTreeAsset LoadAssetCustomUxml (string fileName) {

            VisualTreeAsset vta = null;
            try {
                vta = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> (ResourcesUxmlCustomPath + fileName);

                if (vta == null) {
                    throw new ArgumentNullException ("Asset not found path " + ResourcesUxmlTemplatePath + fileName);
                }

            } catch (ArgumentNullException e) {
                Debug.Log (e.Message);
            }

            return vta;
        }
        private static T GetCache<T> (ref T cache, CacheDelegate<T> callMethod) where T : VisualTreeAsset {
            return (cache ?? (cache = callMethod ()));
        }
        public static void CacheClear () {
            boundsFieldCache = null;
            boundsIntFieldCache = null;
            buttonCache = null;
            colorFieldCache = null;
            curveFieldCache = null;
            doubleFieldCache = null;
            enumFieldCache = null;
            floatFieldCache = null;
            foldOutCache = null;
            gradientFieldCache = null;
            imageCache = null;
            integerFieldCache = null;
            labelCache = null;
            layerFieldCache = null;
            layerMaskFieldCache = null;
            longFieldCache = null;
            maskFieldCache = null;
            rectFieldCache = null;
            rectIntFieldCache = null;
            tagFieldCache = null;
            textFieldCache = null;
            toggleCache = null;
            vector2FieldCache = null;
            vector2IntFieldCache = null;
            vector3FieldCache = null;
            vector3IntFieldCache = null;
            vector4FieldCache = null;
            vector4IntFieldCache = null;
            characterFieldCache = null;
            timeDetectFieldCache = null;
            moveDetectFieldCache = null;
            statusDetectFieldCache = null;
        }

    }
}