using UnityEngine;
using UnityEngine.UI;
using Vuforia;


public class ImageTargetHandler : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void OnTargetFound()
    {
        if (panel != null)
        {
            panel.SetActive(true); 
        }
    }

    public void OnTargetLost()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Oculta el panel
        }
    }
}
