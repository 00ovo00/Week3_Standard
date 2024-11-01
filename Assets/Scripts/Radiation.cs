using System.Collections.Generic;
using UnityEngine;

public class Radiation : MonoBehaviour
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