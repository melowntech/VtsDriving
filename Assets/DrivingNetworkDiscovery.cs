using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ReceivedEvent : UnityEvent<string, string>
{
}

public class DrivingNetworkDiscovery : NetworkDiscovery
{
    public ReceivedEvent received;

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        received.Invoke(fromAddress, data);
    }
}
