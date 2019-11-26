using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using Utility;

public class DataController : MonoBehaviour, IDataController, ICurrencyData, ISaveManager
{
    #region Static

    public static bool Started;

    #endregion //Static
    
    #region Properties
    
    private double coins;
    [ShowNativeProperty]
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
    [ShowNativeProperty]
    public int Gems
    {
        get => gems;
        set
        {
            gems = value;
            uiController.UpdateCurrency(this);
        }
    }

    #endregion //Properties

    #region Private

    private IServiceLocator ServiceLocator { get; set; }
    private IUIController uiController { get; set; }
    [ShowNonSerializedField] private int currentScene;

    #endregion //Private

    #region Unity Methods

    private void OnApplicationPause(bool pause)
    {
        if (Started && pause)
        {
            SaveGameState();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    #endregion //Unity Methods

    public void Init(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
        uiController = ServiceLocator.GetService<IUIController>();

        Coins = 100;
        Gems = 200;
        currentScene = 0;

        DontDestroyOnLoad(this);

        Application.targetFrameRate = 100;
        SceneManager.sceneLoaded += OnLevelLoaded;

        TryLoadData();

        LoadLevel();
    }

    public void LoadLevel()
    {
        LoadScene(((SceneNames)currentScene).ToString());
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode) { }
    
    #region Scene Management

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    #endregion //Scene Management

    #region Save/Load

    public void SaveGameState()
    {
        SaveData data = new SaveData();

        data.coins = coins;
        data.gems = gems;
        
        Serialization.SaveToBinnary<SaveData>(data);
    }

    private void TryLoadData()
    {
        SaveData data;
        if (!Serialization.LoadFromBinnary<SaveData>(out data)) return;

        Coins = data.coins;
        Gems = data.gems;
    }

    #endregion //Save/Load
}

[System.Serializable]
public class SaveData
{
    public double coins;
    public int gems;
}

public enum SceneNames
{
    Game
}
