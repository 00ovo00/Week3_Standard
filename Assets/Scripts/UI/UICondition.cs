using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition mana;  // 새로운 스탯 추가

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}