using UnityEngine;

public class Pinkify : MonoBehaviour
{
    public Material material;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (material.color.g > 0.5)
                material.color = new Color(255 / 255f, 105 / 255f, 180 / 255f);
            else
                material.color = new Color(1, 1, 1);
        }
    }
}
