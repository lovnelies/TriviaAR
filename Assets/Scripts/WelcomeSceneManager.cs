using UnityEngine;
using TMPro;

public class WelcomeSceneManager : MonoBehaviour
{
    public TMP_Text lastScoreText; // Referencia al TMP_Text para el último puntaje
    public TMP_Text lastTimeText;  // Referencia al TMP_Text para el último tiempo

    private void Start()
    {
        // Cargar el último puntaje y tiempo desde PlayerPrefs
        int lastScore = PlayerPrefs.GetInt("PlayerScore", 0);
        float lastTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);

        // Formatear el tiempo en minutos y segundos
        int minutes = Mathf.FloorToInt(lastTime / 60f);
        int seconds = Mathf.FloorToInt(lastTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Actualizar los textos con los valores cargados
        if (lastScoreText != null)
        {
            lastScoreText.text = $"Último puntaje: {lastScore}";
        }

        if (lastTimeText != null)
        {
            lastTimeText.text = $"Último tiempo: {timeString}";
        }
    }
}