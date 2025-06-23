using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using Vuforia;
using System.IO;

public class QuizStuff : MonoBehaviour
{
    public static QuizStuff Instance;
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;
    public TMP_Text timerText;

    private int attempts = 0;
    public TMP_Text questionText;
    public TMP_Text[] answerTexts;
    public Button nextButton;
    public TMP_Text clueText;
    public GameObject cameraObject;
    public TMP_Text penaltyText;
    public TMP_Text scoreText;
    
    // Diccionarios para los datos del juego
    private Dictionary<string, QuestionData> questionDictionary = new Dictionary<string, QuestionData>();
    private Dictionary<string, string> qrToZoneDictionary = new Dictionary<string, string>();
    private Dictionary<string, List<string>> zoneQRs = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> scannedQRs = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> zoneClues = new Dictionary<string, List<string>>();
    
    public GameObject gifObject;
    private string currentTarget = "";
    private string currentZone = "";
    private int score = 0;
    private List<Button> disabledButtons = new List<Button>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayerPrefs.DeleteKey("PlayerScore");
            score = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save();
    }

    public string GetCurrentZone()
    {
        return currentZone;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PauseTimer();
        score = PlayerPrefs.GetInt("PlayerScore", 0);

        GameObject scoreObject = GameObject.Find("puntaje");
        if (scoreObject != null)
        {
            scoreText = scoreObject.transform.Find("scoreText")?.GetComponent<TMP_Text>();
            if (scoreText != null)
            {
                UpdateScoreUI();
            }
        }

        FindUIElements();
    }

    void FindUIElements()
    {
        GameObject panel = GameObject.Find("Panel"); 
        if (panel == null)
        {
            return;
        }
        penaltyText = panel.transform.Find("incorrect")?.GetComponent<TMP_Text>();

        GameObject canvasXD = GameObject.Find("XD");
        if (canvasXD != null)
        {
            gifObject = canvasXD.transform.Find("gif")?.gameObject;
            if (gifObject != null)
            {
                gifObject.SetActive(false);
            }
        }

        questionText = panel.transform.Find("Q")?.GetComponent<TMP_Text>();
        nextButton = panel.transform.Find("next")?.GetComponent<Button>();
        clueText = panel.transform.Find("ClueText")?.GetComponent<TMP_Text>();

        string[] buttonNames = { "A", "B", "C" };
        answerTexts = new TMP_Text[buttonNames.Length];

        for (int i = 0; i < buttonNames.Length; i++)
        {
            Transform button = panel.transform.Find(buttonNames[i]);
            if (button != null)
            {
                answerTexts[i] = button.Find("AnswerText" + (i + 1))?.GetComponent<TMP_Text>();
            }
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextClick);
            nextButton.interactable = false;
        }

        Transform borders = GameObject.Find("Borders")?.transform;
        if (borders != null)
        {
            Transform pista = borders.Find("pista");
            if (pista != null)
            {
                clueText = pista.Find("clueText")?.GetComponent<TMP_Text>();
            }
            Transform puntaje = borders.Find("puntaje");
            if (puntaje != null)
            {
                scoreText = puntaje.Find("scoreText")?.GetComponent<TMP_Text>();
                if (scoreText != null)
                {
                    UpdateScoreUI();
                }
            }
            Transform rawImage = borders.Find("RawImage");
            if (rawImage != null)
            {
                timerText = rawImage.Find("timerText")?.GetComponent<TMP_Text>();
                if (timerText != null)
                {
                    UpdateTimerUI();
                }
            }
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Zona1")
        {
            OnTargetFound("ImageTarget1");
        }

        LoadQuizData();
        
        nextButton?.onClick.AddListener(NextClick);
        if (nextButton != null)
            nextButton.interactable = false;
        UpdateScoreUI();
        ClearPanel();
    }

    private void LoadQuizData()
    {
        try
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "questions.json");
            string jsonContent = "";

            // Verificar si es Android
            if (Application.platform == RuntimePlatform.Android)
            {
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
                www.SendWebRequest();
                while (!www.isDone) { }
                jsonContent = www.downloadHandler.text;
            }
            else
            {
                jsonContent = File.ReadAllText(filePath);
            }

            QuizDataContainer quizData = JsonUtility.FromJson<QuizDataContainer>(jsonContent);
            
            // Cargar preguntas
            foreach (QuestionData question in quizData.questions)
            {
                questionDictionary.Add(question.targetId, question);
                qrToZoneDictionary.Add(question.targetId, question.zone);
            }

            // Organizar QRs por zona
            OrganizeQRsByZone();
            
            // Inicializar listas de QRs escaneados
            InitializeScannedQRs();
            
            // Cargar pistas
            zoneClues = quizData.zoneClues.ToDictionary();

            Debug.Log("Datos del quiz cargados exitosamente desde JSON");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar datos del quiz: " + e.Message);
            // Fallback: cargar datos hardcodeados si el JSON falla
            LoadHardcodedData();
        }
    }

    private void OrganizeQRsByZone()
    {
        zoneQRs.Clear();
        
        foreach (var kvp in qrToZoneDictionary)
        {
            string targetId = kvp.Key;
            string zone = kvp.Value;
            
            if (!zoneQRs.ContainsKey(zone))
            {
                zoneQRs[zone] = new List<string>();
            }
            
            zoneQRs[zone].Add(targetId);
        }
    }

    private void InitializeScannedQRs()
    {
        scannedQRs.Clear();
        scannedQRs.Add("Zona1", new List<string>());
        scannedQRs.Add("Zona2", new List<string>());
        scannedQRs.Add("Zona3", new List<string>());
        scannedQRs.Add("Zona4", new List<string>());
    }

    public void UpdatePenaltyText(TMP_Text penaltyText) {
        penaltyText.text = "Nuevo texto de penalización";
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save();
    }

    public void OnTargetFound(string targetName)
    {
        PauseTimer();
        if (questionDictionary.ContainsKey(targetName))
        {
            currentTarget = targetName;
            currentZone = qrToZoneDictionary[targetName];
            UpdateQuestion(targetName);

            ObserverBehaviour[] allObservers = FindObjectsOfType<ObserverBehaviour>();
            foreach (ObserverBehaviour observer in allObservers)
            {
                observer.enabled = false;
            }

            if (!scannedQRs[currentZone].Contains(targetName))
            {
                scannedQRs[currentZone].Add(targetName);
            }
        }
    }

    public void OnTargetLost()
    {
        currentTarget = "";
        ClearPanel();
    }

    void UpdateQuestion(string targetName)
    {
        if (!questionDictionary.ContainsKey(targetName))
        {
            return;
        }

        attempts = 0;

        var questionData = questionDictionary[targetName];
        questionText.text = questionData.question;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i < questionData.answers.Length)
            {
                answerTexts[i].text = questionData.answers[i];
                answerTexts[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                answerTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }

        nextButton.interactable = false;
        disabledButtons.Clear();
    }

    void ClearPanel()
    {
        if (questionText != null)
            questionText.text = "";

        if (answerTexts != null)
        {
            foreach (var answerText in answerTexts)
            {
                if (answerText != null)
                    answerText.text = "";
            }

            foreach (var text in answerTexts)
            {
                if (text != null && text.transform.parent != null)
                    text.transform.parent.gameObject.SetActive(false);
            }
        }

        if (nextButton != null)
            nextButton.interactable = false;

        disabledButtons.Clear();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = ":" + score.ToString();
        }
    }

    public void AnswerClick(TMP_Text clickedText)
    {
        if (currentTarget != "" && questionDictionary.ContainsKey(currentTarget))
        {
            var questionData = questionDictionary[currentTarget];
            string selectedAnswer = clickedText.text;

            Button clickedButton = clickedText.transform.parent.GetComponent<Button>();

            if (selectedAnswer == questionData.correctAnswer)
            {
                if (attempts == 0)
                {
                    score += 15;
                }
                else if (attempts == 1)
                {
                    score += 10;
                }
                else if (attempts == 2)
                {
                    score += 5;
                }

                clickedButton.image.color = Color.green;
                nextButton.interactable = true;
                ShowRandomClue();
                targetToDisable = currentTarget;
                UpdateScoreUI();
                attempts = 0;
            }
            else
            {
                attempts++;
                ResumeTimer();
                clickedButton.image.color = Color.red;
                clickedButton.interactable = false;
                disabledButtons.Add(clickedButton);
                StartCoroutine(ApplyPenaltyAndReactivateButtons());
                nextButton.interactable = false;
            }

            SetAnswerButtonsInteractable(false);
        }
    }

    void ApplyPenalty()
    {
        StartCoroutine(ActivateGifForPenalty());
    }

    IEnumerator ActivateGifForPenalty()
    {
        if (gifObject != null)
        {
            gifObject.SetActive(true);
            yield return new WaitForSeconds(5);
            gifObject.SetActive(false);
        }
    }

    private IEnumerator ApplyPenaltyAndReactivateButtons()
    {
        ResumeTimer();
        if (penaltyText != null)
        {
            penaltyText.gameObject.SetActive(true);
        }

        for (int i = 5; i > 0; i--)
        {
            if (penaltyText != null)
            {
                penaltyText.text = $"Penalización activa: {i} segundos";
            }
            yield return new WaitForSeconds(1);
        }

        if (penaltyText != null)
        {
            penaltyText.gameObject.SetActive(false);
            PauseTimer();
        }

        foreach (var answerText in answerTexts)
        {
            Button answerButton = answerText.transform.parent.GetComponent<Button>();
            if (!disabledButtons.Contains(answerButton))
            {
                answerButton.interactable = true;
                answerButton.image.color = Color.white;
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

    private string targetToDisable = "";
    public void NextClick()
    {
        ResumeTimer();
        ObserverBehaviour[] allObservers = FindObjectsOfType<ObserverBehaviour>();
        foreach (ObserverBehaviour observer in allObservers)
        {
            observer.enabled = true;
        }

        if (!string.IsNullOrEmpty(targetToDisable))
        {
            DisableImageTarget(targetToDisable);
            targetToDisable = "";
        }

        ClearPanel();
        foreach (var answerText in answerTexts)
        {
            Button answerButton = answerText.transform.parent.GetComponent<Button>();
            answerButton.interactable = true;
            answerButton.image.color = Color.white;
        }

        CheckIfZoneCompleted();
    }

    void DisableImageTarget(string targetName)
    {
        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    public void ShowRandomClue()
    {
        if (clueText == null)
        {
            return;
        }

        if (zoneClues.ContainsKey(currentZone))
        {
            List<string> clues = zoneClues[currentZone];

            if (currentZone == "Zona1" && clues.Count == 1)
            {
                string lastClue = "Pista: Se puede realizar un recital al aire libre";
                clueText.text = lastClue;
                clues.RemoveAt(0);
            }
            else if (clues.Count > 0)
            {
                string randomClue;
                if (currentZone == "Zona1")
                {
                    List<string> tempClues = new List<string>(clues);
                    tempClues.Remove("Pista: Se puede realizar un recital al aire libre");
                    int randomIndex = Random.Range(0, tempClues.Count);
                    randomClue = tempClues[randomIndex];
                    clues.Remove(randomClue);
                }
                else
                {
                    int randomIndex = Random.Range(0, clues.Count);
                    randomClue = clues[randomIndex];
                    clues.RemoveAt(randomIndex);
                }

                clueText.text = randomClue;
            }
            else
            {
                clueText.text = "¡Has completado todas las pistas de esta zona!";
            }
        }
    }

    void CheckIfZoneCompleted()
    {
        if (clueText == null)
        {
            return;
        }

        if (zoneQRs.ContainsKey(currentZone))
        {
            List<string> qrsInZone = zoneQRs[currentZone];
            List<string> scannedInZone = scannedQRs[currentZone];

            if (scannedInZone.Count == qrsInZone.Count)
            {
                clueText.text = $"¡Zona {currentZone} completada! Dirígete a la siguiente zona.";
                MoveToNextZone();
            }
        }
    }

    public int GetScore()
    {
        return score;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    void MoveToNextZone()
    {
        SaveScore();

        if (currentZone == "Zona1")
        {
            currentZone = "Zona2";
            SceneManager.LoadScene("Zona2");
        }
        else if (currentZone == "Zona2")
        {
            currentZone = "Zona3";
            SceneManager.LoadScene("Zona3");
        }
        else if (currentZone == "Zona3")
        {
            currentZone = "Zona4";
            SceneManager.LoadScene("Zona4");
        }
        else if (currentZone == "Zona4")
        {
            PauseTimer();
            
            if (FirebaseManager.Instance != null)
            {
                FirebaseManager.Instance.SavePlayerData(score, elapsedTime);
            }
            
            currentZone = "Zona4";
            SceneManager.LoadScene("ranking");
        }

        if (currentZone != "ranking" && scannedQRs.ContainsKey(currentZone))
        {
            scannedQRs[currentZone].Clear();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveScore();
            if (FirebaseManager.Instance != null)
            {
                FirebaseManager.Instance.SavePlayerData(score, elapsedTime);
            }
        }
    }
}