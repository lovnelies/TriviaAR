using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels; // Array de paneles (Panel1, Panel2, Panel3)
    public Button continuar;    // Referencia al botón "Continuar"
    public Button salir;        // Referencia al botón "Salir"
    public TMP_Text continuarText; // Referencia al texto del botón "Continuar"

    private int currentPanelIndex = 0; // Índice del panel actual

    private void Start()
    {
        // Asegurarse de que solo el primer panel esté activo al inicio
        ActivatePanel(currentPanelIndex);

        // Configurar el botón "Continuar"
        if (continuar != null)
        {
            continuar.onClick.RemoveAllListeners();
            continuar.onClick.AddListener(OnContinueClicked);
        }

        // Configurar el botón "Salir"
        if (salir != null)
        {
            salir.onClick.RemoveAllListeners();
            salir.onClick.AddListener(OnExitClicked);
        }

        // Actualizar el texto del botón "Continuar" al inicio
        UpdateContinueButtonText();
    }

    // Método para manejar el clic en "Continuar"
    public void OnContinueClicked()
    {
        Debug.Log("Botón Continuar clickeado.");

        // Desactivar el panel actual
        DeactivatePanel(currentPanelIndex);

        // Pasar al siguiente panel
        currentPanelIndex++;

        Debug.Log("Índice del panel actual: " + currentPanelIndex);

        // Si hay más paneles, activar el siguiente
        if (currentPanelIndex < panels.Length)
        {
            ActivatePanel(currentPanelIndex);
        }
        else
        {
            Debug.Log("No hay más paneles. Cambiando a la escena Zona1.");
            // Cargar la escena Zona1
            SceneManager.LoadScene("Zona1");
        }

        // Actualizar el texto del botón "Continuar"
        UpdateContinueButtonText();
    }

    // Método para manejar el clic en "Salir"
    private void OnExitClicked()
    {
        Debug.Log("Salir de las instrucciones.");
        // Aquí puedes añadir lógica para cerrar las instrucciones o cambiar de escena
    }

    // Método para activar un panel específico
    private void ActivatePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(true);
            Debug.Log("Panel activado: " + panels[index].name);
        }
        else
        {
            Debug.LogError("Índice de panel fuera de rango: " + index);
        }
    }

    // Método para desactivar un panel específico
    private void DeactivatePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].SetActive(false);
            Debug.Log("Panel desactivado: " + panels[index].name);
        }
        else
        {
            Debug.LogError("Índice de panel fuera de rango: " + index);
        }
    }

    // Método para actualizar el texto del botón "Continuar"
    private void UpdateContinueButtonText()
    {
        if (continuarText != null)
        {
            // Cambiar el texto a "Empezar" si es el último panel
            if (currentPanelIndex == panels.Length - 1)
            {
                continuarText.text = "Empezar";
            }
            else
            {
                continuarText.text = "Continuar";
            }
        }
    }
}