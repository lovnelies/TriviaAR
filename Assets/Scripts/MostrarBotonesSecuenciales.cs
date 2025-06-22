using UnityEngine;
using System.Collections;

public class MostrarBotonesSecuenciales : MonoBehaviour
{
    public Animator[] botonesAnimators; // Array de Animators de los botones
    public float delayEntreBotones = 1f; // Retraso entre la animación de cada botón

    private void Start()
    {
        // Asegurarse de que todos los botones estén ocultos al inicio
        foreach (Animator animator in botonesAnimators)
        {
            animator.gameObject.SetActive(false);
        }

        // Iniciar la secuencia de animaciones
        StartCoroutine(MostrarBotones());
    }

    private IEnumerator MostrarBotones()
    {
        foreach (Animator animator in botonesAnimators)
        {
            // Activar el botón
            animator.gameObject.SetActive(true);

            // Reproducir la animación
            animator.Play("BotonAparece");

            // Esperar a que termine la animación
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Esperar un momento antes de mostrar el siguiente botón
            yield return new WaitForSeconds(delayEntreBotones);
        }
    }
}