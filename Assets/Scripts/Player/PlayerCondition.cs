using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 데미지 받는 객체가 사용할 인터페이스
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition mana { get { return uiCondition.mana; } }

    public float noHungerHealthDecay;
    public float manaUseamount;
    public event Action onTakeDamage;

    private void Start()
    {
        manaUseamount = 10f;
    }

    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        //hunger.Add(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        
        // hunger가 0이면 체력 감소시키기
        if(hunger.curValue <= 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue <= 0.0f)
        {
            Die();
        }
    }

    public void Eat(float amount)
    {
        mana.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }
    // IDamagable 인터페이스 구현부
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount); 
        onTakeDamage?.Invoke();
        // 데미지 받은 UI 효과 발생
        // DamageIndicator의 Flash
    }
    
    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
    
    public bool UseMana(float amount)
    {
        if(mana.curValue - amount < 0)
        {
            return false;
        }
        mana.Subtract(amount);
        return true;
    }

    public void RecoverMana(float amount)
    {
        mana.Add(amount);  // 아이템 소비 시 마나 회복
    }
    
    public void OnUseSkill(InputAction.CallbackContext callbackContext)
    {
        // 스페이스 눌렸으면
        if (callbackContext.phase == InputActionPhase.Started)
        {
            // 스킬 발동
            // 마나 감소
            UseMana(manaUseamount);
        }
    }
}