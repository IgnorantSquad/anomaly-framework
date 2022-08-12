namespace Anomaly
{
    public abstract class CustomComponent
    {
#if UNITY_EDITOR
        public virtual void OnInspectorGUI(CustomComponent target)
        {

        }
#endif
    }
}