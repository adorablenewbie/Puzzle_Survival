using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum SceneType
{
    MainScene,
    PuzzleScene
}

[System.Serializable]
public class ObjectData
{
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public string prefabName; // 어떤 프리팹인지
}

public class SaveNPC : MonoBehaviour
{
    private string savePath => Application.persistentDataPath;

    public void Save(GameObject obj, SceneType scene)
    {
        ObjectData data = new ObjectData();
        data.posX = obj.transform.position.x;
        data.posY = obj.transform.position.y;
        data.posZ = obj.transform.position.z;
        data.rotX = obj.transform.eulerAngles.x;
        data.rotY = obj.transform.eulerAngles.y;
        data.rotZ = obj.transform.eulerAngles.z;
        data.prefabName = obj.name.Replace("(Clone)", "");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText($"{savePath}/save{scene}.json", json);
    }

    public GameObject Load(SceneType scene)
    {
        if (!File.Exists($"{savePath}/save{scene}.json")) return null;

        string json = File.ReadAllText($"{savePath}/save{scene}.json");
        ObjectData data = JsonUtility.FromJson<ObjectData>(json);

        GameObject prefab = Resources.Load<GameObject>("NPC/"+data.prefabName);
        if (prefab == null) return null;

        GameObject obj = Instantiate(prefab);
        obj.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        obj.transform.eulerAngles = new Vector3(data.rotX, data.rotY, data.rotZ);

        return obj;
    }
}
