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
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;
    public GameObject bottle;

    public GameObject briefcase;

    public GameObject clipboard;
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    public NPCFrompot frompots;
    public int npcvalue;
    // Start is called before the first frame update
    private void Start()
    {
        // This line gets your API key (and could be slightly different on Mac/Linux)
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
        frompots = FindObjectOfType<NPCFrompot>();
        if (frompots != null)
        {
        frompots.InitNPCPrompot();
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
        }
    }

    private void StartConversation()
    {
      
        messages = new List<ChatMessage> {
           new ChatMessage(ChatMessageRole.System, string.Join("", frompots.npcprompot) + npcvalue.ToString())
        };

        
        inputField.text = "";
        
       string startString = $"당신은 {frompots.npcname[npcvalue-1]}에게 질문을 하십시오";
        textField.text = startString;

        Debug.Log(startString);
    }

    private async void GetResponse()
    {
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
        if (userMessage.Content.Length > 200)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 200);
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
            Temperature = 0.9,
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
        TriggerAction(responseMessage.Content);
    
        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\n홍길동: {1}", userMessage.Content, responseMessage.Content);

        // Re-enable the OK button
        okButton.enabled = true;
    }

    private void TriggerAction(string responseMessage)
    {
        Debug.Log("Response Content: " + responseMessage);

    Item[] items = FindObjectsOfType<Item>();
    if(responseMessage.Contains("좋은")&&npcvalue==1){
        briefcase.SetActive(true);
        }
     if(responseMessage.Contains("근력")&&npcvalue==3){
        bottle.SetActive(true);
        }
     if(responseMessage.Contains("골다공증")&&npcvalue==4){
        clipboard.SetActive(true);
        }
    }
        
}
