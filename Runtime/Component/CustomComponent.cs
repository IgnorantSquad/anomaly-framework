namespace Anomaly
{
    public abstract class CustomComponent
    {
        public CustomBehaviour behaviour;

#if UNITY_EDITOR
        public virtual void OnInspectorGUI(CustomComponent target)
        {

        }
#endif
    }
}