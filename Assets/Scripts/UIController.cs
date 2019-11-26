using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IUIController
{
    [SerializeField] private Text coins;
    [SerializeField] private Text gems;

    private IServiceLocator ServiceLocator { get; set; }
    
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
