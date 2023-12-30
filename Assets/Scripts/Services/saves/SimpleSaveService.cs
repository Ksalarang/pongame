using Utils;
using utils.interfaces;
using Zenject;

namespace services.saves {
public class SimpleSaveService : SaveService, AppLifecycleListener {
    const string SaveFileName = "savedata.dat";
    readonly Log log;
    readonly PlayerSave save;

    [Inject]
    public SimpleSaveService() {
        log = new(GetType(), false);
        save = new();
        loadData();
    }

    void loadData() {
        if (FileUtils.loadFromFile(SaveFileName, out var data)) {
            log.log($"loaded from file: {data}");
            save.fromJson(data);
        } else {
            log.warn($"failed to load data from file '{SaveFileName}'");
        }
    }

    void saveData() {
        if (FileUtils.writeToFile(SaveFileName, save.toJson())) {
            log.log("data saved");
        } else {
            log.error("failed to save data");
        }
    }

    public PlayerSave getSave() => save;

    public void onPause() {
        saveData();
    }

    public void onQuit() {
        saveData();
    }
}
}