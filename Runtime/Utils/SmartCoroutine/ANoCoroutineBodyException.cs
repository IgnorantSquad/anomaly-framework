namespace Anomaly.Utils
{
    public class ANoCoroutineBodyException : System.Exception
    {
        public override string Message => "This instance is body-less coroutine. You cannot reuse it.";
    }
}