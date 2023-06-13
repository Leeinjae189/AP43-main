using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcCheckAnswer : MonoBehaviour
{
    public float delay = 5f;
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
   

   
    private void Start() {
        textField.text="탐정님이 생각하시는 범인은 누구신가요?";
        okButton.onClick.AddListener(() => CheckAnswer());
    }
    
    private void CheckAnswer()
    {
        if (inputField.text.Contains("김철수")){
            textField.text="정답입니다! 탐정님 덕분에 범인을 잡을 수 있었습니다. 수사협조 감사합니다!";
        }
        else textField.text="확실하신가요? 탐정님?";
    }

    private void HideText()
    {
      
    }
}
