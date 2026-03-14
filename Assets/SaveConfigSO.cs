using UnityEngine;

[CreateAssetMenu(fileName = "SaveConfig", menuName = "SaveSystem/Save Config")]
public class SaveConfigSO : ScriptableObject
{
    [Header("Configuración de Archivos")]
    [Tooltip("Nombre base del archivo de guardado (sin extensión).")]
    public string saveFileName = "gamesave";

    [Tooltip("Extensión del archivo de guardado.")]
    public string fileExtension = ".sav";

    [Tooltip("Número máximo de slots de guardado.")]
    [Range(1, 10)]
    public int maxSaveSlots = 3;

    [Header("Opciones de Seguridad")]
    [Tooltip("Aplicar cifrado Base64 al string guardado.")]
    public bool useEncryption = false;

    [Tooltip("Clave usada para el cifrado (XOR simple). Cámbiala en producción.")]
    public string encryptionKey = "M1_C1av3_S3cr3ta";

    [Header("Opciones de Respaldo")]
    [Tooltip("Crear archivo de respaldo (.bak) antes de sobreescribir.")]
    public bool createBackup = true;

    [Header("Debug")]
    public bool enableLogs = true;
    
    public string GetSavePath(int slot)
    {
        slot = Mathf.Clamp(slot, 0, maxSaveSlots - 1);
        return System.IO.Path.Combine(
            Application.persistentDataPath,
            $"{saveFileName}_slot{slot}{fileExtension}"
        );
    }
    
    public string Encrypt(string raw)
    {
        if (!useEncryption || string.IsNullOrEmpty(raw)) return raw;

        char[] key = encryptionKey.ToCharArray();
        char[] chars = raw.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            chars[i] = (char)(chars[i] ^ key[i % key.Length]);

        return System.Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(new string(chars))
        );
    }

    
    public string Decrypt(string encrypted)
    {
        if (!useEncryption || string.IsNullOrEmpty(encrypted)) return encrypted;

        try
        {
            byte[] bytes = System.Convert.FromBase64String(encrypted);
            char[] chars = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();
            char[] key = encryptionKey.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)(chars[i] ^ key[i % key.Length]);
            return new string(chars);
        }
        catch
        {
            Debug.LogError("[SaveConfigSO] Error al descifrar. ¿Clave incorrecta?");
            return null;
        }
    }

    public void Log(string msg)
    {
        if (enableLogs) Debug.Log($"[SaveSystem] {msg}");
    }
}

