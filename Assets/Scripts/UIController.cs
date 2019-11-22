using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IUIController
{
    private IServiceLocator ServiceLocator { get; set; }

    [SerializeField] Text coins;
    [SerializeField] Text gems;

    public void Init(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
        DontDestroyOnLoad(this);
    }

    public void UpdateCurrency(ICurrencyData data)
    {
        coins.text = data.Coins.ToString();
        gems.text = data.Gems.ToString();
    }
}
