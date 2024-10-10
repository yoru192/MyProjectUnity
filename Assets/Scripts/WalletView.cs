using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountView;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Enemy _enemy;

    private void OnEnable()
    {
        _wallet.AmountChanged += DisplayAmount;
        _enemy.Died += AddAmount;
    }

    private void OnDisable()
    {
        _wallet.AmountChanged -= DisplayAmount;
        //_enemy.Died -= AddAmount;
    }
    
    public void DisplayAmount()
    {
        float amount = _wallet.Money;
        _amountView.text = amount.ToString(CultureInfo.InvariantCulture);
    }

    public void AddAmount()
    {

        _wallet.AddMoney(_enemy.maxHealth);
        DisplayAmount();
    }
}
