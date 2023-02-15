using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [CreateAssetMenu(menuName = "Pool Recipe", fileName = "PoolRecipe")]
    public class APoolRecipe : ScriptableObject
    {
        public APoolObject[] objectList;


        public APoolObject Find(string name)
        {
            for (int i = 0; i < objectList.Length; ++i)
            {
                if (objectList[i].name != name) continue;
                return objectList[i];
            }
            return null;
        }
    }
}