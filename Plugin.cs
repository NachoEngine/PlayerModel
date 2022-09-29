using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using PlayerModel.Player;
using PlayerModel.Utils;
using System.Net;

namespace PlayerModel
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

        public bool Cheaking = true;

        public void Start()
        {
            if (STARTPLAYERMOD)
                return;

            Events.GameInitialized += OnGameInitialized;
        }

        string rootPath;
        public static string playerpath;
        public static string[] files;
        public static string[] fileName;
        public static string[] page;
        public static List<Material> materials = new List<Material>();
        public static Material defMat;
        public static Material matalpha;
        public static Material chestmat;
        GameObject gorillachest;
        
        void OnGameInitialized(object sender, EventArgs e)
        {
            //forcerenderingoff or switching to transparent material breaks the mod
            //this sets gorillachest standard material to transparent mode
            //uses the alpha channel to hide the gorillachest when selecting a playermodel
            
            gorillachest = GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/gorillachest");
            chestmat = gorillachest.GetComponent<Renderer>().material;
            Debug.Log("ChestMaterial Shader:  " + chestmat.shader.name);
            chestmat.SetColor("_Color", new Color(1, 1, 1, 1.0f));
            chestmat.SetFloat("_Mode", 3);
            chestmat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            chestmat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            chestmat.EnableKeyword("_ALPHABLEND_ON");
            chestmat.renderQueue = 3000;

            StartCoroutine(StartPlayerModel());
        }

        IEnumerator StartPlayerModel()
        {
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/gorilla"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/gorillachest"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/head/gorillaface"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/gorilla"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/rig/body/gorillachest"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/rig/body/head/gorillaface"));

            rootPath = Directory.GetCurrentDirectory();

            playerpath = Path.Combine(rootPath, "BepInEx", "Plugins", "PlayerModel", "PlayerAssets");

            if (!Directory.Exists(playerpath))
            {
                Directory.CreateDirectory(playerpath);
                yield return new WaitForSeconds(0.5f);

                WebClient webClient = new WebClient();

                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=17VmQyBhn78ToCASyQ0dJHuoPoHPncyAi", playerpath + @"\Amogus.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1j2ny1ohnHkUoK-yqM7OZV-zt4ZGxuu66", playerpath + @"\Babbling Baboon.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1CFIPOZ11cqXAuXlt1np4l7j3kBTGtaXO", playerpath + @"\Beautifulboy.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=18niPks_72vsBnIbaWpvT4iwD7YA7nyoA", playerpath + @"\character stump.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1tz41u9au0TxWjRFQ5sYAqp2rnoIaTmP4", playerpath + @"\Kyle The Robot.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1niqY3Rnz0VPAwNYE4gtEaGwaRGqWtTGJ", playerpath + @"\Lar Gibbon.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1aXc6nbGFQnA7R8F64LUUthhDEToLP5qF", playerpath + @"\Siamang Gibbon.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=12L-F8T_AIG8xzpdgfZ_5H0UmOOHOljoU", playerpath + @"\The Ape.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1izFz5mBLcNWUn4oq7qaPup9_CzlTRUhC", playerpath + @"\The Chimp.gtmodel");
                webClient.DownloadFile("https://drive.google.com/uc?export=download&id=1L2uTiElqeLRzC8zwFCjNRtMiO2Es14Np", playerpath + @"\Toon Gorilla.gtmodel");

                yield return new WaitForSeconds(2f);
            }

            files = Directory.GetFiles(playerpath, "*.gtmodel");//cant Path.GetFileName - cant convert string[] to string

            fileName = new string[files.Length]; //creating new array with same length as files array

            for (int i = 0; i < fileName.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]); //getting file names from directories
                Debug.Log(fileName[i]);
            }

            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerModel.Assets.PlayerModelStand");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject asset = bundle.LoadAsset<GameObject>("misc");
            var localasset = Instantiate(asset);

            DontDestroyOnLoad(localasset);

            GameObject misc = localasset.transform.GetChild(0).gameObject;

            misc.transform.position = new Vector3(-53.3f, 16.216f, -124.6f);
            misc.transform.localRotation = Quaternion.Euler(0f, -60f, 0f);

            SelectButton = misc.transform.Find("misc.selector").gameObject;
            SelectButton.AddComponent<PlayerModelButton>().button = 1;

            RightButton = misc.transform.Find("misc.rightpage").gameObject;
            RightButton.AddComponent<PlayerModelButton>().button = 2;

            LeftButton = misc.transform.Find("misc.leftpage").gameObject;
            LeftButton.AddComponent<PlayerModelButton>().button = 3;

            GameObject canvasText = misc.transform.Find("Canvas").gameObject;

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

            for (int i = 0; i < misc_orbs.Length; i++)
            {
                misc_orbs[i].AddComponent<PlayerModelButton>().button = 4 + i;
                misc_orbs[i].GetComponent<PlayerModelButton>().setColour = false;
                yield return new WaitForEndOfFrame();
            }

            materials.Add(Resources.Load<Material>($"objects/equipment/materials/" + "bluealive"));

            defMat = Resources.Load<Material>("objects/treeroom/materials/lightfur");

            mat_preview = new Material[misc_orbs.Length];

            for (int i = 0; i < mat_preview.Length; i++)
            {
                mat_preview[i] = misc_orbs[i].GetComponent<MeshRenderer>().material;
                yield return new WaitForEndOfFrame();
            }

            GameObject left_empty = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject right_empty = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //for hand placement
            left_empty.GetComponent<SphereCollider>().enabled = false;
            right_empty.GetComponent<SphereCollider>().enabled = false;

            left_empty.transform.localScale = new Vector3(.03f, .5f, .03f);
            right_empty.transform.localScale = new Vector3(.03f, .5f, .03f);

            STARTPLAYERMOD = true;
            PlayerModelController.PreviewModel(playerIndex);
            yield break;
        }

        static public Material[] mat_preview;
        static public GameObject[] misc_orbs;
        static public int mat_index;
        static public Text model_text;
        static public Text author_text;
        static public GameObject misc_preview;
        static public Material player_main_material;
        static public GameObject SelectButton;
        static public GameObject RightButton;
        static public GameObject LeftButton;

        static public bool nachotext = false;
        static public bool IsGorilla = true;
        static public int playerIndex = 0;
        static public GameObject playermodel;
        static public GameObject clone_body;
        static public bool selectflag = false;
        static public bool leftflag = false;
        static public bool rightflag = false;
        static public bool clone_body_flag = false;
        static public bool flag_inroom = true;
        static public bool STARTPLAYERMOD = false;
        static public int assignedIndex = 0;// index of array, 

        public static void SetMainDisplayButtonMaterial(Material material) => misc_orbs[0].GetComponent<MeshRenderer>().material = material;

        public static void ButtonPress(int button)
        {
            switch (button)
            {
                case 1:

                    if (nachotext == false)
                    {
                        Destroy(GameObject.Find("nachoengine_playermodelmod"));
                        nachotext = true;
                    }

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
                    else
                    {
                        if (assignedIndex == playerIndex)//playermodel to gorilla 
                        {
                            PlayerModelController.UnloadModel();
                            IsGorilla = true;
                            player_main_material = null;
                        }
                        else//playermodel to playermodel
                        {
                            PlayerModelController.UnloadModel();
                            IsGorilla = false;
                            player_main_material = null;
                            assignedIndex = playerIndex;
                        }
                    }

                    break;
                case 2:
                    playerIndex++;
                    if (playerIndex > fileName.Length - 1)
                        playerIndex = 0;
                    PlayerModelController.PreviewModel(playerIndex);

                    break;
                case 3:
                    playerIndex--;
                    if (playerIndex < 0)
                        playerIndex = fileName.Length - 1;//10 items but starts from 0 so 0 to 9 = 10 items
                    PlayerModelController.PreviewModel(playerIndex);

                    break;
                case 4:
                    PlayerModelController.player_preview.GetComponent<MeshRenderer>().material = mat_preview[0];

                    break;
                case 5:
                    PlayerModelController.player_preview.GetComponent<MeshRenderer>().material = mat_preview[1];

                    break;
                case 6:
                    PlayerModelController.player_preview.GetComponent<MeshRenderer>().material = mat_preview[2];

                    break;
                case 7:
                    PlayerModelController.player_preview.GetComponent<MeshRenderer>().material = mat_preview[3];

                    break;
            }
        }

        public float currentTime = 0;
        
        public void hidechest()
        {
            chestmat.SetColor("_Color", new Color(1, 1, 1, 0.0f));
            //Debug.Log("material set to alpha");
        }
        public void showchest()
        {
            chestmat.SetColor("_Color", new Color(1, 1, 1, 1.0f));
            //Debug.Log("material set to gorilla");
        }
        public void Update()
        {
            if (Time.time < currentTime)
                return;

            currentTime = Time.time + 1;

            if (PlayerModelController.localPositionY == 1f)
                PlayerModelController.localPositionY = -1f;
            else
                PlayerModelController.localPositionY = 1f;
        }
        
        public void FixedUpdate()
        {
            
            if (!STARTPLAYERMOD)
                return;

            if (Keyboard.current.jKey.wasPressedThisFrame)
                SelectButton.GetComponent<PlayerModelButton>().Press();

            if (Keyboard.current.hKey.wasPressedThisFrame)
                LeftButton.GetComponent<PlayerModelButton>().Press();

            if (Keyboard.current.kKey.wasPressedThisFrame)
                RightButton.GetComponent<PlayerModelButton>().Press();

            PlayerModelController.rotationY -= 0.5f;
            //Debug.Log(IsGorilla);
            if (PhotonNetwork.InRoom)
            {
                if (IsGorilla == true)//in a room, is gorilla model
                {
                    PlayerModelAppearance.ShowOnlineRig();
                    PlayerModelAppearance.ShowOfflineRig();
                    showchest();
                    flag_inroom = true;
                    PlayerModelAppearance.flag1 = true;
                    
                    clone_body = null;
                }
                else//in a room, is playermodel
                {
                    if (clone_body != null && playermodel != null)
                    {
                        PlayerModelAppearance.HideOnlineRig();
                        PlayerModelAppearance.HideOfflineRig();
                        hidechest();

                        if (PlayerModelController.CustomColors)
                            PlayerModelAppearance.AssignColor(playermodel);

                        if (PlayerModelController.GameModeTextures)
                            PlayerModelAppearance.AssignMaterial(clone_body, playermodel);
                    }

                    if (clone_body == null)
                        clone_body = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");

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
                    
                    PlayerModelAppearance.flag1 = true;
                    PlayerModelAppearance.ShowOfflineRig();
                    showchest();
                }
                else//not in a room, is playermodel
                {
                    playermodel = GameObject.Find("playermodel.body");
                    if (playermodel != null)
                    {
                        PlayerModelAppearance.ResetMaterial(playermodel);
                        PlayerModelAppearance.HideOfflineRig();
                        hidechest();
                        

                        if (PlayerModelController.CustomColors)
                            PlayerModelAppearance.AssignColor(playermodel);
                    }
                    else
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
                }

            }

        }//fixedupdate
    }
}
