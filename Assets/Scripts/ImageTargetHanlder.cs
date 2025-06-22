using UnityEngine;
using UnityEngine.UI;
using Vuforia;


public class ImageTargetHandler : MonoBehaviour
{
    // Arrastra tu Panel desde el Inspector
    public GameObject panel;

    private void Start()
    {
        // Asegúrate de que el panel esté oculto al iniciar
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    // Método para asignar en el evento OnTargetFound
    public void OnTargetFound()
    {
        if (panel != null)
        {
            panel.SetActive(true); // Muestra el panel
        }
    }

    // Método para asignar en el evento OnTargetLost
    public void OnTargetLost()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Oculta el panel
        }
    }
}
