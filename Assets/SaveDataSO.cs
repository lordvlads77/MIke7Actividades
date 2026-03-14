using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveSystem/Save Data")]
public class SaveDataSO : ScriptableObject
{
    [Header("Datos Serializados")]
    [TextArea(5, 20)]
    public string serializedData = "";

    [Header("Metadatos")]
    public string slotName = "Slot_1";
    public string lastSaveDate = "";
    public int saveVersion = 1;
    
    public void SetData<T>(T data)
    {
        serializedData = JsonUtility.ToJson(data, prettyPrint: true);
        lastSaveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Recupera el objeto deserializando el string JSON almacenado.
    /// </summary>
    public T GetData<T>()
    {
        if (string.IsNullOrEmpty(serializedData))
        {
            Debug.LogWarning($"[SaveDataSO] No hay datos en el slot '{slotName}'.");
            return default;
        }
        return JsonUtility.FromJson<T>(serializedData);
    }

    /// <summary>
    /// Limpia todos los datos del slot.
    /// </summary>
    public void ClearData()
    {
        serializedData = "";
        lastSaveDate = "";
        Debug.Log($"[SaveDataSO] Slot '{slotName}' limpiado.");
    }

    public bool HasData() => !string.IsNullOrEmpty(serializedData);
}

