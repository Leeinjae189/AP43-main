using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldMouseOver : MonoBehaviour, IPointerClickHandler
{
    public TMP_InputField inputField;

   public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 시 실행될 동작
        inputField.text = "클릭한 텍스트";
    }
}
