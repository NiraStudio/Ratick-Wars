using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Character")]
[System.Serializable]
public class CharacterData :ScriptableObject{
    public string characterNameCode;
    public CharacterQuality Quality;
    public Sprite icon;
    public GameObject prefab;
    public int id,hp;
    public float damage,attackSpeed,range,speed;
    [Range(3,10)]
    public int startSpawnCount, increaseCount;
    public string Code;

    public void setDirty()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

public enum CharacterQuality
{
    Common,Rare,Epic
}
