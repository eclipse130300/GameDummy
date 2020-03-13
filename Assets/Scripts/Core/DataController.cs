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

    private long softCurrency;
    [ShowNativeProperty]
    public long SoftCurrency
    {
        get
        {
            return softCurrency;
        }
        set
        {
            softCurrency = value;
            uiController.UpdateCurrency(this);
        }
    }
    
    private long donateCurrency;
    [ShowNativeProperty]
    public long DonateCurrency
    {
        get
        {
            return donateCurrency;
        }
        set
        {
            donateCurrency = value;
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
        
        currentScene = 0;

        DontDestroyOnLoad(this);

        Application.targetFrameRate = 100;
        SceneManager.sceneLoaded += OnLevelLoaded;

        TryLoadData();

        LoadGame();

        Started = true;
    }

    public void LoadGame()
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

        data.Add("SoftCurrency", SoftCurrency);
        data.Add("DonateCurrency", DonateCurrency);

        if (Serialization.SaveToBinnary<SaveData>(data)) Debug.Log("Game saved succesfuly");
        else Debug.Log("Game not saved");
    }

    private void TryLoadData()
    {
        SaveData data;
        if (!Serialization.LoadFromBinnary<SaveData>(out data)) return;

        SoftCurrency = data.Get<long>("SoftCurrency");
        DonateCurrency = data.Get<long>("DonateCurrency");
    }

    #endregion //Save/Load
}

[System.Serializable]
public class SaveData
{
    private Dictionary<string, object> container;

    public SaveData()
    {
        container = new Dictionary<string, object>();
    }

    public void Add(string name, object obj)
    {
        container.Add(name, obj);
    }

    public T Get<T>(string name)
    {
        try
        {
            return (T)container[name];
        }
        catch
        {
            return default;
        }
    }
}

public enum SceneNames
{
    Game
}
