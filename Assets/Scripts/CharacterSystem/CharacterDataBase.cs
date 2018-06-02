using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataBase : ScriptableObject {
    public List<CharacterData> DataBase = new List<CharacterData>();
    public int Count
    {
        get { return DataBase.Count; }
    }
    public CharacterData GiveByID(int id)
    {
        CharacterData data = CreateInstance(typeof(CharacterData)) as CharacterData;
        bool found = false;
        foreach (var item in DataBase.ToArray())
        {
            if (item.id == id)
            {
                data = item;
                found = true;
                break;
            }
        }
        if (found)
            return data;
        else
            return null;
    }
    public CharacterData GiveByIndex(int i)
    {
        return DataBase[i];
    }
    public void AddCharacter(CharacterData data)
    {
        DataBase.Add(data);
        setDirty();
    }
    public void RemoveCharacter(CharacterData data)
    {
        DataBase.Remove(data);
        setDirty();

    }
    public int IDGiver
    {
        get
        {
            int a=1;
            if (DataBase.Count > 0)
            {
                a = DataBase[0].id;
                for (int i = 0; i < Count; i++)
                {
                    if (DataBase[i].id > a)
                        a = DataBase[i].id;
                }
                a++;
            }
            return a;
        }
    }

    public void RemoveCharacter(int Index)
    {
        DataBase.Remove(DataBase[Index]);
    }
   /* public CharacterData GiveByRandom()
    {
        CharacterData d = CreateInstance(typeof(CharacterData)) as CharacterData;
        do
        {
            d = DataBase[Random.Range(0, DataBase.Count)];
        } while (GameManager.instance.DoesPlayerHasThisCharacter(d.id) == false);
        return d;
    }*/

    public List<CharacterData> GiveByQuality(CharacterQuality type)
    {
        List<CharacterData> answer = new List<CharacterData>();
        foreach (var item in DataBase)
        {
            if (item.Quality == type)
                answer.Add(item);
        }
        return answer;
    }
    public CharacterQuality giveCharacterQuality(int id)
    {
        return GiveByID(id).Quality;
    }
   /* public CharacterData GiveNewCharacter()
    {
        CharacterData d = null;
        List<CharacterData> data = new List<CharacterData>();
        foreach (var item in DataBase.ToArray())
        {
            if (!GameManager.instance.DoesPlayerHasThisCharacter(item.id))
            {
                data.Add(item);
            }
        }
        if (data.Count > 0)
        {
            d = data[Random.Range(0, data.Count)];
        }
        else
            d = DataBase[Random.Range(0, data.Count)];

        return d;
    }*/
    public void setDirty()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
