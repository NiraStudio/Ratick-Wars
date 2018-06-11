using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterDataEditor : EditorWindow{

    public const string FOLDER_NAME = "DataBase";
    public const string FILE_NAME = "CharacterDataBase.asset";
    public const string FULL_PATH = @"Assets/" + FOLDER_NAME + "/" + FILE_NAME;

    public static Vector2 WindowSize= new Vector2(1200, 500);
    public static Vector2 CreateAndEditButtonSize= new Vector2(100, 250);
    public static Vector2 IconButtonSize = new Vector2(100, 100);

    static Vector2 EditScrollPos;
    CharacterDataBase dataBase;
    CharacterData temp, EditTemp;
    Texture2D tempIcon,EditTexture;
    bool create;
    int SelectedCharacter=-1;
    [MenuItem("AlphaTool/CharacterSystem")]
    public static void InIt()
    {
        CharacterDataEditor window = EditorWindow.GetWindow<CharacterDataEditor>();
        window.minSize = WindowSize; window.maxSize = WindowSize;

        window.title = "Character System";
        window.Show();
    }


    void OnEnable()
    {
        dataBase = AssetDatabase.LoadAssetAtPath(FULL_PATH, typeof(CharacterDataBase)) as CharacterDataBase;

        if (dataBase == null)
        {
            if (!AssetDatabase.IsValidFolder(@"Assets/" + FOLDER_NAME))
                AssetDatabase.CreateFolder(@"Assets", FOLDER_NAME);

            dataBase = new CharacterDataBase();
            AssetDatabase.CreateAsset(dataBase, FULL_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        temp=EditTemp = CreateInstance<CharacterData>();
        EditorUtility.SetDirty(temp); 
    }

    void OnGUI()
    {

        if (GUI.changed)
        {
            EditorUtility.SetDirty(temp);

        }
        GUILayout.BeginHorizontal("Box");

        //Create And Edit Button
            GUILayout.BeginVertical(GUILayout.Width(CreateAndEditButtonSize.x));
    
               if (GUILayout.Button("Create", GUILayout.Height(CreateAndEditButtonSize.y), GUILayout.Width(CreateAndEditButtonSize.x)))
                    create = true;
               else if (GUILayout.Button("Edit", GUILayout.Height(CreateAndEditButtonSize.y), GUILayout.Width(CreateAndEditButtonSize.x)))
                    create = false;

            GUILayout.EndVertical();


        //Create And Edit Part
            if (create)
                Create();
            else
                Edit();



        GUILayout.EndHorizontal();
    }

    void Create()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true));



        GUILayout.BeginHorizontal();// Upgrade and Detail

        #region Detail

        GUILayout.BeginVertical("Box",GUILayout.Width(500));


        #region Icon_Name_Quality_Prefab_HP

        GUILayout.BeginHorizontal();

        //-----IconButton--------

        if (temp.icon != null)
            tempIcon = temp.icon.texture;


        if (GUILayout.Button(tempIcon, GUILayout.Width(IconButtonSize.x), GUILayout.Height(IconButtonSize.y)))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, 0);
        }
        string commend = Event.current.commandName;
        if (commend == "ObjectSelectorClosed")
        {
            temp.icon = (Sprite)EditorGUIUtility.GetObjectPickerObject();
        }
        //---------------------------


        GUILayout.BeginVertical();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Name Code:");
        temp.characterNameCode = GUILayout.TextField(temp.characterNameCode, GUILayout.Width(100));
        GUILayout.Label("Character Quality:");
        temp.Quality = (CharacterQuality)EditorGUILayout.EnumPopup(temp.Quality, GUILayout.Width(100));

        GUILayout.EndHorizontal();

        GUILayout.Space(25);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Prefab:");
        temp.prefab = EditorGUILayout.ObjectField(temp.prefab, typeof(GameObject), false) as GameObject;
        GUILayout.Label("HitPoint:");
        temp.hp = EditorGUILayout.IntField( temp.hp);
        GUILayout.Label("Character Type:");
        temp.Type = (CharacterType)EditorGUILayout.EnumPopup(temp.Type, GUILayout.Width(100));
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();



        GUILayout.EndHorizontal();





        #endregion

        GUILayout.Space(15);

        #region Range_Damage_AttackSpeed_Speed

        GUILayout.BeginHorizontal();

        GUILayout.Label("Attack Speed:");
        temp.attackSpeed = EditorGUILayout.FloatField( temp.attackSpeed);
        GUILayout.Space(15);

        GUILayout.Label("Speed:");
        temp.speed = EditorGUILayout.FloatField(temp.speed);
        GUILayout.Space(15);

        GUILayout.Label("Damage:");
        temp.damage = EditorGUILayout.FloatField(temp.damage);
        GUILayout.Space(15);

        GUILayout.Label("Range:");
        temp.range = EditorGUILayout.FloatField(temp.range);
        GUILayout.Space(15);
        


        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(15);

        #region StartSpawn_IncreasePerLevel

        GUILayout.BeginHorizontal();

        GUILayout.Label("Start Spawn Count:");
        temp.startSpawnCount = EditorGUILayout.IntSlider(temp.startSpawnCount, 3, 10);
        GUILayout.Space(15);

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.EndVertical();


        #endregion

        #region Upgrade

        #endregion

        GUILayout.EndHorizontal();


        if (GUILayout.Button("Create Character",GUILayout.Height(75),GUILayout.ExpandWidth(true)))// Create Button
        {
            #region AddCharacter And Clean Temp
            CharacterData a = ScriptableObject.CreateInstance<CharacterData>();
            a.characterNameCode = temp.characterNameCode;
            a.prefab = temp.prefab;
            a.id = dataBase.IDGiver;
            a.icon = temp.icon;
            a.speed = temp.speed;
            a.Quality = temp.Quality;
            a.attackSpeed = temp.attackSpeed;
            a.hp = temp.hp;
            a.damage = temp.damage;
            a.startSpawnCount = temp.startSpawnCount;
            a.Code = temp.Code;
            a.range = temp.range;

            string path = @"Assets/Data/CharacterData/";
            if (!AssetDatabase.IsValidFolder(@"Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");

            if (!AssetDatabase.IsValidFolder(@"Assets/Data/" + "CharacterData"))
                AssetDatabase.CreateFolder(@"Assets/Data", "CharacterData");
            string b;
            if (a.characterNameCode != null)
                b = temp.characterNameCode;
            else
                b = "New character data";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + b + ".asset");

            AssetDatabase.CreateAsset(a, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            dataBase.AddCharacter(a);
            temp = CreateInstance<CharacterData>();
            EditorUtility.SetDirty(temp);
            #endregion
        }

        
        GUILayout.EndVertical();
    }

    void Edit()
    {
        GUILayout.BeginVertical();
        #region EditPart
        if (EditTemp != null)
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));



            GUILayout.BeginHorizontal();// Upgrade and Detail

            #region Detail

            GUILayout.BeginVertical("Box", GUILayout.Width(500));


            #region Icon_Name_Quality_Prefab_HP

            GUILayout.BeginHorizontal();

            //-----IconButton--------

            if (EditTemp.icon != null)
                tempIcon = EditTemp.icon.texture;
            else
                tempIcon = null;

            if (GUILayout.Button(tempIcon, GUILayout.Width(IconButtonSize.x), GUILayout.Height(IconButtonSize.y)))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, 0);
            }
            string commend = Event.current.commandName;
            if (commend == "ObjectSelectorClosed")
            {
                EditTemp.icon = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            }
            //---------------------------


            GUILayout.BeginVertical();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Character Name Code:");
            EditTemp.characterNameCode=EditTemp.name = GUILayout.TextField(EditTemp.characterNameCode, GUILayout.Width(100));
            EditTemp.name = EditTemp.characterNameCode;
            GUILayout.Label("Character Quality:");
            EditTemp.Quality = (CharacterQuality)EditorGUILayout.EnumPopup(EditTemp.Quality, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            GUILayout.Space(25);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Character Prefab:");
            EditTemp.prefab = EditorGUILayout.ObjectField(EditTemp.prefab, typeof(GameObject), false) as GameObject;
            GUILayout.Label("HitPoint:");
            EditTemp.hp = EditorGUILayout.IntField(EditTemp.hp);
            GUILayout.Label("Character Type:");
            temp.Type = (CharacterType)EditorGUILayout.EnumPopup(temp.Type, GUILayout.Width(100));
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();



            GUILayout.EndHorizontal();





            #endregion

            GUILayout.Space(15);

            #region Range_Damage_AttackSpeed_Speed

            GUILayout.BeginHorizontal();

            GUILayout.Label("Attack Speed:");
            EditTemp.attackSpeed = EditorGUILayout.FloatField(EditTemp.attackSpeed);
            GUILayout.Space(15);

            GUILayout.Label("Speed:");
            EditTemp.speed = EditorGUILayout.FloatField(EditTemp.speed);
            GUILayout.Space(15);

            GUILayout.Label("Damage:");
            EditTemp.damage = EditorGUILayout.FloatField(EditTemp.damage);
            GUILayout.Space(15);

            GUILayout.Label("Range:");
            EditTemp.range = EditorGUILayout.FloatField(EditTemp.range);
            GUILayout.Space(15);



            GUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(15);

            #region StartSpawn_IncreasePerLevel

            GUILayout.BeginHorizontal();

            GUILayout.Label("Start Spawn Count:");
            EditTemp.startSpawnCount = EditorGUILayout.IntSlider(EditTemp.startSpawnCount, 3, 10);
            GUILayout.Space(15);


            GUILayout.EndHorizontal();

            #endregion

            GUILayout.EndVertical();


            #endregion

            #region Upgrade

            #endregion

            GUILayout.EndHorizontal();




            GUILayout.EndVertical();
        }
        #endregion

       EditScrollPos= GUILayout.BeginScrollView(EditScrollPos,"Box",GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        for (int i = 0; i < dataBase.Count; i++)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(IconButtonSize.x));

            #region Select_Delete_BTN
            GUILayout.BeginHorizontal();
            if (dataBase.GiveByIndex(i).icon != null)
                EditTexture = dataBase.GiveByIndex(i).icon.texture;
            else
                EditTexture = null;
            float m = i == SelectedCharacter ? 1.5f : 1;
            if (GUILayout.Button(EditTexture, GUILayout.Width(IconButtonSize.x *m), GUILayout.Height(IconButtonSize.y * m)))
            {
                EditTemp = dataBase.GiveByIndex(i);
                SelectedCharacter = i;
            }
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20))){
                if (EditorUtility.DisplayDialog("Delete Character", "Are you sure you want to delete " + dataBase.GiveByIndex(i).characterNameCode + "?", "Yes", "No"))
                {
                    AssetDatabase.DeleteAsset(@"Assets/Data/CharacterData/" + dataBase.GiveByIndex(i).name + ".asset");
                    dataBase.RemoveCharacter(i);
                    return;

                }
            }
            GUILayout.EndHorizontal();
            #endregion


            GUILayout.Label(dataBase.GiveByIndex(i).characterNameCode);
            GUILayout.EndVertical();


            GUILayout.Space(15);
        }
        GUILayout.Space(30);

        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

        GUILayout.EndVertical();
        dataBase.setDirty();
        EditTemp.setDirty();
    }
}
