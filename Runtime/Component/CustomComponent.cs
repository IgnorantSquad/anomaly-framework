namespace Anomaly
{
    public abstract class CustomComponent
    {
        [UnityEngine.HideInInspector]
        public CustomBehaviour behaviour;

#if UNITY_EDITOR
        public virtual void OnInspectorGUI(CustomComponent target)
        {

        }
#endif
    }
}