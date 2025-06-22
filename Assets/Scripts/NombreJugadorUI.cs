using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NameSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;
    public Button startGameButton;
    public TMP_Text errorText;
    
    void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }
        
        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
        
        // Asegurar que FirebaseManager existe
        if (FirebaseManager.Instance == null)
        {
            GameObject firebaseManagerGO = new GameObject("FirebaseManager");
            firebaseManagerGO.AddComponent<FirebaseManager>();
        }
    }
    
   public void StartGame()
{
    string playerName = nameInputField.text.Trim();

    if (string.IsNullOrEmpty(playerName))
    {
        ShowError("Por favor, ingresa tu nombre");
        return;
    }

    if (playerName.Length < 2)
    {
        ShowError("El nombre debe tener al menos 2 caracteres");
        return;
    }

    startGameButton.interactable = false;  // Deshabilitar botón mientras se guarda

    FirebaseManager.Instance.SavePlayerNameToFirebase(playerName, success =>
    {
        if (success)
        {
            // Nombre guardado correctamente, cargar siguiente escena
            SceneManager.LoadScene("Zona1");
        }
        else
        {
            ShowError("Error al guardar el nombre, intenta de nuevo");
            startGameButton.interactable = true;  // Reactivar botón para intentar de nuevo
        }
    });
}




void LoadGameScene()
{
    SceneManager.LoadScene("Zona1");
}

    
    void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            
            // Ocultar el error después de 3 segundos
            Invoke("HideError", 3f);
        }
    }
    
    void HideError()
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }
}
