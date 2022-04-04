using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomaly
{
    public class CustomBehaviour : MonoBehaviour
    {
        GameObject _gameObject = null;
        new public GameObject gameObject
        {
            get
            {
                if (_gameObject == null) return base.gameObject;
                return _gameObject;
            }
        }

        Transform _transform = null;
        new public Transform transform
        {
            get
            {
                if (_transform == null) return base.transform;
                return _transform;
            }
        }


        void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            MethodInfo GetMethod(string methodName)
            {
                var type = GetType();

                while (type != typeof(System.Object))
                {
                    MethodInfo info = GetType()
                        .GetMethod(methodName,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (info != null) return info;
                    type = type.BaseType;
                }
                return null;
            }
            bool IsValidMagicFunction(MethodInfo method)
            {
                return method != null
                    && method.GetParameters().Length == 0
                    && method.ReturnType == typeof(void);
            }

            _gameObject = base.gameObject;
            _transform = base.transform;

            var self = this;

            (string, UpdateManager.EFunctionType)[] methodList = new (string, UpdateManager.EFunctionType)[] {
                ("OnFixedUpdate", UpdateManager.EFunctionType.FIXEDUPDATE),
                ("OnISOFixedUpdate", UpdateManager.EFunctionType.FIXEDUPDATE_ISO),
                ("OnUpdate", UpdateManager.EFunctionType.UPDATE),
                ("OnISOUpdate", UpdateManager.EFunctionType.UPDATE_ISO),
                ("OnLateUpdate", UpdateManager.EFunctionType.LATEUPDATE),
                ("OnISOLateUpdate", UpdateManager.EFunctionType.LATEUPDATE_ISO)
            };

            for (int i = 0; i < methodList.Length; ++i)
            {
                var method = GetMethod(methodList[i].Item1);
                if (!IsValidMagicFunction(method)) continue;
                UpdateManager.Register(this, method, methodList[i].Item2);
            }
        }

        protected void InitializeComponent(params CustomComponent[] components)
        {
            for (int i = 0; i < components.Length; ++i)
            {
                components[i].Initialize(this);
            }
        }


#if UNITY_EDITOR
        public virtual void OnInspectorGUI(UnityEditor.Editor editor, UnityEditor.SerializedObject serializedObject)
        {

        }
#endif
    }
}


#if UNITY_EDITOR
namespace Anomaly.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(CustomBehaviour), true)]
    public class CustomObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            (target as CustomBehaviour).OnInspectorGUI(this, serializedObject);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty((target as CustomBehaviour).gameObject.scene);
            }
        }
    }
}
#endif
