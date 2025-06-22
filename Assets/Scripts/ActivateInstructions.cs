using UnityEngine;

public class ActivateInstructions : MonoBehaviour
{
    public GameObject instrucciones; // Asigna aquí el GameObject del panel en el Inspector

    void Start()
    {
        if (instrucciones != null)
        {
            // Activa el GameObject del panel
            instrucciones.SetActive(true);
        }
        else
        {
            Debug.LogWarning("El objeto 'Instrucciones' no está asignado en el Inspector.");
        }
    }
}
