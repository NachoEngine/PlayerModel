using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Reflection;
using System.IO;
using Photon.Pun;
using showgorilla;
using UnityEngine.UI;


namespace PlayerModel
{

    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

        public bool Cheaking = true;

        GameObject gorillabody;//base gorilla gameobject

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

            files = Directory.GetFiles(playerpath , "*.gtmodel");//cant Path.GetFileName - cant convert string[] to string
            foreach (var file in files)
            {
                Debug.Log(file);
            }
            fileName = new string[files.Length]; //creating new array with same length as files array
            for(int i=0; i<fileName.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]); //getting file names from directories
                //Debug.Log("Found "+fileName[i]+" Player model");
            }

            gorillabody = GameObject.Find("OfflineVRRig/Actual Gorilla/gorilla");
            
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerModel.Assets.misc");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject asset = bundle.LoadAsset<GameObject>("misc");
            var localasset = Instantiate(asset);

            Debug.Log("loaded misc");

            GameObject misc = localasset.transform.GetChild(0).gameObject;
            Debug.Log("found misc child");

            misc.transform.position = new Vector3(-53.3f, 16.216f, -124.6f);
            misc.transform.localRotation = Quaternion.Euler(0f, -60f, 0f);
            

            
            SelectButton = misc.transform.Find("misc.selector").gameObject;
            ButtonTrigger.AssignButton(SelectButton);

            RightButton = misc.transform.Find("misc.rightpage").gameObject;
            ButtonTrigger.AssignButton(RightButton);

            LeftButton = misc.transform.Find("misc.leftpage").gameObject;
            ButtonTrigger.AssignButton(LeftButton);
            Debug.Log("Find Canvas");
            GameObject canvasText = misc.transform.Find("Canvas").gameObject;
            Debug.Log("found canvas");
            
            GameObject modelText = canvasText.transform.Find("model.text").gameObject;
            GameObject authorText = canvasText.transform.Find("author.text").gameObject;

            model_text = modelText.GetComponent<Text>();
            author_text = authorText.GetComponent<Text>();
            
            
            misc_preview = GameObject.Find("misc.preview");

            misc_orbs = new GameObject[4];
            misc_orbs[0] = GameObject.Find("misc.fur");
            misc_orbs[1] = GameObject.Find("misc.lava");
            misc_orbs[2] = GameObject.Find("misc.rock");
            misc_orbs[3] = GameObject.Find("misc.ice");
            
            for(int i=0; i<misc_orbs.Length; i++)
            {
                ButtonTrigger.AssignButton(misc_orbs[i]);
            }
            
            mat_preview = new Material[misc_orbs.Length];
            
            for(int i=0;i< mat_preview.Length; i++)
            {
                mat_preview[i] = misc_orbs[i].GetComponent<MeshRenderer>().material;
            }
            
            GameObject left_empty = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject right_empty = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //for hand placement
            left_empty.GetComponent<SphereCollider>().enabled = false;
            right_empty.GetComponent<SphereCollider>().enabled = false;

            left_empty.transform.localScale = new Vector3(.03f, .5f, .03f);
            right_empty.transform.localScale = new Vector3(.03f, .5f, .03f);
            
            GameObject hand_l = GameObject.Find("hand.L");
            GameObject hand_r = GameObject.Find("hand.R");

            Quaternion rotL = Quaternion.Euler(hand_l.transform.rotation.x, hand_l.transform.rotation.y, hand_l.transform.rotation.z + 27f);

            Quaternion rotR = Quaternion.Euler(hand_r.transform.rotation.x, hand_r.transform.rotation.y, hand_r.transform.rotation.z - 27f);


            

            STARTPLAYERMOD = true;
            if (STARTPLAYERMOD)
            {
                PlayerModelController.PreviewModel(playerIndex);
            }
            
        }
        static public Material[] mat_preview;
        static public GameObject[] misc_orbs;
        static public int mat_index;
        static public Text model_text;
        static public Text author_text;
        static public GameObject misc_preview;
        static public Material player_main_material;
        public GameObject SelectButton;
        public GameObject RightButton;
        public GameObject LeftButton;

        public bool nachotext = false;
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
                            
                            Showgorilla.HideOnlineRig();
                            Showgorilla.HideOfflineRig();

                            
                            if (PlayerModelController.CustomColors)
                            {
                                
                                Showgorilla.AssignColor(playermodel);
                            }

                            if (PlayerModelController.GameModeTextures)
                            {
                                Showgorilla.AssignMaterial(clone_body, playermodel);
                            }
                            
                            
                        }
                        if (clone_body == null)
                        {
                            clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
                            
                        }
                        if (playermodel == null)
                        {
                            while (playermodel == null)
                            {
                                PlayerModelController.LoadModel(assignedIndex);
                                playermodel = GameObject.Find("playermodel.body");
                                
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
                        Showgorilla.ResetMaterial(playermodel);
                        Showgorilla.HideOfflineRig();
                        if (PlayerModelController.CustomColors)
                        {

                            Showgorilla.AssignColor(playermodel);
                        }
                        
                    }

                }

                if (SelectButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    if(nachotext == false)
                    {
                        Destroy(GameObject.Find("nachoengine_playermodelmod"));
                        nachotext = true;
                    }
                    SelectButton.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    if (selectflag == true)
                    {
                        selectflag = false;
                        if (IsGorilla == true)//switching from gorilla to playermodel
                        {
                            while (playermodel == null)
                            {
                                Debug.Log("Gorilla to player model loading");
                                PlayerModelController.LoadModel(playerIndex);
                                playermodel = GameObject.Find("playermodel.body");
                                
                            }
                            assignedIndex = playerIndex;
                            IsGorilla = false;
                            Debug.Log("Gorilla to player model loaded");
                            PlayerModelController.AssignModel();
                        }
                        else//model to model
                        {
                            if (assignedIndex == playerIndex)
                            {
                                PlayerModelController.UnloadModel(assignedIndex);
                                IsGorilla = true;
                                player_main_material = null;
                            }
                            else
                            {
                                player_main_material = null;
                                PlayerModelController.UnloadModel(assignedIndex);

                                while (playermodel == null)
                                {
                                    PlayerModelController.LoadModel(playerIndex);
                                    playermodel = GameObject.Find("playermodel.body");
                                    
                                }
                                assignedIndex = playerIndex;
                                Debug.Log(player_main_material.name);
                                

                            }

                        }    
                    }
                }
                else
                {
                    SelectButton.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    selectflag = true;
                }

                if (LeftButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    
                    LeftButton.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    if (leftflag == true)
                    {
                        leftflag = false;
                        playerIndex--;
                        if (playerIndex < 0)
                        {
                            playerIndex = fileName.Length - 1;//10 items but starts from 0 so 0 to 9 = 10 items
                        }

                        PlayerModelController.PreviewModel(playerIndex);
                        
                    }
                }
                else
                {
                    LeftButton.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    leftflag = true;
                }
                
                if (RightButton.GetComponent<ButtonTrigger>().pressed == true)
                {
                    
                    RightButton.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    if (rightflag == true)
                    {
                        rightflag = false;
                        playerIndex++;
                        if (playerIndex > fileName.Length - 1)
                        {
                            playerIndex = 0;
                        }

                        PlayerModelController.PreviewModel(playerIndex);

                    }
                }
                else
                {
                    RightButton.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    rightflag = true;
                }


                //preview materials buttons

                for(int i=0; i < misc_orbs.Length; i++)
                {
                    if(misc_orbs[i].GetComponent<ButtonTrigger>().pressed == true)
                    {
                        PlayerModelController.player_preview.GetComponent<MeshRenderer>().material = mat_preview[i];
                    }
                }
                 //hi <3
            }

        }//fixedupdate


    }
}
