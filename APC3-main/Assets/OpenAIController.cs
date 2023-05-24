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

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    // Start is called before the first frame update
    void Start()
    {
        // This line gets your API key (and could be slightly different on Mac/Linux)
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "참가자들은 우승자가 되면 거액의 상금을 받을 수 있다는 의문의 초대장을 받고 게임에 참가한다.게임은 무도회장에서 가면무도회 컨셉으로 진행되고 참가자들은 가면을 쓰고 정체를 숨긴다.자정이 되자 종소리가 울리고 도우미들은 참가자들에게 게임의 룰이 적힌 종이를 배부한다.참가자 중 정체를 숨긴 불청객을 찾아낸 참가자들의 수에 따라 분할된 상금이 지급되고, 만약 찾지 못하면 불청객이 상금을 차지한다는 것이 이 게임의 규칙이다.다같이 종이를 읽던 중에 음악이 끊기고 조명이 꺼진다. 다시 조명이 켜지자 참가자 중 하나였던 남자가 유리병에 머리를 맞아 피를 흘리는 시체로 발견된다.참가자들은 놀라서 출입문을 향해 도망가지만 모든 문은 폐쇄됐다.결국 참가자들은 불청객을 찾기 위해 주변을 탐문하기로 한다. 참가자는 총 6명으로 주인공, 홍길동, 이훈이, 박철수, 박맹구, 봉미선이고 서로 처음 보는 사이이다. 당신은 그중 홍길동입니다.l 홍길동은 25세 여성으로 취직을 준비하면서 생활비를 벌기위해 매일 튼튼병원 근처에 있는 편의점에서 일하는 게 힘들어서 게임에 참가했다. 홍길동은 김영희가 편의점을 자주 들리자 얘기를 나누면서 김영희가 입원한 이유와 병실 호수를 알게 되었다. 홍길동은 차분하고 예의바르다. 홍길동은 이미 주어진 설정 이외의 질문을 받으면 잘 모르겠다고 대답한다.")
        };

        inputField.text = "";
        string startString = "당신은 홍길동에게 질문을 하십시오";
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
        
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);
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

        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\nGuard: {1}", userMessage.Content, responseMessage.Content);

        // Re-enable the OK button
        okButton.enabled = true;
    }
}
