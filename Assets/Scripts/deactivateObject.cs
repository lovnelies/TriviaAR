using UnityEngine;
using Vuforia;

public class TargetStatusHandler : MonoBehaviour
{
    public GameObject myObject; // El objeto 3D que deseas activar/desactivar
    private ObserverBehaviour observerBehaviour;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            myObject.SetActive(true); // Activa el objeto cuando el target esté rastreado
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            myObject.SetActive(false); // Desactiva el objeto cuando se pierde el target
        }
    }
}
