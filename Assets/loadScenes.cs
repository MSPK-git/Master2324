using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("BreakdanceScene", LoadSceneMode.Additive);
    }
}