using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    public float delay = 5f;
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    public GameObject bottle;

    public GameObject briefcase;
    public Player player;
    public GameObject clipboard;
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    public NPCFrompot frompots;
    public int npcvalue;
    public TMP_Text mesh;
    private int check;
    // Start is called before the first frame update
    private void Start()
    {
        // This line gets your API key (and could be slightly different on Mac/Linux)
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
        
    }

    private void StartConversation()
    {
      
        messages = new List<ChatMessage> {
           new ChatMessage(ChatMessageRole.System, string.Join("", frompots.npcprompot) + npcvalue.ToString())
        };

        
        inputField.text = "";
        
       string startString = $"당신은 {frompots.npcname[npcvalue-1]}에게 질문을 하십시오";
        textField.text = startString;

        Debug.Log(npcvalue);
        check=npcvalue;
    }

    private async void GetResponse()
    {
        Debug.Log("start");
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the OK button
        okButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 300)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 300);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));
        
        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("You: {0}", userMessage.Content);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.3,
            MaxTokens = 50,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // Add the response to the list of messages
        messages.Add(responseMessage);
        
        // Trigger the desired action
        TriggerAction(responseMessage.Content,userMessage.Content);
        Debug.Log(frompots.npcname[npcvalue-1]);    
        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\n{1}: {2}", userMessage.Content,frompots.npcname[npcvalue-1], responseMessage.Content);

        // Re-enable the OK button
        okButton.enabled = true;
    }

    private void TriggerAction(string responseMessage,string userMessage)
    {
        Debug.Log("Response Content: " + responseMessage);

    Item[] items = FindObjectsOfType<Item>();

    if(responseMessage.Contains("근력")||responseMessage.Contains("아니")&&npcvalue==3){
        if(mesh.enabled==false) mesh.enabled=true;
        mesh.text=frompots.givehint[0];
        Invoke("HideText", delay);
                    
        }
     if(responseMessage.Contains("도박")&&npcvalue==4&&player.hasItems[1]){
            frompots.npcprompot[2]=frompots.npcwithevidence[1];
        }
        else{
            Debug.Log(responseMessage);
        }
        if(responseMessage.Contains("조명")&&npcvalue==3){
            frompots.npcprompot[3]=frompots.npcwithevidence[3];
        }
    }
     private void HideText()
    {
       mesh.enabled = false; // Text 요소를 비활성화하여 숨김
    }
}
