using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3.0f; // 상호작용 거리
    public LayerMask interactableLayer; // 상호작용 가능한 레이어
    private Camera playerCamera; // 플레이어 카메라

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // E 키를 눌렀을 때 상호작용 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hitInfo;
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // 상호작용 대상 레이어에 충돌한 경우 실행
            if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    // 상호작용 실행
                    interactable.Interact();
                }
            }
        }
    }
}