using System.Collections.Generic;
using services.saves;
using services.sounds;
using UnityEngine;
using Utils;
using utils.interfaces;
using Zenject;

namespace services {
public class ServiceManager: MonoBehaviour {
    [Inject] SoundService soundService;
    [Inject] PlayerPrefsService playerPrefsService;
    [Inject] SaveService saveService;

    Log log;
    List<AppLifecycleListener> appLifecycleListeners;
    List<SaveLoadListener> saveLoadListeners;

    void Awake() {
        log = new(GetType(), false);
        appLifecycleListeners = new();
        saveLoadListeners = new();
        registerServices();
    }

    void registerServices() {
        log.log("register services");
        registerService(soundService);
        registerService(playerPrefsService);
        registerService(saveService);
        onSavesLoaded();
    }

    void registerService(Service service) {
        if (service is AppLifecycleListener appLifecycleListener) {
            appLifecycleListeners.Add(appLifecycleListener);
        }
        if (service is SaveLoadListener saveLoadListener) {
            saveLoadListeners.Add(saveLoadListener);
        }
    }

    void onSavesLoaded() {
        log.log("on save loaded");
        var save = saveService.getSave();
        foreach (var listener in saveLoadListeners) {
            listener.onSaveLoaded(save);
        }
    }

    void OnApplicationPause(bool pauseStatus) {
        log.log("on pause");
        foreach (var listener in appLifecycleListeners) {
            listener.onPause();
        }
    }

    void OnApplicationQuit() {
        log.log("on quit");
        foreach (var listener in appLifecycleListeners) {
            listener.onQuit();
        }
    }
}

public interface Service {}
}