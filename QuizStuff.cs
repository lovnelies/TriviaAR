using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class quizStuff : MonoBehaviour
{
    public TMP_Text questionText; // Texto de la pregunta
    public TMP_Text[] answerTexts; // Array de textos para los botones de respuesta (A, B, C)
    public Button nextButton; // Botón para continuar

    // Diccionario para asociar preguntas con ImageTargets
    private Dictionary<string, quizQuestions> questionDictionary = new Dictionary<string, quizQuestions>();

    private string currentTarget = ""; // Nombre del ImageTarget detectado
    private int score = 0;
    private List<Button> disabledButtons = new List<Button>(); // Lista para almacenar los botones desactivados

    void Start()
    {
        Debug.Log("Inicializando preguntas...");

        // Inicializa las preguntas directamente en el script
        questionDictionary.Add("ImageTarget1", new quizQuestions
        {
            Q = "¿Cual es la capital de Francia?",
            Answers = new string[] { "Paris", "Seul", "Santiago" },
            CorrectAnswer = "Paris"
        });

        questionDictionary.Add("ImageTarget2", new quizQuestions
        {
            Q = "¿Cuanto es 5 + 3?",
            Answers = new string[] { "8", "3", "1" },
            CorrectAnswer = "8"
        });

        Debug.Log("Preguntas inicializadas: " + questionDictionary.Count);

        nextButton.onClick.AddListener(NextClick); // Configura el botón "Next"
        nextButton.interactable = false; // Desactiva el botón "Next" al inicio

        ClearPanel(); // Limpia el panel al inicio
    }

    public void OnTargetFound(string targetName)
    {
        Debug.Log("ImageTarget detectado: " + targetName);

        if (questionDictionary.ContainsKey(targetName))
        {
            currentTarget = targetName;
            Debug.Log("Pregunta asociada encontrada para: " + targetName);
            UpdateQuestion(targetName);
        }
        else
        {
            Debug.LogWarning("No hay una pregunta asociada para el ImageTarget: " + targetName);
        }
    }

    public void OnTargetLost()
    {
        Debug.Log("ImageTarget perdido");
        currentTarget = "";
        ClearPanel();
    }

    void UpdateQuestion(string targetName)
    {
        if (!questionDictionary.ContainsKey(targetName))
        {
            Debug.LogError("Error: No se encontró la pregunta para el target " + targetName);
            return;
        }

        var questionData = questionDictionary[targetName];
        questionText.text = questionData.Q;

        // Actualiza los textos de los botones con las respuestas
        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < questionData.Answers.Length)
            {
                answerTexts[i].text = questionData.Answers[i];
                answerTexts[i].transform.parent.gameObject.SetActive(true); // Asegura que el botón esté activo
            }
            else
            {
                answerTexts[i].transform.parent.gameObject.SetActive(false); // Desactiva botones no usados
            }
        }

        Debug.Log("Pregunta mostrada: " + questionData.Q);
        Debug.Log("Respuestas mostradas: " + string.Join(", ", questionData.Answers));

        nextButton.interactable = false; // Desactiva el botón "Next" hasta que se valide la respuesta

        disabledButtons.Clear(); // Limpiar lista de botones desactivados al mostrar una nueva pregunta
    }

    void ClearPanel()
    {
        Debug.Log("Limpiando panel...");
        questionText.text = "";

        foreach (var answerText in answerTexts)
        {
            answerText.text = "";
        }

        foreach (var text in answerTexts)
        {
            text.transform.parent.gameObject.SetActive(false); // Desactiva los botones
        }

        nextButton.interactable = false;
        disabledButtons.Clear(); // Limpiar lista de botones desactivados
    }

    public void AnswerClick(TMP_Text clickedText)
    {
        if (currentTarget != "" && questionDictionary.ContainsKey(currentTarget))
        {
            var questionData = questionDictionary[currentTarget];
            string selectedAnswer = clickedText.text;

            Debug.Log($"Respuesta seleccionada: {selectedAnswer}");
            Debug.Log($"Respuesta correcta: {questionData.CorrectAnswer}");

            Button clickedButton = clickedText.transform.parent.GetComponent<Button>();

            // Cambia el color del botón según si la respuesta es correcta o incorrecta
            if (selectedAnswer == questionData.CorrectAnswer)
            {
                score += 10;
                Debug.Log($"Respuesta correcta. Puntuación actual: {score}");
                clickedButton.image.color = Color.green; // Respuesta correcta, verde
                nextButton.interactable = true; // Activa el botón "Next"
            }
            else
            {
                Debug.Log("Respuesta incorrecta. Penalización de 5 segundos.");
                clickedButton.image.color = Color.red; // Respuesta incorrecta, roja
                disabledButtons.Add(clickedButton); // Añadir a la lista de botones desactivados
                StartCoroutine(ApplyPenaltyAndReactivateButtons());
                nextButton.interactable = false;
            }

            // Desactiva todos los botones de respuesta
            SetAnswerButtonsInteractable(false);
        }
        else
        {
            Debug.LogWarning("No hay un ImageTarget activo o la pregunta no está registrada.");
        }
    }

    private IEnumerator ApplyPenaltyAndReactivateButtons()
    {
        Debug.Log("Penalización activa. Esperando 5 segundos...");
        yield return new WaitForSeconds(5); // Espera 5 segundos
        Debug.Log("Penalización finalizada. Reactivando botones...");

        // Reactiva los botones de respuesta que no han sido presionados
        foreach (var answerText in answerTexts)
        {
            Button answerButton = answerText.transform.parent.GetComponent<Button>();
            if (!disabledButtons.Contains(answerButton)) // Solo reactiva los que no fueron desactivados
            {
                answerButton.interactable = true;
                answerButton.image.color = Color.white; // Restaura el color original
            }
        }
    }

    void SetAnswerButtonsInteractable(bool state)
    {
        foreach (var answerText in answerTexts)
        {
            answerText.transform.parent.GetComponent<Button>().interactable = state;
        }
    }

    public void NextClick()
    {
        Debug.Log("Botón 'Next' clicado. Preparando para la siguiente pregunta...");
        ClearPanel();
        foreach (var answerText in answerTexts)
        {
            answerText.transform.parent.GetComponent<Button>().image.color = Color.white; // Restaurar color original (blanco)
        }
    }

    // Clase para manejar datos de preguntas y respuestas
    [System.Serializable]
    public class quizQuestions
    {
        public string Q; // Pregunta
        public string[] Answers; // Array de respuestas posibles (A, B, C)
        public string CorrectAnswer; // Respuesta correcta
    }
}
