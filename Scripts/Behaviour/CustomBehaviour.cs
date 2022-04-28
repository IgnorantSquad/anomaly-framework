using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomaly
{
    public class CustomBehaviour : MonoBehaviour
    {
        // GameObject _gameObject = null;
        // new public GameObject gameObject
        // {
        //     get
        //     {
        //         if (_gameObject == null) return base.gameObject;
        //         return _gameObject;
        //     }
        // }

        // Transform _transform = null;
        // new public Transform transform
        // {
        //     get
        //     {
        //         if (_transform == null) return base.transform;
        //         return _transform;
        //     }
        // }


        private HashSet<Type> sharedComponents = new HashSet<Type>();
        private Dictionary<Type, CustomComponent.BaseData> componentsData = new Dictionary<Type, CustomComponent.BaseData>();


        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            InitializeMagicFunc();
            InitializeComponents(this.GetType());
        }

        public void InitializeMagicFunc()
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

            // _gameObject = base.gameObject;
            // _transform = base.transform;

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
        public void InitializeComponents(params CustomComponent.BaseData[] data)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                componentsData.Add(data[i].GetType(), data[i]);
                var attribute = Attribute.GetCustomAttribute(data[i].GetType(), typeof(SharedComponentDataAttribute)) as SharedComponentDataAttribute;
                sharedComponents.Add(attribute.OuterType);
            }
        }

        public void InitializeComponents(System.Type targetType)
        {
            var fields = targetType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.FieldType.IsSubclassOf(typeof(CustomComponent.BaseData))) continue;

                if (componentsData.ContainsKey(field.FieldType)) continue;

                var attribute = Attribute.GetCustomAttribute(field.FieldType, typeof(SharedComponentDataAttribute)) as SharedComponentDataAttribute;
                if (attribute == null)
                {
                    Debug.LogError($"Wrong Data Format!! You must add SharedComponentData attribute. See {field.FieldType}.");
                    continue;
                }

                componentsData.Add(field.FieldType, field.GetValue(this) as CustomComponent.BaseData);
                sharedComponents.Add(attribute.OuterType);
            }
        }


        public T GetComponentData<T>() where T : CustomComponent.BaseData
        {
            if (!componentsData.ContainsKey(typeof(T))) throw new System.Exception("Wrong ComponentData Access");
            return componentsData[typeof(T)] as T;
        }

        public T GetSharedComponent<T>() where T : CustomComponent, new()
        {
            if (!sharedComponents.Contains(typeof(T))) throw new System.Exception("Wrong Component Access");
            return ComponentPool.Get<T>();
        }
    }
}
