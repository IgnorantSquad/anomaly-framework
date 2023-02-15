using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomaly
{
    public class ABehaviour : MonoBehaviour, AIEventReceiver, AIEventSender
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

            var manager = AUpdateManager.Instance;

            var method = GetMethod("OnFixedUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterFixedUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);

            method = GetMethod("OnUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);

            method = GetMethod("OnLateUpdate");
            if (IsValidMagicFunction(method)) manager.RegisterLateUpdate(this, method.CreateDelegate(typeof(System.Action), this) as System.Action);
        }

        public void InitializeComponents(System.Type targetType)
        {
            var manager = AUpdateManager.Instance;

            var fields = targetType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.FieldType.IsSubclassOf(typeof(AComponent))) continue;

                var data = field.GetValue(this);

                (data as AComponent).behaviour = this;

                var castFixed = data as AIFixedUpdater;
                var castUpdate = data as AIUpdater;
                var castLast = data as AILateUpdater;

                if (castFixed != null)
                {
                    manager.RegisterFixedUpdate(this, castFixed.FixedUpdate);
                }
                if (castUpdate != null)
                {
                    manager.RegisterUpdate(this, castUpdate.Update);
                }
                if (castLast != null)
                {
                    manager.RegisterLateUpdate(this, castLast.LateUpdate);
                }
            }
        }


        public void HandleReceivedEvent<T>(T e) where T : AEvent
        {
            e.Invoke();
            OnEventReceived(e, typeof(T));
        }

        public void SendEvent<T>(AIEventReceiver to, T e = null) where T : AEvent, new()
        {
            if (e == null) e = new T();
            e.sender = this;
            e.receiver = to as ABehaviour;
            to.HandleReceivedEvent(e);
        }

        public virtual void OnEventReceived(AEvent e, Type t)
        {
        }
    }
}
