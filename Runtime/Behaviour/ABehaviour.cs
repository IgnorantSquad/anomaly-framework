using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Anomaly
{
    public class ABehaviour : MonoBehaviour, AIEventReceiver, AIEventSender
    {
        private AUpdateManager.Data onFixedUpdate = new AUpdateManager.Data();
        private AUpdateManager.Data onUpdate = new AUpdateManager.Data();
        private AUpdateManager.Data onLateUpdate = new AUpdateManager.Data();

        protected virtual void OnEnable()
        {
            if (onFixedUpdate.target != null) AUpdateManager.Instance.RegisterFixedUpdate(onFixedUpdate);
            if (onUpdate.target != null) AUpdateManager.Instance.RegisterUpdate(onUpdate);
            if (onLateUpdate.target != null) AUpdateManager.Instance.RegisterLateUpdate(onLateUpdate);
        }

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
            Type classType = GetType();
            BindingFlags methodFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            var manager = AUpdateManager.Instance;

            MethodInfo method = FindMethod(classType, "OnFixedUpdate", methodFlag);
            if (method != null) onFixedUpdate.target = this;

            method = FindMethod(classType, "OnUpdate", methodFlag);
            if (method != null) onUpdate.target = this;

            method = FindMethod(classType, "OnLateUpdate", methodFlag);
            if (method != null) onLateUpdate.target = this;


            IEnumerable<Type> GetTypeRecursively(Type t)
            {
                while (t != typeof(ABehaviour))
                {
                    yield return t;
                    t = t.BaseType;
                }
            }

            MethodInfo FindMethod(Type t, string name, BindingFlags flag)
            {
                var search = GetTypeRecursively(t);
                foreach (Type current in search)
                {
                    var info = current.GetMethod(name, flag);
                    if (info == null) continue;
                    if (info.IsAbstract) continue;
                    if (info.DeclaringType != current) continue;
                    return info;
                }

                return null;
            }

            bool IsValidEventFunction(MethodInfo method)
            {
                if (method == null) return false;
                if (method.ReturnType != typeof(void)) return false;

                var param = method.GetParameters();
                if (param == null || param.Length != 1) return false;

                if (!param[0].ParameterType.IsSubclassOf(typeof(AEvent))) return false;

                return true;
            }
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
                var castLate = data as AILateUpdater;

                int id = GetInstanceID();

                if (castFixed != null)
                {
                    if (onFixedUpdate.method == null) onFixedUpdate.method = castFixed.FixedUpdate;
                    else onFixedUpdate.method += castFixed.FixedUpdate;
                }
                if (castUpdate != null)
                {
                    if (onUpdate.method == null) onUpdate.method = castUpdate.Update;
                    else onUpdate.method += castUpdate.Update;
                }
                if (castLate != null)
                {
                    if (onLateUpdate.method == null) onLateUpdate.method = castLate.LateUpdate;
                    else onLateUpdate.method += castLate.LateUpdate;
                }
            }
        }

        public virtual void OnFixedUpdate() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }


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
