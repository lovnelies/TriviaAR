using UnityEngine;
using TMPro;
using UnityEngine.UI; // Necesario para usar RawImage
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent; // Referencia al componente TMP_Text
    public float delayBetweenCharacters = 0.1f; // Retraso entre cada carácter
    public float delayAfterComplete = 1f; // Retraso después de completar el texto
    public RawImage rawImageToShow; // Referencia a la RawImage que se mostrará al final
    public RawImage anotherImageToShow; // Referencia a la RawImage que se mostrará al final
    public TMP_Text textToShow; // Referencia a la RawImage que se mostrará al final
    public Button  botoncito;
    public GameObject  continuar;
    public GameObject  salir;
    private string fullText; // Texto completo a mostrar

     private void Start()
    {
        // Obtener el texto completo del componente TMP_Text
        fullText = textComponent.text;

        // Desactivar los botones al inicio
        if (continuar != null) continuar.SetActive(false);
        if (salir != null) salir.SetActive(false);

        // Iniciar el efecto typewriter
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        // Inicializar el texto vacío
        // Inicializar el texto vacío
        textComponent.text = "";

        // Mostrar el texto letra por letra
        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i]; // Añadir un carácter
            yield return new WaitForSeconds(delayBetweenCharacters); // Esperar antes de añadir el siguiente carácter
        }

        // Esperar un momento después de completar el texto
        yield return new WaitForSeconds(delayAfterComplete);

        // Reactivar los botones al finalizar el typewriter
        if (continuar != null) continuar.SetActive(true);
        if (salir != null) salir.SetActive(true);
            
        if (anotherImageToShow != null)
        {
            anotherImageToShow.gameObject.SetActive(true);
            Debug.Log("RawImage visible.");
        }
        else
        {
            Debug.LogError("No se asignó una RawImage en el Inspector.");
        }
if (textToShow != null)
        {
            textToShow.gameObject.SetActive(true);
            Debug.Log("RawImage visible.");
        }
        else
        {
            Debug.LogError("No se asignó una RawImage en el Inspector.");
        }

        // Mostrar la RawImage al finalizar el efecto typewriter
        if (rawImageToShow != null)
        {
            rawImageToShow.gameObject.SetActive(true);
            Debug.Log("RawImage visible.");
        }
        else
        {
            Debug.LogError("No se asignó una RawImage en el Inspector.");
        }
        if (textToShow != null)
        {
            textToShow.gameObject.SetActive(true);
            Debug.Log("TEXTO VISIBLE.");
        }
        else
        {
            Debug.LogError("No se asignó TEXTO.");
        }
        if (botoncito != null)
        {
            botoncito.gameObject.SetActive(true);
            Debug.Log("BOTON VISIBLE.");
        }
        else
        {
            Debug.LogError("No se asignó BOTON.");
        }
    }
}