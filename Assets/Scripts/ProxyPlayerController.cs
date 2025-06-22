using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ProxyPlayerController : MonoBehaviour
{    
    public void HandleImage(string target) {
        QuizStuff.Instance.OnTargetFound(target);
    }
    
    public void HandleButton(TMP_Text clickedText){
        QuizStuff.Instance.AnswerClick(clickedText);
    }
    public void HandleScore(TMP_Text scoreText){
        QuizStuff.Instance.AnswerClick(scoreText);
    }
   public void HandlePenaltyText(TMP_Text penaltyText) {
    QuizStuff.Instance.UpdatePenaltyText(penaltyText);
}
 public float GetElapsedTime(TMP_Text timerText)
    {
        return QuizStuff.Instance.GetElapsedTime();
    }
    
}

