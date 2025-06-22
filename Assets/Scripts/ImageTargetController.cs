using Vuforia;
using UnityEngine;

public class ImageTargetController : MonoBehaviour
{
    // Este evento se puede usar para notificar el comportamiento de los paneles
    public delegate void OnTargetFoundEvent(string targetName);
    public static event OnTargetFoundEvent OnTargetFound;

    public delegate void OnTargetLostEvent(string targetName);
    public static event OnTargetLostEvent OnTargetLost;

    // Suscribir al evento del DefaultObserverEventHandler
    public void OnTargetFoundMethod(string targetName)
    {
        // Notificar que el target ha sido encontrado
        OnTargetFound?.Invoke(targetName);
    }

    public void OnTargetLostMethod(string targetName)
    {
        // Notificar que el target ha sido perdido
        OnTargetLost?.Invoke(targetName);
    }
}
