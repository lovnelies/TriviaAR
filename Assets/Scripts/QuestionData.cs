using System;
using System.Collections.Generic;

[Serializable]
public class QuestionData
{
    public string targetId;
    public string zone;
    public string question;
    public string[] answers;
    public string correctAnswer;
}

[Serializable]
public class QuizDataContainer
{
    public QuestionData[] questions;
    public ZoneClues zoneClues;
}

[Serializable]
public class ZoneClues
{
    public string[] Zona1;
    public string[] Zona2;
    public string[] Zona3;
    public string[] Zona4;
    
    public Dictionary<string, List<string>> ToDictionary()
    {
        return new Dictionary<string, List<string>>
        {
            { "Zona1", new List<string>(Zona1) },
            { "Zona2", new List<string>(Zona2) },
            { "Zona3", new List<string>(Zona3) },
            { "Zona4", new List<string>(Zona4) }
        };
    }
}