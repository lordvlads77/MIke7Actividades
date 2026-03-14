using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "SaveManager", menuName = "SaveSystem/Save Manager")]
public class SaveManagerSO : ScriptableObject
{
    [Header("Dependencias")]
    public SaveConfigSO config;

    [Header("Slots de Guardado")]
    public SaveDataSO[] saveSlots; // Asigna tantos SaveDataSO como maxSaveSlots
    
    public void Save<T>(T gameData, int slot = 0)
    {
        if (!ValidateSlot(slot)) return;
        
        SaveDataSO data = saveSlots[slot];
        data.SetData(gameData);
        
        string toWrite = config.Encrypt(data.serializedData);

        
        string path = config.GetSavePath(slot);
        if (config.createBackup && File.Exists(path))
            File.Copy(path, path + ".bak", overwrite: true);
        
        File.WriteAllText(path, toWrite);
        config.Log($"Guardado en slot {slot} → {path}");
    }
    
    public T Load<T>(int slot = 0)
    {
        if (!ValidateSlot(slot)) return default;

        string path = config.GetSavePath(slot);

        if (!File.Exists(path))
        {
            config.Log($"No se encontró archivo de guardado en slot {slot}.");
            return default;
        }
        
        string raw = File.ReadAllText(path);

        
        string json = config.Decrypt(raw);

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError($"[SaveManagerSO] Datos corruptos o inválidos en slot {slot}.");
            return default;
        }
        
        saveSlots[slot].serializedData = json;
        saveSlots[slot].lastSaveDate   = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        T result = saveSlots[slot].GetData<T>();
        config.Log($"Cargado desde slot {slot} → {path}");
        return result;
    }
    
    public void Delete(int slot = 0)
    {
        if (!ValidateSlot(slot)) return;

        string path = config.GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            config.Log($"Archivo de slot {slot} eliminado.");
        }

        saveSlots[slot].ClearData();
    }
    
    public bool SaveExists(int slot = 0)
    {
        if (!ValidateSlot(slot)) return false;
        return File.Exists(config.GetSavePath(slot));
    }

    // ──────────────────────────────────────────────
    //  Privados
    // ──────────────────────────────────────────────

    private bool ValidateSlot(int slot)
    {
        if (config == null)
        {
            Debug.LogError("[SaveManagerSO] SaveConfigSO no asignado.");
            return false;
        }
        if (saveSlots == null || saveSlots.Length == 0)
        {
            Debug.LogError("[SaveManagerSO] No hay SaveDataSO asignados en saveSlots.");
            return false;
        }
        if (slot < 0 || slot >= saveSlots.Length)
        {
            Debug.LogError($"[SaveManagerSO] Slot {slot} fuera de rango (0-{saveSlots.Length - 1}).");
            return false;
        }
        if (saveSlots[slot] == null)
        {
            Debug.LogError($"[SaveManagerSO] saveSlots[{slot}] es null.");
            return false;
        }
        return true;
    }
}
