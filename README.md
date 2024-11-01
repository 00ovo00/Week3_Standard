# Week3_Standard

<details>
    <summary><b> Q1. 숙련 1강 ~ 숙련 3강</b></summary>
    <div markdown="1">
    <ul>
        
**분석 문제** 분석한 내용을 직접 작성하고, 강의의 코드를 다시 한번 작성하며 복습해봅시다.

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
    </ul>
  </div>
</details>

<details>
    <summary><b> Q2. 숙련 4강 ~ 숙련 6강 </b></summary>
    <div markdown="1">
    <ul>

**분석 문제** 분석한 내용을 직접 작성하고, 강의의 코드를 다시 한번 작성하며 복습해봅시다.

- 별도의 UI 스크립트를 만드는 이유에 대해 객체지향적 관점에서 생각해보세요.
    
    <aside>
    
    기능을 모듈화 하여 재사용성과 확장성을 높이기 위해 UI를 별도의 스크립트로 작성한다. 로직과 UI가 분리되어 작성되어야 코드 흐름을 파악하기 쉽고 유지보수에도 용이하다. 
    
    </aside>
    
- 인터페이스의 특징에 대해 정리해보고 구현된 로직을 분석해보세요.
    
    <aside>
    
    인터페이스는 공통된 기능을 여러 클래스가 구현하도록 형식을 미리 정의한다. 인터페이스 자체는 구현되지 않으며 이를 상속하는 클래스가 구현 해야할 틀을 제시하는 역할을 한다.
    
    </aside>
    
    ```csharp
    /* PlayerCondition.cs */
    // 인터페이스 제시부
    // 피해를 입을 수 있는 모든 클래스는 이 인터페이스를 상속
    public interface IDamagable
    {
        void TakePhysicalDamage(int damageAmount);
    }
    
    public class PlayerCondition : MonoBehaviour, IDamagable
        // IDamagable 인터페이스 구현부
        // IDamagable을 상속한 클래스는 구현 필수
        public void TakePhysicalDamage(int damageAmount)
        {
            health.Subtract(damageAmount); 
            onTakeDamage?.Invoke();
            // 데미지 받은 UI 효과 발생
            // DamageIndicator의 Flash
        }
    }
    ```
    
    ```csharp
    /* Campfire.cs */
    using System.Collections.Generic;
    using UnityEngine;
    
    public class CampFire : MonoBehaviour
    {
        public int damage;
        public float damageRate;
    
    // IDamagable한 오브젝트의 리스트
        private List<IDamagable> things = new List<IDamagable>();
    
        private void Start()
        {
        // 데미지를 주는 메소드를 지속적으로 호출
            InvokeRepeating("DealDamage", 0, damageRate);
        }
    
        void DealDamage()
        {
        // 리스트에 있는 모든 오브젝트에 데미지 입히기
            for(int i = 0; i<things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
        // IDamagable 상속한 객체가 trigger 되면 리스트에 추가
            if(other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                things.Add(damagable);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                things.Remove(damagable);
            }
        }
    }
    ```
    
- 핵심 로직을 분석해보세요. (UI 스크립트 구조, `CampFire`, `DamageIndicator`)
    
    <aside>
    
    **PlayerCondition**
    플레이어의 상태를 실시간으로 관리하는 역할로 직접적인 상태 변화의 수치 변경을 담당하는 클래스
    **Condition**
    각 상태 데이터를 UI에 반영하는 클래스(개별 상태 UI 담당)
    **UIConditon**
    Conditon 클래스를 포함하여 모든 상태의 UI를 연결
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/3313da1d-9e3e-49ba-a64b-11ec9ce9f3f8/79c4df0a-36f3-440b-91e3-2f1b5102c56b/image.png)
    
    </aside>
    
    ```csharp
    /* CampFire.cs */
    public class CampFire : MonoBehaviour
    {
        public int damage;  // 데미지 양
        public float damageRate;    // 데미지 적용 주기
    
        // 데미지 가할 대상 리스트
        private List<IDamagable> things = new List<IDamagable>();
    
        private void Start()
        {
            // 데미지 적용 주기마다 데미지 주는 메소드 호출
            InvokeRepeating("DealDamage", 0, damageRate);
        }
    
        void DealDamage()
        {
            // 데미지 적용 리스트에 있는 모든 객체에 데미지 적용
            for(int i = 0; i<things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            // IDamagable 상속하는 객체가 trigger 되면 데미지 적용 목록에 추가
            if(other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                things.Add(damagable);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            // IDamagable 상속하는 객체가 trigger 되지 않는 상태면 데미지 적용 목록에서 삭제
            if(other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                things.Remove(damagable);
            }
        }
    }
    ```
    
    ```csharp
    /* DamageIndicator.cs */
    // 붉은색 DamageIndicator Image 활성화되었다가
    // flashSpeed 동안 점차 투명해지고 비활성화하는 로직
    public class DamageIndicator : MonoBehaviour
    {
        public Image image;
        public float flashSpeed;
    
        private Coroutine coroutine;
    
        private void Start()
        {
            // 플레이어가 데미지 받을 때 Flash 호출하도록 등록
            CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
        }
    
        public void Flash()
        {
            // 실행중인 코루틴이 있으면 중지
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
    
            image.enabled = true;
            image.color = new Color(1f, 105f/255f, 105f/255f);
            coroutine = StartCoroutine(FadeAway());
        }
        
        private IEnumerator FadeAway()
        {
            float startAlpha = 0.3f;    // 시작 알파값
            float a = startAlpha;
    
            while(a > 0.0f) 
            {
                // 시간 지날수록 알파값 감소하도록 설정
                a -= (startAlpha / flashSpeed) * Time.deltaTime;
                image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
                yield return null;
            }
            image.enabled = false;
        }
    }
    ```    
    </aside>
    </ul>
  </div>
</details>

<details>
    <summary><b> Q3. 숙련 9강 ~ 숙련 11강</b></summary>
    <div markdown="1">
    <ul>

**분석 문제** : 분석한 내용을 직접 작성하고, 강의의 코드를 다시 한번 작성하며 복습해봅시다.

- `Interaction` 기능의 구조와 핵심 로직을 분석해보세요.
    
    ```csharp
    /* Interaction.cs */
    // 플레이어가 Interactable한 오브젝트와 상호작용하도록 하는 역할
    // Raycast를 이용해 특정 오브젝트 감지하고 정보를 받아와 UI에 표시하고 이벤트 발생시킴
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;
    
    public interface IInteractable
    {
        public string GetInteractPrompt();
        public void OnInteract();
    }
    
    public class Interaction : MonoBehaviour
    {
        public float checkRate = 0.05f; // 상호작용 중인지 체크하는 빈도
        private float lastCheckTime;
        public float maxCheckDistance;
        public LayerMask layerMask;
    
        public GameObject curInteractGameObject;
        private IInteractable curInteractable;
    
        public TextMeshProUGUI promptText;
        private Camera camera;
    
        void Start()
        {
            camera = Camera.main;
        }
    
        void Update()
        {
            if(Time.time - lastCheckTime > checkRate)
            {
                lastCheckTime = Time.time;
                
                // 스크린 기준 정중앙에서 ray 발사
                Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                RaycastHit hit;
    
                // maxCheckDistance 내에 layerMask와 일치하여 raycast된 hit 정보 가져오기
                if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
                {
                    // 현재 상호작용 중인 객체가 아니면(새로운 객체를 상호작용하는 경우)
                    if(hit.collider.gameObject != curInteractGameObject)
                    {
                        // 현재 상호작용 중인 객체 정보 갱신하고 프롬프트 텍스트 설정
                        curInteractGameObject = hit.collider.gameObject;
                        curInteractable = hit.collider.GetComponent<IInteractable>();
                        SetPromptText();
                    }
                }
                // raycast된 객체가 없으면
                else
                {
                    // 현재 상호작용 중인 객체 null로 만들고 프롬프트의 텍스트 비활성화
                    curInteractGameObject = null;
                    curInteractable = null;
                    promptText.gameObject.SetActive(false);
                }
            }
        }
    
        private void SetPromptText()
        {
            // 상호작용 텍스트 설정하고 활성화
            promptText.gameObject.SetActive(true);
            promptText.text = curInteractable.GetInteractPrompt();
        }
    
        public void OnInteractInput(InputAction.CallbackContext context)
        {
            // E키가 눌렸고 상호작용 가능한 객체가 있을 때
            if(context.phase == InputActionPhase.Started && curInteractable != null)
            {
                // 상호작용하는 함수 호출(상호작용 실행) 후
                curInteractable.OnInteract();
                // 상호작용 중인 객체가 없는 상태로 reset
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    ```
    
- `Inventory` 기능의 구조와 핵심 로직을 분석해보세요.
    
    ```csharp
    /* UIInventory.cs */
    // 획득한 아이템 저장하고 UI를 통해 화면에 출력하는 역할
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;
    
    public class UIInventory : MonoBehaviour
    {
        public ItemSlot[] slots;
    
        public GameObject inventoryWindow;
        public Transform slotPanel;
        public Transform dropPosition;
    
        [Header("Selected Item")]
        private ItemSlot selectedItem;
        private int selectedItemIndex;
        public TextMeshProUGUI selectedItemName;
        public TextMeshProUGUI selectedItemDescription;
        public TextMeshProUGUI selectedItemStatName;
        public TextMeshProUGUI selectedItemStatValue;
        public GameObject useButton;
        public GameObject equipButton;
        public GameObject unEquipButton;
        public GameObject dropButton;
    
        private int curEquipIndex;
    
        private PlayerController controller;
        private PlayerCondition condition;
    
        void Start()
        {
            controller = CharacterManager.Instance.Player.controller;
            condition = CharacterManager.Instance.Player.condition;
            dropPosition = CharacterManager.Instance.Player.dropPosition;
    
            controller.inventory += Toggle;
            CharacterManager.Instance.Player.addItem += AddItem;
    
            inventoryWindow.SetActive(false);
            slots = new ItemSlot[slotPanel.childCount];
    
            for(int i = 0; i < slots.Length; i++)
            {
                slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
                slots[i].index = i;
                slots[i].inventory = this;
                slots[i].Clear();
            }
    
            ClearSelectedItemWindow();  // 초기 세팅
        }
    
        public void AddItem()
        {
            // 현재 상호작용 중인 아이템 정보 가져오기
            ItemData data = CharacterManager.Instance.Player.itemData;
            // 중복 가능한 아이템이면
            if (data.canStack)
            {
                // 아이템 정보 가져오기
                ItemSlot slot = GetItemStack(data);
                // 최대 개수보다 적으면
                if(slot != null)
                {
                    // 수량만 더해주고 UI 갱신
                    slot.quantity++;
                    UpdateUI();
                    // 현재 상호작용 중인 아이템 없는 상태로 만듦
                    CharacterManager.Instance.Player.itemData = null;
                    return;
                }
            }
            // 아이템 슬롯이 비어있으면 빈 슬롯 세팅
            ItemSlot emptySlot = GetEmptySlot();
            // 슬롯에 아이템이 있으면
            if(emptySlot != null)
            {
                // 아이템 데이터와 수량을 UI에 갱신
                emptySlot.item = data;
                emptySlot.quantity = 1;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
            // 이미 해당 아이템 최고 개수인 경우 아이템 버림
            ThrowItem(data);
            CharacterManager.Instance.Player.itemData = null;
        }
    
        public void UpdateUI()
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    slots[i].Set();
                }
                else
                {
                    slots[i].Clear();
                }
            }
        }
    
        ItemSlot GetItemStack(ItemData data)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                // 아이템 데이터와 슬롯의 아이템이 같고 슬롯 수량이 최대값보다 작으면
                if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
                {
                    return slots[i];
                }
            }
            return null;
        }
    
        public void SelectItem(int index)
        {
            if (slots[index].item == null) return;
    
            selectedItem = slots[index];
            selectedItemIndex = index;
    
            selectedItemName.text = selectedItem.item.displayName;
            selectedItemDescription.text = selectedItem.item.description;
            
            // 스탯 문자열은 공백으로 초기화
            selectedItemStatName.text = string.Empty;
            selectedItemStatValue.text = string.Empty;
    
            // item이 consumable인 경우에 수치를 출력
            for(int i = 0; i< selectedItem.item.consumables.Length; i++)
            {
                selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
                selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
            }
    
            useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
            equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
            unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
            dropButton.SetActive(true);
        }
    
        // 버린 아이템 UI 정보 업데이트
        void RemoveSelctedItem()
        {
            selectedItem.quantity--;
    
            if(selectedItem.quantity <= 0)
            {
                if (slots[selectedItemIndex].equipped)
                {
                    UnEquip(selectedItemIndex);
                }
    
                selectedItem.item = null;
                ClearSelectedItemWindow();
            }
            UpdateUI();
        }
    }
    ```
    </ul>
  </div>
</details>
