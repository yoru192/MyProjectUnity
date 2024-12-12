using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountView;
    [SerializeField] private Wallet _wallet;

    private void OnEnable()
    {
        _wallet.AmountChanged += DisplayAmount;

        // Підписуємося на подію смерті ворогів
        Enemy.AnyEnemyDied += AddAmount;
    }

    private void OnDisable()
    {
        _wallet.AmountChanged -= DisplayAmount;

        // Відписуємося від події смерті ворогів
        Enemy.AnyEnemyDied -= AddAmount;
    }

    public void DisplayAmount()
    {
        float amount = _wallet.Money;
        _amountView.text = amount.ToString(CultureInfo.InvariantCulture);
    }

    private void AddAmount(int amount)
    {
        // Додаємо кількість грошей, яка відповідає здоров'ю ворога
        _wallet.AddMoney(amount);
        DisplayAmount();
    }
}


