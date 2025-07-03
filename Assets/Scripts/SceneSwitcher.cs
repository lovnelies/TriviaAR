using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        Debug.Log("Bot�n presionado, cargando escena: " + sceneIndex);
        // Cambia completamente a la nueva escena
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}