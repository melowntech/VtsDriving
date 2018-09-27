using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

internal class Peer
{
    public Button button;
    public string address;
    public string data;
    public float last;

    public Peer(string adr, string data, Button but)
    {
        address = adr;
        this.data = data;
        button = but;
        last = Time.time;
    }
}

public class NetworkGui : MonoBehaviour
{
    public Button peerButton;

    InputField joinAddress;
    InputField createName;
    GameObject peersList;

    NetworkManager man;
    DrivingNetworkDiscovery disc;

    Dictionary<string, Peer> peers = new Dictionary<string, Peer>();

    void Start()
    {
        joinAddress = GameObject.Find("JoinAddress").GetComponent<InputField>();
        createName = GameObject.Find("CreateName").GetComponent<InputField>();
        peersList = GameObject.Find("PeersList");
        man = FindObjectOfType<NetworkManager>();
        disc = FindObjectOfType<DrivingNetworkDiscovery>();
        disc.received.AddListener(BroadcastReceived);
        disc.Initialize();
        disc.StartAsClient();
    }

    void RegenerateGui()
    {
        uint idx = 0;
        foreach (var p in peers)
        {
            var rt = p.Value.button.GetComponent<Transform>();
            rt.localPosition = new Vector3(rt.localPosition[0], (idx++) * -40f - 10f, 0);
        }
    }

    public void BroadcastReceived(string adr, string data)
    {
        if (peers.ContainsKey(adr))
        {
            peers[adr].last = Time.time;
        }
        else
        {
            var but = Instantiate(peerButton, peersList.transform, false);
            but.GetComponentInChildren<Text>().text = data;
            but.GetComponent<Button>().onClick.AddListener(delegate { JoinPeer(adr); });
            peers.Add(adr, new Peer(adr, data, but));
            RegenerateGui();
        }
    }

    void Update()
    {
        List<string> keysToDelete = new List<string>();
        foreach (var p in peers)
        {
            if (p.Value.last + 3 < Time.time)
                keysToDelete.Add(p.Key);
        }
        foreach (var k in keysToDelete)
            peers.Remove(k);
        if (keysToDelete.Count > 0)
            RegenerateGui();
    }

    public void JoinButton()
    {
        if (disc.isClient)
            disc.StopBroadcast();
        man.networkAddress = joinAddress.text;
        man.StartClient();
    }

    public void CreateButton()
    {
        disc.StopBroadcast();
        disc.broadcastData = createName.text;
        disc.Initialize();
        disc.StartAsServer();
        man.StartHost();
    }

    public void JoinPeer(string adr)
    {
        disc.StopBroadcast();
        man.networkAddress = adr;
        man.StartClient();
    }
}
