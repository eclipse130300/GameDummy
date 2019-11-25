using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour, IServiceLocator
{
    private Dictionary<object, IService> services;

    public T GetService<T>()
    {
        try
        {
            return (T)services[typeof(T)];
        }
        catch (KeyNotFoundException)
        {
            Debug.LogFormat("Service {0} is not registered", typeof(T).ToString());
            Debug.LogFormat("Creating {0}...", typeof(T).ToString());
            return (T)AddService<T>();
        }
    }

    private IService AddService<T>()
    {
        IGeneralizedServiceBuilder serviceBuilder = new ServiceBuilder();
        services.Add(typeof(T), serviceBuilder.Create<T>());
        return services[typeof(T)];
    }

    private void Start()
    {
        services = new Dictionary<object, IService>();

        try
        {
            IService[] servicesToAdd = new IService[] 
            {
                GetService<IUIController>(),
                GetService<IDataController>()
            };
            foreach (IService service in servicesToAdd)
                Debug.LogFormat("Service {0} successfuly created", service.name);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        
        foreach (var service in services)
            service.Value.Init(this);
    }
}

#region Factory

[System.Serializable]
public class UIControllerBuilder : IServiceBuilder
{
    public IService Create()
    {
        GameObject uiController = Object.Instantiate(Resources.Load<GameObject>("Services/UIController"));
        uiController.name = "UIController";
        return uiController.GetComponent<IService>();
    }
}

[System.Serializable]
public class DataControllerBuilder : IServiceBuilder
{
    public IService Create()
    {
        GameObject uiController = Object.Instantiate(Resources.Load<GameObject>("Services/DataController"));
        uiController.name = "DataController";
        return uiController.GetComponent<IService>();
    }
}

#endregion

#region GeneralizedFactory

[System.Serializable]
public class ServiceBuilder : IGeneralizedServiceBuilder
{
    public IService Create<T>()
    {
        object service = typeof(T);
        GameObject uiController = Object.Instantiate(Resources.Load<GameObject>("Services/" + service.ToString()));
        uiController.name = service.ToString();
        return uiController.GetComponent<IService>();
    }
}

#endregion