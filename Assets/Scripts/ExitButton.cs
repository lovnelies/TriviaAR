using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Método para salir de la aplicación
    public void QuitApplication()
    {
        // Cierra la aplicación (solo funciona en builds compilados)
        Application.Quit();

        // Si estás en el Editor, muestra un mensaje en la consola
        #if UNITY_EDITOR
       
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el Editor
        #endif
    }
}