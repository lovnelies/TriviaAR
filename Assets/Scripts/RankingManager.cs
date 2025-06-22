using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text rankingText;
    public TMP_Text playerStatsText;
    public Button playAgainButton;
    public Button exitButton;
    public GameObject loadingPanel;
    public RectTransform contentTransform; 

    
    void Start()
    {
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
            
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
        
        ShowPlayerStats();
        LoadAndDisplayRanking();
    }
    
    void ShowPlayerStats()
    {
        if (playerStatsText != null && QuizStuff.Instance != null)
        {
            string playerName = FirebaseManager.Instance.GetPlayerName();
            int score = QuizStuff.Instance.GetScore();
            float time = QuizStuff.Instance.GetElapsedTime();
            
            playerStatsText.text = $"¡Felicidades {playerName}!\n" +
                                 $"Puntaje Final: {score}\n" +
                                 $"Tiempo: {FormatTime(time)}";
        }
    }
    
    void LoadAndDisplayRanking()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
            
        FirebaseManager.Instance.LoadRankings(OnRankingLoaded);
    }
    
    void OnRankingLoaded(List<PlayerGameData> rankings)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
            
        DisplayRanking(rankings);
    }
    
    void DisplayRanking(List<PlayerGameData> rankings)
    {
        if (rankingText == null) return;
        string currentPlayerName = FirebaseManager.Instance.GetPlayerName();
        
        string rankingDisplay = "🏆 TOP 10 RANKING 🏆\n\n";
        
        for (int i = 0; i < rankings.Count; i++)
        {
            PlayerGameData player = rankings[i];
            string medal = "";
            
            switch (i)
            {
                case 0: medal = "🥇"; break;
                case 1: medal = "🥈"; break;
                case 2: medal = "🥉"; break;
                default: medal = $"{i + 1}."; break;
            }
            
            string line = string.Format("{0} {1,-10} {2,5} pts - {3}\n",
    medal,
    player.nombre,
    player.puntaje,
    FormatTime(player.tiempo));

if (player.nombre == currentPlayerName)
{
    // Agrega ★ y aplica color dorado al jugador actual
    line = $"<color=#FFD700>{medal} ★ {player.nombre,-10} {player.puntaje,5} pts - {FormatTime(player.tiempo)}</color>\n";
}

rankingDisplay += line;

        }
        
        if (rankings.Count == 0)
        {
            rankingDisplay += "No hay datos disponibles";
        }
        
        rankingText.text = rankingDisplay;
        // Forzar un resize basado en el texto
LayoutRebuilder.ForceRebuildLayoutImmediate(rankingText.rectTransform);

// Ajusta el alto del Content para que el Scroll funcione bien
float newHeight = rankingText.preferredHeight + 20f; // un poco de margen
contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);

    }
    
    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void PlayAgain()
    {
        // Reiniciar el juego
        SceneManager.LoadScene("NameScene");
    }
    
    public void ExitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}