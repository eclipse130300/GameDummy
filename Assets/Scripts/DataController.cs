using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour, IDataController, ICurrencyData, ISaveManager
{
    private IServiceLocator ServiceLocator { get; set; }
    private IUIController uiController { get; set; }

    private double coins;
    public double Coins
    {
        get => coins;
        set
        {
            coins = value;
            uiController.UpdateCurrency(this);
        }
    }

    private int gems;
    public int Gems
    {
        get => gems;
        set
        {
            gems = value;
            uiController.UpdateCurrency(this);
        }
    }

    public void Init(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
        uiController = serviceLocator.GetService<IUIController>();

        Coins = 100;
        Gems = 200;

        DontDestroyOnLoad(this);

        Application.targetFrameRate = 100;
        SceneManager.sceneLoaded += OnLevelLoaded;

        TryLoadData();

        LoadLevel();
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public void LoadLevel() { }

    #region Save/Load

    public void SaveGameState() { }

    private void TryLoadData() { }

    #endregion
}
