using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;  // 회전 범위 최소값
    public float maxXLook;  // 회전 범위 최대값
    private float camCurXRot;   // 현재 회전값
    public float lookSensitivity;   // 민감도

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true; // 인벤토리용 커서 활성화 여부
    // true면 카메라 회전, 인벤토리 비활성화, 커서 비활성화
    // false면 카메라 고정, 인벤토리 활성화, 커서 활성화

    private Rigidbody rigidbody;
    
    public Action inventory;
    public Action setting;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 마우스 커서 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        // 인벤토리 비활성 상태일 때만 카메라 움직이도록 제한
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // 키 처음 눌린 상태
        // if (context.phase == InputActionPhase.Started)
        // 계속해서 키 입력되므로 Performed 사용
        // 키 눌린 상태
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        // 키 눌리지 않은 상태
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;    // 이동 중지
        }
    }
    
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    
    public void OnSetting(InputAction.CallbackContext callbackContext)
    {
        // Ctrl 키 눌렸으면
        if (callbackContext.phase == InputActionPhase.Started)
        {
            setting?.Invoke();
            ToggleCursor();
        }
    }

    public void ToggleCursor()
    {
        // 커서 상태 확인
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        // 락 상태이면(설정창 안열리고 커서 비활성화) 락 해제
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;  // 커서 활성화 여부 토글
    }
}