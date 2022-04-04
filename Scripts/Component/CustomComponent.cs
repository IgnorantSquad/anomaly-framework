namespace Anomaly
{
    public abstract class CustomComponent
    {
        protected CustomBehaviour target;

        public virtual void Initialize(CustomBehaviour target)
        {
            this.target = target;
        }

#if UNITY_EDITOR
        public abstract void OnInspectorGUI(UnityEditor.Editor editor, UnityEditor.SerializedProperty target);
#endif
    }
}