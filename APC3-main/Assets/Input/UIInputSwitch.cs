using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputSwitch : MonoBehaviour
{
    public event Action OnUICanceled;

    /// <summary>
    /// <see langword="true"/> 이면 사용자가 UI 모드에서 "Swich to Player"에 할당된 키를 눌렀을때 자동으로 플레이 모드로 전환한다.
    /// </summary>
    [SerializeField] private bool autoSwitchOnCanceled = false; 

    private static PlayerInput playerInput;
    private static StarterAssetsInputs assetInputs;

    private void Init()
    {
        if(playerInput == null || assetInputs == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            playerInput = player.GetComponent<PlayerInput>();
            assetInputs = player.GetComponent<StarterAssetsInputs>();
        }
    }


    void OnEnable()
    {
        Init();
        SwitchToUIActionMap(); // InputMap을 UI모드로 설정한다
        
        // UI 모드에서 사용자가 Swich to Player에 설정된 키를 눌렀을때 이벤트 리스너를 등록한다.
        playerInput.actions["Switch to Player"].performed += OnSwitchToPlayerActionMap;
    }

    void OnDisable()
    {
        SwichToPlayerActionMap(); // InputMap을 플레이모드로 설정한다.

        // 리스너를 삭제한다.
        playerInput.actions["Switch to Player"].performed -= OnSwitchToPlayerActionMap;
    }

    /// <summary>
    /// UI 모드에서 사용자가 "Swich to Player"에 할당된 키의 이벤트 핸들러
    /// </summary>
    /// <param name="context"></param>
    private void OnSwitchToPlayerActionMap(InputAction.CallbackContext context)
    {
        if(autoSwitchOnCanceled)
        {
            SwichToPlayerActionMap();
        }
        
        OnUICanceled?.Invoke(); // 이벤트를 발생한다.
    }
    
    /// <summary>
    /// 사용자 키 설정을 UI 모드로 전환한다.
    /// </summary>
    public void SwitchToUIActionMap()
    {
        playerInput.SwitchCurrentActionMap("UI"); // Input 설정을 UI 매핑 설정으로 변경한다.
        assetInputs.cursorInputForLook = false; // 화면을 클릭했을때 강제로 커서가 잠기는 걸 방지한다.
        assetInputs.cursorLocked = false; // 화면을 클릭했을때 강제로 커서가 잠기는 걸 방지한다.

        Cursor.visible = true; // 커서를 화면에 표시한다.
        Cursor.lockState = CursorLockMode.Confined; // 커서의 범위를 화면으로 제한한다.
    }

    /// <summary>
    /// 사용자 키 설정을 플레이 모드로 전환한다.
    /// </summary>
    public void SwichToPlayerActionMap()
    {
        playerInput.SwitchCurrentActionMap("Player"); // Input 설정을 플레이 매핑으로 변경한다.
        assetInputs.cursorInputForLook = true;
        assetInputs.cursorLocked = true;

        Cursor.visible = false; // 커서를 숨긴다.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
