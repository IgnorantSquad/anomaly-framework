using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [System.Serializable]
    public partial class SerializableDictionary<_Typ> : ISerializationCallbackReceiver
    {
        #region Constructor
        public SerializableDictionary() { }
        public SerializableDictionary(string defaultKey, _Typ defaultValue = default(_Typ))
        {
            keyList = new string[] { defaultKey };
            valueList = new _Typ[] { defaultValue };
            OnAfterDeserialize();
        }
        #endregion

        public Dictionary<string, _Typ> Container { get; set; } = new Dictionary<string, _Typ>();

        public _Typ Get(string key)
        {
            Debug.Assert(Container.TryGetValue(key, out var value));
            return value;
        }

        #region Serialization
        [SerializeField] private string[] keyList;
        [SerializeField] private _Typ[] valueList;

        public void OnAfterDeserialize()
        {
            Container.Clear();

            for (int i = 0; i < Mathf.Min(keyList.Length, valueList.Length); ++i)
            {
                // Avoid conflict
                if (Container.ContainsKey(keyList[i]))
                {
                    keyList[i] = System.Guid.NewGuid().ToString();
                }
                Container.Add(keyList[i], valueList[i]);
            }

            keyList = null;
            valueList = null;
        }
        public void OnBeforeSerialize()
        {
            keyList = new string[Container.Keys.Count];
            valueList = new _Typ[Container.Values.Count];

            Container.Keys.CopyTo(keyList, 0);
            Container.Values.CopyTo(valueList, 0);
        }
        #endregion
    }

    public class AnimationEventDictionary
    {

    }
}

#if UNITY_EDITOR
namespace Anomaly.Utils
{
    using UnityEditor;

    public abstract class SerializableDictionaryDrawer : PropertyDrawer
    {
        private SerializedProperty keyList, valueList;
        private int dictionarySize = 0;
        private bool foldout = false;

        protected virtual string KeyLabel => "Key";
        protected virtual string ValueLabel => "Value";

        protected abstract float Margin { get; }

        private void Initialize(SerializedProperty property)
        {
            keyList = property.FindPropertyRelative("keyList");
            valueList = property.FindPropertyRelative("valueList");

            dictionarySize = Mathf.Min(keyList.arraySize, valueList.arraySize);
        }

        private Rect Display(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = position;

            rect.x += 10F;
            rect.size = new Vector2(GUI.skin.label.CalcSize(label).x + 10F, 18F);

            foldout = EditorGUI.BeginFoldoutHeaderGroup(rect, foldout, label);

            if (!foldout)
            {
                EditorGUI.EndFoldoutHeaderGroup();
                return rect;
            }

            rect = DisplayAddButton(rect, position);

            rect = DisplayDictionary(rect, position);

            EditorGUI.EndFoldoutHeaderGroup();
            return rect;
        }

        private Rect DisplayAddButton(Rect rect, Rect origin)
        {
            rect.position = new Vector2(origin.x + origin.width * 0.8f - 50F, rect.y);
            rect.size = new Vector2(80F, 20F);

            if (GUI.Button(rect, "Add"))
            {
                keyList.arraySize++;
                valueList.arraySize++;
            }

            rect.y += 23F;

            return rect;
        }

        private Rect DisplayDictionary(Rect rect, Rect origin)
        {
            for (int i = 0; i < dictionarySize; ++i)
            {
                int cachedIndex = i;
                rect = DispalyElement(rect, origin, cachedIndex);
                dictionarySize = Mathf.Min(keyList.arraySize, valueList.arraySize);
            }

            return rect;
        }

        private Rect DispalyElement(Rect rect, Rect origin, int index)
        {
            rect.x = origin.x + 30F;
            rect.size = new Vector2(70F, 18F);
            if (!string.IsNullOrEmpty(KeyLabel)) GUI.Label(rect, KeyLabel);

            rect.x += 70F - 9 * EditorGUI.indentLevel;
            rect.width = origin.width * 0.8f - 70F;
            EditorGUI.PropertyField(rect, keyList.GetArrayElementAtIndex(index), GUIContent.none);

            rect.x += origin.width * 0.8f - 62.5f;
            rect.size = Vector2.one * 20F;
            if (GUI.Button(rect, "x"))
            {
                keyList.DeleteArrayElementAtIndex(index);
                valueList.DeleteArrayElementAtIndex(index);
                return rect;
            }

            rect.position = new Vector2(origin.x + 30F, rect.y + 22.5f);
            rect.size = new Vector2(70F, 18F);
            if (!string.IsNullOrEmpty(ValueLabel)) GUI.Label(rect, ValueLabel);

            rect.x += !string.IsNullOrEmpty(ValueLabel) ? 70F - 9 * EditorGUI.indentLevel : 0F;
            rect.width = !string.IsNullOrEmpty(ValueLabel) ? origin.width * 0.8f - 70F : origin.width * 0.8f;
            EditorGUI.PropertyField(rect, valueList.GetArrayElementAtIndex(index), GUIContent.none);

            rect.y += Margin;

            return rect;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Initialize(property);

            Display(EditorGUI.IndentedRect(position), property, label);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return dictionarySize == 0 || !foldout ? 18F : (Margin + 22.5f) * dictionarySize + 20F;
        }
    }

    [CustomPropertyDrawer(typeof(SerializableDictionary<AnimationEventCallback>))]
    public class SerializableDictionaryEventDrawer : SerializableDictionaryDrawer
    {
        protected override string KeyLabel => "Function";
        protected override string ValueLabel => string.Empty;

        protected override float Margin => 110F;
    }

    [CustomPropertyDrawer(typeof(SerializableDictionary<int>))]
    [CustomPropertyDrawer(typeof(SerializableDictionary<float>))]
    [CustomPropertyDrawer(typeof(SerializableDictionary<bool>))]
    [CustomPropertyDrawer(typeof(SerializableDictionary<string>))]
    public partial class SerializableDictionaryValueDrawer : SerializableDictionaryDrawer
    {
        protected override float Margin => 38F;
    }


    public partial class SerializableDictionaryValueDrawer { }
}
#endif