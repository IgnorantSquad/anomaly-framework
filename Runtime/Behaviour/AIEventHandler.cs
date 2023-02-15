using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public interface AIEventReceiver
    {
        void HandleReceivedEvent<T>(T e) where T : AEvent;

        void OnEventReceived(AEvent e, System.Type t);
    }

    public interface AIEventSender
    {
        void SendEvent<T>(AIEventReceiver to, T e = null) where T : AEvent, new();
    }
}
