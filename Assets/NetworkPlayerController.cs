using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeColor")]
    Color color;

    void Start()
    {
        if (isLocalPlayer)
        {
            GameObject map = FindObjectOfType<VtsMap>().gameObject;
            GetComponent<VtsColliderProbe>().mapObject = map;
            GetComponent<VtsRigidBodyActivate>().map = map;
            GameObject.Find("mainCamera").GetComponent<FollowingCamera>().target = gameObject;
            GameObject.Find("mainCamera").GetComponent<FollowingCamera>().enabled = true;
            GameObject.Find("minimapCamera").GetComponent<MinimapCamera>().target = gameObject;
            GameObject.Find("minimapCamera").GetComponent<MinimapCamera>().enabled = true;
            foreach (var c in gameObject.GetComponents<MonoBehaviour>())
                c.enabled = true;
        }
        if (isServer)
        {
            color = Color.HSVToRGB(Random.value, 1, 1);
        }
    }

    void OnChangeColor(Color color)
    {
        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var mr in mrs)
        {
            Material m = mr.material;
            if (m.name != "SkyCarBodyGrey (Instance)")
                continue;
            m.color = color;
            mr.material = m;
        }
    }

    public override void OnStartClient()
    {
        OnChangeColor(color);
    }
}
