using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    public class APoolManager : Anomaly.ABehaviour
    {
        public static APoolManager Instance { get; private set; }

        [SerializeField]
        private APoolRecipe recipe;


        private Dictionary<string, Queue<APoolObject>> pool = new Dictionary<string, Queue<APoolObject>>();


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
                pool.Add(name, new Queue<APoolObject>());
            }

            for (int i = 0; i < count; ++i)
            {
                var obj = Instantiate(target.gameObject).GetComponent<APoolObject>();
                obj.Name = name;
                obj.Return();
            }
        }


        public APoolObject Get(string name)
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
            obj.CurrentState = APoolObject.State.Using;
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Add(APoolObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.CurrentState = APoolObject.State.Prepare;
            pool[obj.Name].Enqueue(obj);
        }
    }
}