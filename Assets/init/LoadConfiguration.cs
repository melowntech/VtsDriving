using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// these structs emit warnings that some fields are not being used, however they are used in the serialization
#pragma warning disable

[Serializable]
public class WorldConfig
{
    public string name;
    public string mapconfigUrl;
    public double[] position; // long, lat, alt
    public float[] sunDirection;
    public uint collisionlod;

    public static WorldConfig current;
}

[Serializable]
internal class WorldsCollection
{
    public List<WorldConfig> worlds;

    public static WorldsCollection current;
}

#pragma warning restore

public class LoadConfiguration : MonoBehaviour
{
    public TextAsset worldsConfig;

    Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.options.Clear();
        WorldConfig.current = null;
        WorldsCollection.current = JsonUtility.FromJson<WorldsCollection>(worldsConfig.text);
        foreach (WorldConfig w in WorldsCollection.current.worlds)
        {
            Dropdown.OptionData item = new Dropdown.OptionData(w.name);
            dropdown.options.Add(item);
        }
        OnSelect();
    }

    public void OnSelect()
    {
        WorldConfig.current = WorldsCollection.current.worlds[dropdown.value];
    }
}
