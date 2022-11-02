using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    public class PoolManager : Anomaly.CustomBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [SerializeField]
        private PoolRecipe recipe;


        private Dictionary<string, Queue<PoolObject>> pool = new Dictionary<string, Queue<PoolObject>>();


        protected override void Awake()
        {
            if (!ReferenceEquals(Instance, null))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (a, b) =>
            {
                var keys = pool.Keys;
                foreach (var key in keys)
                {
                    while (pool[key].Count > 0)
                    {
                        pool[key].Dequeue();
                    }
                }
            };
        }


        public void WarmUp(string name, int count)
        {
            var target = recipe.Find(name);
            if (target == null) return;

            if (!pool.ContainsKey(name))
            {
                pool.Add(name, new Queue<PoolObject>());
            }

            for (int i = 0; i < count; ++i)
            {
                var obj = Instantiate(target.gameObject).GetComponent<PoolObject>();
                obj.Name = name;
                obj.Return();
            }
        }


        public PoolObject Get(string name)
        {
            if (!pool.ContainsKey(name))
            {
                WarmUp(name, 5);
            }

            if (pool[name].Count == 0)
            {
                WarmUp(name, 1);
            }

            var obj = pool[name].Dequeue();
            obj.CurrentState = PoolObject.State.Using;
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Add(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.CurrentState = PoolObject.State.Prepare;
            pool[obj.Name].Enqueue(obj);
        }
    }
}