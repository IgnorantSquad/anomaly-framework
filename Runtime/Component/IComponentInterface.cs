namespace Anomaly
{
    public interface IFixedUpdater
    {
        void FixedUpdate();
    }

    public interface IUpdater
    {
        void Update();
    }

    public interface ILateUpdater
    {
        void LateUpdate();
    }
}