using BepInEx;
using System;
using UnityEngine;
using Utilla;
using DitzelGames.FastIK;
using System.Reflection;
using System.IO;
using Photon.Pun;
using showgorilla;
using UnityEngine.UI;

namespace PlayerModel
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        public bool Cheaking = true;
        // private static GameObject banana;
        public static readonly string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#pragma warning disable IDE0051 // IDE0051: Remove unused member


        

        GameObject gorillabody;//base gorilla gameobject

        
        void Awake()
        {
            Utilla.Events.RoomJoined += RoomJoined;
            Utilla.Events.RoomLeft += RoomLeft;

            
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        string rootPath;
        public static string playerpath;
        public static string[] files;
        public static string[] fileName;
        public static string[] page;

        void OnGameInitialized(object sender, EventArgs e)
        {
            rootPath = Directory.GetCurrentDirectory();

            playerpath = Path.Combine(rootPath, "BepInEx", "Plugins", "PlayerModel", "PlayerAssets");

            if(!Directory.Exists(playerpath))
            {
                Directory.CreateDirectory(playerpath);
            }

            files = Directory.GetFiles(playerpath);//cant Path.GetFileName - cant convert string[] to string
            foreach (var file in files)
            {
                Debug.Log(file);
            }
            fileName = new string[files.Length]; //creating new array with same length as files array
            for(int i=0; i<fileName.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]); //getting file names from directories
                Debug.Log("Found "+fileName[i]+" Player model");
            }

            gorillabody = GameObject.Find("OfflineVRRig/Actual Gorilla/gorilla");
            
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerModel.Assets.misc");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            Debug.Log("loaded from str");
            GameObject asset = bundle.LoadAsset<GameObject>("pm_stand");
            Instantiate(asset);
            Debug.Log("stand loaded");
            GameObject pm_stand = GameObject.Find("GorillaTag_PlayerModelMod_Stand");
            if (pm_stand == null)
            {
                Debug.LogError("cant find pm_stand");
            }
            Debug.Log("Found pm_stand");
            pm_stand.transform.position = new Vector3(-53.3f, 16.216f, -124.6f);
            pm_stand.transform.localRotation = Quaternion.Euler(-90f, -60f, 0f);
            pm_stand.AddComponent<MeshCollider>();

            Debug.Log("set stand transform");




            SelectButton = GameObject.Find("selector");
            GameObject Select_on = GameObject.Find("Select_on");
            ButtonTrigger.AssignButton(SelectButton, Select_on);

            Debug.Log("select button set");

            RightButton = GameObject.Find("right_button");
            GameObject RightButton_on = GameObject.Find("RightButton_on");
            
            ButtonTrigger.AssignButton(RightButton, RightButton_on);
            
            Debug.Log("rightButton set");

            LeftButton = GameObject.Find("left_button");
            GameObject LeftButton_on = GameObject.Find("LeftButton_on");
            ButtonTrigger.AssignButton(LeftButton, LeftButton_on);

            Debug.Log("leftButton set");
            STARTPLAYERMOD = true;
        }


        
        public GameObject SelectButton;
        public GameObject RightButton;
        public GameObject LeftButton;

        
        public bool IsGorilla = true;
        public int playerIndex = 0;
        public GameObject playermodel;
        public GameObject clone_body;
        public bool selectflag = false;
        public bool leftflag = false;
        public bool rightflag = false;
        public bool clone_body_flag = false; 
        public bool flag_inroom = true;
        public bool STARTPLAYERMOD = false;
        public int assignedIndex = 0;// index of array, 
        void FixedUpdate()
        {
            if (STARTPLAYERMOD == true)
            {


                if (PhotonNetwork.InRoom)
                {
                    if (IsGorilla == true)//in a room, is gorilla model
                    {

                        flag_inroom = true;
                        Showgorilla.flag1 = true;

                        Showgorilla.ShowOnlineRig();
                        Showgorilla.ShowOfflineRig();
                        clone_body = null;
                    }
                    else//in a room, is playermodel
                    {
                        if (clone_body != null && playermodel != null)
                        {
                            Debug.Log("Found clone_body and playermodel");
                            Showgorilla.HideOnlineRig();
                            Showgorilla.HideOfflineRig();

                            Showgorilla.AssignColor(playermodel);
                            Showgorilla.AssignMaterial(clone_body, playermodel);
                        }
                        if (clone_body == null)
                        {
                            clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
                            Debug.Log("Finding clone_body");
                        }
                        if (playermodel == null)
                        {
                            while (playermodel == null)
                            {
                                PlayerModelController.LoadModel(assignedIndex);
                                playermodel = GameObject.Find("playermodel.body");
                                Debug.Log("Finding playermodel");
                                PlayerModelController.AssignModel();
                            }


                        }



                    }
                }
                else if (!PhotonNetwork.InRoom)
                {
                    flag_inroom = false;
                    clone_body = null;
                    if (IsGorilla == true)//not in a room, is gorilla model
                    {
                        Showgorilla.flag1 = true;
                        Showgorilla.ShowOfflineRig();
                    }
                    else//not in a room, is playermodel
                    {
                        Showgorilla.HideOfflineRig();
                        Showgorilla.AssignColor(playermodel);
                        Showgorilla.ResetMaterial(playermodel); //not in a lobby, reset back to darkfur material
                    }

                }



                if (SelectButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    if (selectflag == true)
                    {
                        selectflag = false;
                        if (IsGorilla == true)//switching from gorilla to playermodel
                        {


                            while (playermodel == null)
                            {
                                PlayerModelController.LoadModel(playerIndex);
                                playermodel = GameObject.Find("playermodel.body");
                            }
                            assignedIndex = playerIndex;
                            IsGorilla = false;
                            PlayerModelController.AssignModel();





                        }
                        else//model to model
                        {
                            if (assignedIndex == playerIndex)
                            {
                                PlayerModelController.UnloadModel(assignedIndex);
                                IsGorilla = true;
                            }
                            else
                            {
                                PlayerModelController.UnloadModel(assignedIndex);

                                while (playermodel == null)
                                {
                                    PlayerModelController.LoadModel(playerIndex);
                                    playermodel = GameObject.Find("playermodel.body");
                                }
                                assignedIndex = playerIndex;
                                Debug.Log("Model to model");
                            }


                        }

                        Debug.Log("Select Button Pressed");
                    }
                }
                else
                {

                    selectflag = true;
                }




                //




                if (LeftButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    if (leftflag == true)
                    {
                        leftflag = false;
                        playerIndex--;
                        if (playerIndex < 0)
                        {
                            playerIndex = fileName.Length - 1;//10 items but starts from 0 so 0 to 9 = 10 items
                        }


                        Debug.Log("Left Button Pressed");
                    }
                }
                else
                {
                    leftflag = true;
                }

                if (RightButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    if (rightflag == true)
                    {
                        rightflag = false;
                        playerIndex++;
                        if (playerIndex > fileName.Length - 1)
                        {
                            playerIndex = 0;
                        }


                        Debug.Log("Right Button Pressed");
                    }
                }
                else
                {
                    rightflag = true;
                }
            }
        }//fixedupdate


        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
    

        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }

        private void RoomJoined(object sender, Events.RoomJoinedArgs e)
        {
            Debug.Log($"Private room: {e.isPrivate}, Gamemode: {e.Gamemode}");
        }

        private void RoomLeft(object sender, Events.RoomJoinedArgs e)
        {
            Debug.Log($"Private room: {e.isPrivate}, Gamemode: {e.Gamemode}");
        }
    }
}
