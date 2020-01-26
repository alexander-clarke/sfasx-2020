using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] public int money { get; private set; } = 100;

    [SerializeField] private int moneyPerSecond = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoMoneyPerSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            money += moneyPerSecond;
        }
    }

    public void Init(int startMoney)
    {
        money = startMoney;
        StartCoroutine(DoMoneyPerSecond());
    }

    public void ResetMoney()
    {
        StopAllCoroutines();
    }

    internal void GiveMoney(int val)
    {
        money += val;
    }


    public bool TrySpend(int val)
    {
        if (money - val < 0)
        {
            return false;
        }
        else
        {
            money -= val;
            return true;
        }
    }
}
