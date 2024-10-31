# Week3_Standard

### Q1. 숙련 1강 ~ 숙련 3강

**분석 문제** : 분석한 내용을 직접 작성하고, 강의의 코드를 다시 한번 작성하며 복습해봅시다.

- 입문 주차와 비교해서 입력 받는 방식의 차이와 공통점을 비교해보세요.
    
    <aside>
    
    공통점 : InputSystem 이용하여 입력
    
    차이점: Look Event를    
    입문 주차 강의 에서는 Control Type Vector2로 설정하여 Mouse Position 값을 입력 받아 사용, 마우스의 위치를 이용, InputSystem의 Behavior로 SendMessage 방식 사용    
    숙련 주차 강의 에서는 Control Type Delta로 설정하여 Mouse Delta 값을 입력 받아 사용, 이전 프레임에서의 마우스 변화값을 이용, InputSystem의 Behavior로 Invoke Unity Events 방식 사용
    
    </aside>
    
- `CharacterManager`와 `Player`의 역할에 대해 고민해보세요.
    
    <aside>
    
    Player는 상호작용 중인 아이템이나 장착 중인 아이템 정보를 가지고 있으며, 이동, 시점, 상태를 제어하는 컴포넌트 가지고 있음
    
    CharacterManager는 싱글톤으로 구현되어 Player를 Get으로 간접적으로 가져옴,
    다른 클래스에서 Player에 직접 접근하지 않도록 하면서 Player 정보를 활용할 수 있게 간접적으로 열어두는 역할
    
    </aside>
    
- 핵심 로직을 분석해보세요 (`Move`, `CameraLook`, `IsGrounded`)

```csharp
private void Move()
{
    // curMovementInput: 얼마나 이동하는지에 대한 값(사용자 입력으로 받아오기)
    // 이동에 대한 Vector 정보 저장(방향 정보)
    Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
    // 속도 정보 더하기
    dir *= moveSpeed;
    // 현재 y축 속도 적용하며 점프 상태 유지
    dir.y = rigidbody.velocity.y;
    // 물체가 움직이도록 설정(실제 물리이동 처리)
    rigidbody.velocity = dir;
}
```

```csharp
void CameraLook()
{
    // 민감도와 마우스 움직임 입력값에 따라 카메라 회전 값 설정
    camCurXRot += mouseDelta.y * lookSensitivity;
    // 시야각 내에서 카메라 회전하도록 제한
    camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
    // 3D 좌표 공간에서 움직임 절대값 반전되므로 조정
    cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

    // y축 움직임 설정(플레이어 기준 y축으로만 회전)
    transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
}
/* 카메라 위치와 마우스 위치 동기화 */
// 플레이어가 입력한 마우스 위치를 직관적으로 반영하여 카메라를 회전시키려면
// 위와 같이 축과 절대값을 반대로 적용하는 과정 필요
```

```csharp
bool IsGrounded()
{
    // 플레이어 기준 4방향 아래로 향하는 ray(책상 다리 형태)
    Ray[] rays = new Ray[4]
    {
        new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
    };

    for (int i = 0; i < rays.Length; i++)
    {
        // 바닥에 Raycast되면 
        if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
        {
            // 땅에 붙어있는 상태로 인식
            return true;
        }
    }
    // 공중에 떠있는 상태로 인식
    return false;
}
```

- `Move`와 `CameraLook` 함수를 각각 `FixedUpdate`, `LateUpdate`에서 호출하는 이유에 대해 생각해보세요.
    
    <aside>
    
    FixedUpdate는 Time.deltaTime과 무관하게 일정 시간 간격으로 호출되어 물리 기반 로직 실행에 적합, Move는 Rigidbody 이용해 실제 물리 연산이 일어나므로 FixedUpdate에서 실행하여 프레임 속도와 관계 없이 일관된 플레이어 움직임 구현 가능하게 함
    
    LateUpdate는 모든 Update 실행이 완료되고 호출, 플레이어가 프레임 당 이동한 만큼의 최신 위치와 시점을 카메라에 반영해야하므로 LateUpdate에서 호출하게 함
    
    </aside>
