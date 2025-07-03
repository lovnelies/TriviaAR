using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text finalText; // Referencia al TMP_Text "final"

    private void Start()
{
    // Detener el cronómetro
    if (QuizStuff.Instance != null)
    {
        QuizStuff.Instance.PauseTimer();
    }

    // Mostrar el puntaje y el tiempo
    if (QuizStuff.Instance != null)
    {
        int score = QuizStuff.Instance.GetScore();
        float elapsedTime = QuizStuff.Instance.GetElapsedTime();

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        finalText.text = $"Puntaje final: {score}\nTiempo: {timeString}";
    }
    else
    {
        //Debug.LogError("No se encontró la instancia de QuizStuff.");
    }
}
}