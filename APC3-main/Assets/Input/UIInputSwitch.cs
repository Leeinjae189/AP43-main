using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputSwitch : MonoBehaviour
{
    public event Action OnUICanceled;

    /// <summary>
    /// <see langword="true"/> �̸� ����ڰ� UI ��忡�� "Swich to Player"�� �Ҵ�� Ű�� �������� �ڵ����� �÷��� ���� ��ȯ�Ѵ�.
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
        SwitchToUIActionMap(); // InputMap�� UI���� �����Ѵ�
        
        // UI ��忡�� ����ڰ� Swich to Player�� ������ Ű�� �������� �̺�Ʈ �����ʸ� ����Ѵ�.
        playerInput.actions["Switch to Player"].performed += OnSwitchToPlayerActionMap;
    }

    void OnDisable()
    {
        SwichToPlayerActionMap(); // InputMap�� �÷��̸��� �����Ѵ�.

        // �����ʸ� �����Ѵ�.
        playerInput.actions["Switch to Player"].performed -= OnSwitchToPlayerActionMap;
    }

    /// <summary>
    /// UI ��忡�� ����ڰ� "Swich to Player"�� �Ҵ�� Ű�� �̺�Ʈ �ڵ鷯
    /// </summary>
    /// <param name="context"></param>
    private void OnSwitchToPlayerActionMap(InputAction.CallbackContext context)
    {
        if(autoSwitchOnCanceled)
        {
            SwichToPlayerActionMap();
        }
        
        OnUICanceled?.Invoke(); // �̺�Ʈ�� �߻��Ѵ�.
    }
    
    /// <summary>
    /// ����� Ű ������ UI ���� ��ȯ�Ѵ�.
    /// </summary>
    public void SwitchToUIActionMap()
    {
        playerInput.SwitchCurrentActionMap("UI"); // Input ������ UI ���� �������� �����Ѵ�.
        assetInputs.cursorInputForLook = false; // ȭ���� Ŭ�������� ������ Ŀ���� ���� �� �����Ѵ�.
        assetInputs.cursorLocked = false; // ȭ���� Ŭ�������� ������ Ŀ���� ���� �� �����Ѵ�.

        Cursor.visible = true; // Ŀ���� ȭ�鿡 ǥ���Ѵ�.
        Cursor.lockState = CursorLockMode.Confined; // Ŀ���� ������ ȭ������ �����Ѵ�.
    }

    /// <summary>
    /// ����� Ű ������ �÷��� ���� ��ȯ�Ѵ�.
    /// </summary>
    public void SwichToPlayerActionMap()
    {
        playerInput.SwitchCurrentActionMap("Player"); // Input ������ �÷��� �������� �����Ѵ�.
        assetInputs.cursorInputForLook = true;
        assetInputs.cursorLocked = true;

        Cursor.visible = false; // Ŀ���� �����.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
