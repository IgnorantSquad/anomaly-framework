namespace Anomaly
{
    public abstract class AComponent
    {
        [UnityEngine.HideInInspector]
        public ABehaviour behaviour;

#if UNITY_EDITOR
        public virtual void OnInspectorGUI(AComponent target)
        {

        }
#endif
    }
}