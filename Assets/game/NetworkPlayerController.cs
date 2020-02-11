using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    [SyncVar]
    WorldConfig config = WorldConfig.current;

    [SyncVar(hook = "OnChangeColor")]
    Color color;

    void Start()
    {
        if (isServer)
        {
            color = Color.HSVToRGB(Random.value, Random.value * 0.5f + 0.5f, Random.value * 0.5f + 0.5f);
        }

        if (isLocalPlayer)
        {
            Debug.Log("Initializing world '" + config.name + "'");

            // initialize the map
            GameObject map = FindObjectOfType<VtsMap>().gameObject;
            map.GetComponent<VtsMap>().Map.SetMapconfigPath(config.mapconfigUrl);
            {
                VtsMapMakeLocal l = map.GetComponent<VtsMapMakeLocal>();
                l.x = config.position[0];
                l.y = config.position[1];
                l.z = config.position[2];
            }

            // initialize the car
            GetComponent<VtsRigidBodyActivate>().map = map;
            GetComponent<VtsColliderProbe>().mapObject = map;
            GetComponent<VtsColliderProbe>().collidersLod = config.collisionlod;

            // initialize other objects
            GameObject.Find("mainCamera").GetComponent<FollowingCamera>().target = gameObject;
            GameObject.Find("mainCamera").GetComponent<FollowingCamera>().enabled = true;
            GameObject.Find("minimapCamera").GetComponent<MinimapCamera>().target = gameObject;
            GameObject.Find("minimapCamera").GetComponent<MinimapCamera>().enabled = true;
            GameObject.Find("sun").transform.rotation = Quaternion.Euler(VtsUtil.V2U3(config.sunDirection));

            // enable all components
            foreach (var c in gameObject.GetComponents<MonoBehaviour>())
                c.enabled = true;
        }
    }

    void OnChangeColor(Color color)
    {
        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var mr in mrs)
        {
            Material m = mr.material;
            if (m.name == "SkyCarBodyGrey (Instance)" || m.name == "MinimapIndicator (Instance)")
            {
                m.color = color;
                mr.material = m;
            }
        }
    }

    public override void OnStartClient()
    {
        OnChangeColor(color);
    }
}
