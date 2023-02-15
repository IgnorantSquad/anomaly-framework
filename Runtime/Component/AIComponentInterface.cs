namespace Anomaly
{
    public interface AIFixedUpdater
    {
        void FixedUpdate();
    }

    public interface AIUpdater
    {
        void Update();
    }

    public interface AILateUpdater
    {
        void LateUpdate();
    }
}