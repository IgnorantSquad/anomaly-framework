using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public interface IEventReceiver
    {
        void HandleReceivedEvent<T>(T e) where T : BaseEvent;

        void OnEventReceived(BaseEvent e, System.Type t);
    }

    public interface IEventSender
    {
        void SendEvent<T>(IEventReceiver to, T e) where T : BaseEvent;
    }
}
