using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextManager : MonoBehaviour
{
    private TMP_Text TextBox;

    //Speed between char being added to text
    public float LetterDelay;

    //Speed between char being removed from text
    public float RemoveDelay;
    char[] Letters;

    struct DialogueRequest {
      public float StartDelay;
      public float ReadTime;
      public string Dialogue;
    }

    List<DialogueRequest> Stack = new List<DialogueRequest>();

    void Start()
    {
        TextBox = this.gameObject.GetComponent<TextMeshProUGUI>();
        TextBox.text = "";
    }

    public void TextRequest(float StartDelay, string Dialogue, float ReadTime)
    {
        DialogueRequest NewRequest = new DialogueRequest();
        NewRequest.StartDelay = StartDelay;
        NewRequest.Dialogue = Dialogue;
        NewRequest.ReadTime = ReadTime;
        Stack.Add(NewRequest);

        // If any longer than its already running
        if(Stack.Count == 1) {
          Letters = Stack[0].Dialogue.ToCharArray();
          StartCoroutine("AddChar",StartDelay);
        }
    }

    int OnLetter = 0;

    //Adding text
    public void NextCharacter()
    {
        TextBox.text += Letters[OnLetter];
        OnLetter++;
        if(OnLetter > Letters.Length - 1)
        {
          StartCoroutine("RemoveChar",Stack[0].ReadTime);
          Stack.Remove(Stack[0]);
          return;
        }
        StartCoroutine("AddChar",LetterDelay);
    }

    private IEnumerator AddChar(float t)
    {
        yield return new WaitForSeconds(t);
        NextCharacter();
    }
    //Removing Text
    public void NextCharacterRemove()
    {
      TextBox.text = TextBox.text.Remove(OnLetter-1);
      OnLetter--;
      if(OnLetter == 0)
      {
        if(Stack.Count > 0)
        {
          Letters = Stack[0].Dialogue.ToCharArray();
          StartCoroutine("AddChar",Stack[0].StartDelay);
        }
        return;
      }
      StartCoroutine("RemoveChar",RemoveDelay);
    }

    private IEnumerator RemoveChar(float t)
    {
        yield return new WaitForSeconds(t);
        NextCharacterRemove();
    }
}
