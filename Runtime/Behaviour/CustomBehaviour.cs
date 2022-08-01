using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomaly
{
    public class CustomBehaviour : MonoBehaviour
    {
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

            var manager = Managers.Update;

            var method = GetMethod("OnFixedUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterFixedUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);

            method = GetMethod("OnUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);

            method = GetMethod("OnLateUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterLateUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);
        }

        public void InitializeComponents(System.Type targetType)
        {
            var fields = targetType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.FieldType.IsSubclassOf(typeof(CustomComponent))) continue;

                var data = field.GetValue(this);

                var castFixed = data as IFixedUpdater;
                var castUpdate = data as IUpdater;
                var castLast = data as ILateUpdater;

                if (castFixed != null)
                {
                    Managers.Update.RegisterFixedUpdate(this, castFixed.FixedUpdate);
                }
                if (castUpdate != null)
                {
                    Managers.Update.RegisterUpdate(this, castUpdate.Update);
                }
                if (castLast != null)
                {
                    Managers.Update.RegisterLateUpdate(this, castLast.LateUpdate);
                }
            }
        }
    }
}
