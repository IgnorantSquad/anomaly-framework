using System.Collections.Generic;
using System.Reflection;

namespace Anomaly
{

    public static class UpdateManager
    {
        static Dictionary<EFunctionType, CustomList<CustomMethodBinder>> objectList = new Dictionary<EFunctionType, CustomList<CustomMethodBinder>>(new FunctionTypeExt());

        #region For FunctionType
        public enum EFunctionType : int
        {
            FIXEDUPDATE,
            FIXEDUPDATE_ISO,
            UPDATE,
            UPDATE_ISO,
            LATEUPDATE,
            LATEUPDATE_ISO
        }

        public struct FunctionTypeExt : IEqualityComparer<EFunctionType>
        {
            public bool Equals(EFunctionType a, EFunctionType b)
            {
                return a == b;
            }

            public int GetHashCode(EFunctionType f)
            {
                return (int)f;
            }
        }

        public class CustomMethodBinder
        {
            public CustomBehaviour target;
            public MethodInfo method;
            public bool Invoke(params object[] pl)
            {
                if (target == null || method == null) return true;
                method.Invoke(target, pl);
                return false;
            }
        }
        #endregion

        public static void Register(CustomBehaviour obj, MethodInfo m, EFunctionType type)
        {
            if (obj == null) return;
            if (!objectList.ContainsKey(type)) objectList.Add(type, CustomList<CustomMethodBinder>.Create(new CustomMethodBinder() { target = obj, method = m }));
            else objectList[type].Add(new CustomMethodBinder() { target = obj, method = m });
        }


        static bool IsNull(EFunctionType type, CustomList<CustomMethodBinder> target)
        {
            if (!objectList.ContainsKey(type)) return true;
            return false;
        }


        public static void FixedUpdate()
        {
            UpdateLoop(EFunctionType.FIXEDUPDATE, false);
            UpdateLoop(EFunctionType.FIXEDUPDATE_ISO, true);
        }

        public static void Update()
        {
            UpdateLoop(EFunctionType.UPDATE, false);
            UpdateLoop(EFunctionType.UPDATE_ISO, true);
        }

        public static void LateUpdate()
        {
            UpdateLoop(EFunctionType.LATEUPDATE, false);
            UpdateLoop(EFunctionType.LATEUPDATE_ISO, true);
        }

        static void UpdateLoop(EFunctionType type, bool isIsolated)
        {
            CustomList<CustomMethodBinder> search = objectList.ContainsKey(type) ? objectList[type] : null;
            while (search != null)
            {
                //if (IsNull(type, search)) continue;

                var target = search.data;

                if (target.target.gameObject.activeInHierarchy == false && !isIsolated)
                {
                    search = search.next;
                    continue;
                }

                target.Invoke();

                search = search.next;
            }
        }
    }

}