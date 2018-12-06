using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class QuitFromGame : MonoBehaviour
{
    void Start()
    {
        if (!FindObjectOfType<NetworkManager>())
            SceneManager.LoadScene(0); // go back to menu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
