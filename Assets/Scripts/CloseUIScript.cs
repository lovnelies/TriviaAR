namespace SampleScene
{
    using UnityEngine;

    public class CloseUIScript : MonoBehaviour
    {
        [SerializeField] GameObject UIElement; // Referencia al elemento de UI que se cerrará

        public void CloseUI()
        {
            // Cerrar el elemento de UI
            if (UIElement != null)
            {
                UIElement.SetActive(false);
                Debug.Log("Elemento de UI cerrado: " + UIElement.name);
            }
            else
            {
                Debug.LogError("No se asignó un elemento de UI en CloseUIScript.");
            }

            // Reanudar el cronómetro
            if (QuizStuff.Instance != null)
            {
                QuizStuff.Instance.ResumeTimer();
                Debug.Log("Cronómetro reanudado.");

                // Verificar la zona actual antes de generar una pista aleatoria
                if (QuizStuff.Instance.GetCurrentZone() != "Zona1")
                {
                    // Generar una pista aleatoria solo si no está en la Zona 1
                    QuizStuff.Instance.ShowRandomClue();
                    Debug.Log("Pista aleatoria generada.");
                }
                else
                {
                    Debug.Log("Estás en la Zona 1. No se generará una pista aleatoria.");
                }
            }
            else
            {
                Debug.LogError("No se encontró la instancia de QuizStuff.");
            }
        }
    }
}