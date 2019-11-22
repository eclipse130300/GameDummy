using UnityEngine;

#region DataController

public interface ICurrencyData
{
    double Coins { get; set; }
    int Gems { get; set; }
}

public interface ISaveManager
{
    void SaveGameState();
}

#endregion

public interface IServiceLocator
{
    T GetService<T>();
}

public interface IService
{
    string name { get; set; }
    void Init(IServiceLocator serviceLocator);
}

public interface IServiceBuilder
{
    IService Create();
}

public interface IGeneralizedServiceBuilder
{
    IService Create<T>();
}

#region Services

public interface IUIController : IService
{
    void UpdateCurrency(ICurrencyData data);
}

public interface IDataController : IService
{

}

#endregion