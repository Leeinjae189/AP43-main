using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;
using TMPro;
public class NPCInteraction : MonoBehaviour
{
    public GameObject OpenAIController; // Open AI Controller 스크립트가 추가된 GameObject
    public GameObject uiObject; // 활성화할 UI Object
    public FirstPersonController anodl;
    private bool isInteractable = false; // 상호작용 가능한 상태인지를 나타내는 변수
    public TMP_InputField inputField;
    public GameObject Main;
    private bool contact=true;
    private void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E)&&contact)
        {
            ActivateInteraction();
        }

        if (isInteractable && Input.GetKeyDown(KeyCode.E)&& !contact)
        {
            ReActivateInteraction();
        }
    }

    private void ReActivateInteraction()
    {
        OpenAIController.GetComponent<OpenAIController>().enabled=false;
        uiObject.SetActive(false); // UI Object 활성화
        anodl.enabled=true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }

    private void ActivateInteraction()
    {
        OpenAIController.GetComponent<OpenAIController>().enabled=true;
        uiObject.SetActive(true); // UI Object 활성화
        anodl.enabled=false;
        inputField.Select();
       
        // 여기에 추가적인 상호작용 로직을 구현할 수 있습니다.
    }
}
