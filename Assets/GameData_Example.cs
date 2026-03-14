using UnityEngine;

// ══════════════════════════════════════════════════════════════
//  VECTOR3 SERIALIZABLE
//  JsonUtility no puede serializar Vector3 de Unity directamente,
//  así que usamos este wrapper. Se convierte con .ToVector3()
//  y se crea desde un Vector3 con SerializableVector3.From(v).
// ══════════════════════════════════════════════════════════════

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    /// <summary>Crea un SerializableVector3 desde un Vector3 de Unity.</summary>
    public static SerializableVector3 From(Vector3 v) =>
        new SerializableVector3 { x = v.x, y = v.y, z = v.z };

    /// <summary>Convierte de vuelta a Vector3 de Unity.</summary>
    public Vector3 ToVector3() => new Vector3(x, y, z);

    public override string ToString() => $"({x:F2}, {y:F2}, {z:F2})";
}


// ══════════════════════════════════════════════════════════════
//  ESTRUCTURA DE DATOS DEL JUEGO
//  Todos los campos que quieras persistir van aquí.
//  JsonUtility solo serializa campos públicos o [SerializeField].
// ══════════════════════════════════════════════════════════════

[System.Serializable]
public struct GameData
{
    // Jugador
    public string playerName;
    public int    level;
    public float  health;

    // Posición — capturada automáticamente desde el transform
    public SerializableVector3 position;

    // Progreso
    public int    score;
    public int    coinsCollected;
    public bool   bossDefeated;
    public int    currentScene;

    // Inventario (array serializable)
    public string[] inventory;

    // Configuración guardada
    public float musicVolume;
    public float sfxVolume;
}


// ══════════════════════════════════════════════════════════════
//  EJEMPLO DE USO — MonoBehaviour
//  Arrastra el SaveManagerSO al campo saveManager en el Inspector.
//  Este script debe estar en el GameObject del jugador para que
//  transform.position capture su posición automáticamente.
// ══════════════════════════════════════════════════════════════
public class GameSaveExample : MonoBehaviour
{
    [Header("Sistema de Guardado")]
    public SaveManagerSO saveManager;

    [Header("Slot activo")]
    [Range(0, 2)]
    public int activeSlot = 0;

    // ── Guardar ─────────────────────────────────────────────
    // transform.position se lee en el momento exacto de llamar SaveGame(),
    // capturando la posición actual del jugador en la escena.
    public void SaveGame()
    {
        GameData data = new GameData
        {
            playerName     = "Héroe",
            level          = 5,
            health         = 80f,
            position       = SerializableVector3.From(transform.position), // ← automático
            score          = 1234,
            coinsCollected = 42,
            bossDefeated   = true,
            currentScene   = 2,
            inventory      = new[] { "Espada", "Poción", "Escudo" },
            musicVolume    = 0.8f,
            sfxVolume      = 1.0f
        };

        saveManager.Save(data, activeSlot);
        Debug.Log($"Partida guardada en slot {activeSlot} — Posición: {data.position}");
    }

    // ── Cargar ──────────────────────────────────────────────
    public void LoadGame()
    {
        if (!saveManager.SaveExists(activeSlot))
        {
            Debug.Log("No hay partida guardada en este slot.");
            return;
        }

        GameData data = saveManager.Load<GameData>(activeSlot);

        // Restaura la posición del jugador
        transform.position = data.position.ToVector3();

        Debug.Log($"Partida cargada → Jugador: {data.playerName} | Nivel: {data.level} | Posición: {data.position}");
    }

    // ── Borrar ──────────────────────────────────────────────
    public void DeleteSave()
    {
        saveManager.Delete(activeSlot);
        Debug.Log($"Slot {activeSlot} borrado.");
    }

    // ── Teclas de prueba rápida ──────────────────────────────
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
        if (Input.GetKeyDown(KeyCode.Delete)) DeleteSave();
    }
}

