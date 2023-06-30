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
using System.Linq;
using UnityEngine.Audio;


namespace PlayerModel
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.8")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        static public AudioClip _audioClip;
        static public bool _useMic = true;
        static public string _selectedDevice;
        static public AudioMixerGroup _mixerGroupMic;
        static public bool Cheaking = true;
        void Awake()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }
        

        string rootPath;
        public static string textSavePath;
        public static string playerpath;
        public static string[] files;
        public static string[] fileName;
        public static string[] page;
        public static List<Material> materials = new List<Material>();
        public static Material defMat;
        public static Material matalpha;
        
        
        
        public void OnGameInitialized(object sender, EventArgs e)
        {
            
            //LipSyncStart();
            for (int thisReading = 0; thisReading < samples; thisReading++)
            {
                readings[thisReading] = 0;
            }

            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/gorilla"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/gorillachest"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/head/gorillaface"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/gorilla"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/rig/body/gorillachest"));
            PlayerModelAppearance.playerGameObjects.Add(GameObject.Find("Global/Local VRRig/Actual Gorilla/rig/body/head/gorillaface"));
            PlayerModelAppearance.gorillabody = GorillaTagger.Instance.offlineVRRig.mainSkin.gameObject;

            
            rootPath = Directory.GetCurrentDirectory();

            playerpath = Path.Combine(rootPath, "BepInEx", "Plugins", "PlayerModel", "PlayerAssets");
            textSavePath = Path.Combine(rootPath, "BepInEx", "Plugins", "PlayerModel", textfilename);
            //preload gtmodels to PlayerAssets folder
            if (!Directory.Exists(playerpath))
            {
                //stores list of all embeded files from dll
                List<string> embededmodels = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList();

                //removes not gtmodels from list
                string type = ".gtmodel";
                for (int i = 0; i < embededmodels.Count; i++)
                {

                    if (!embededmodels[i].EndsWith(type))
                    {
                        embededmodels.Remove(embededmodels[i]);
                    }
                }

                Directory.CreateDirectory(playerpath);
                //stores embeded models to folder
                foreach (String model in embededmodels)
                {
                    //Debug.Log(model);

                    string filename = model.Replace("PlayerModel.PlayerAssets.", "");
                    MemoryStream ms = new MemoryStream();   // buffer for file bytes
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(model))  // open resource
                    {
                        stream.CopyTo(ms);   // copy to buffer
                        byte[] bb = ms.ToArray();   // need array to save
                        File.WriteAllBytes(playerpath + @"\" + filename, bb);   // save byte array to file

                    }
                }

            }

            files = Directory.GetFiles(playerpath, "*.gtmodel");//cant Path.GetFileName - cant convert string[] to string

            fileName = new string[files.Length]; //creating new array with same length as files array

            for (int i = 0; i < fileName.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]); //getting file names from directories
                //Debug.Log(fileName[i]);
            }
            //Debug.Log("Loading MISC");
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerModel.Assets.playermodelstand");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject asset = bundle.LoadAsset<GameObject>("misc");
            var localasset = Instantiate(asset);

            DontDestroyOnLoad(localasset);
            //Debug.Log("MISC Loaded");
            GameObject misc = localasset.transform.GetChild(0).gameObject;

            misc.transform.position = new Vector3(-53.3f, 16.216f, -124.6f);
            misc.transform.localRotation = Quaternion.Euler(0f, -60f, 0f);
            //Debug.Log("MISC Child");
            SelectButton = misc.transform.Find("misc.selector").gameObject;
            //Debug.Log("MISC selectbutton");
            SelectButton.AddComponent<PlayerModelButton>().button = 1;
            //Debug.Log("MISC select add");
            RightButton = misc.transform.Find("misc.rightpage").gameObject;
            RightButton.AddComponent<PlayerModelButton>().button = 2;

            LeftButton = misc.transform.Find("misc.leftpage").gameObject;
            LeftButton.AddComponent<PlayerModelButton>().button = 3;
            //Debug.Log("Buttons Loaded");
            GameObject canvasText = misc.transform.Find("Canvas").gameObject;

            GameObject modelText = canvasText.transform.Find("model.text").gameObject;
            GameObject authorText = canvasText.transform.Find("author.text").gameObject;
            GameObject versionText = canvasText.transform.Find("version.text").gameObject;

            versionText.GetComponent<Text>().text = "V" + PluginInfo.Version;

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
                
            }

            materials.Add(Resources.Load<Material>($"objects/equipment/materials/" + "bluealive"));

            defMat = Resources.Load<Material>("objects/treeroom/materials/lightfur");

            mat_preview = new Material[misc_orbs.Length];

            for (int i = 0; i < mat_preview.Length; i++)
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

            

            if (File.Exists(textSavePath))
            {
                string[] defaultdata = readFromText(textSavePath);

                //Debug.Log("Checking Default PlayerModel Data");
                //Debug.Log(defaultdata[1]);
                if (defaultdata[1] == Convert.ToString(0))//If IsGorilla
                {
                    PlayerModelController.PreviewModel(Convert.ToInt32(defaultdata[0]));
                    if (defaultdata[2] == model_text.text)//matching playermodel name
                    {
                        Debug.Log("Loading Default PlayerModel");
                        playerIndex = Convert.ToInt32(defaultdata[0]);
                        PlayerModelController.PreviewModel(playerIndex);
                        IsGorilla = false;
                    }

                }
                else
                {
                    //Debug.Log("Loading as Gorilla");
                    playerIndex = 0;
                    PlayerModelController.PreviewModel(0);
                    IsGorilla = true;
                }

            }
            STARTPLAYERMOD = true;

        }
        

        public static AudioSource _audioSource;
        public static bool vflag = true;
        public static GameObject voiceprefab;
        public static AudioMixer _audioMixer;

        public static GameObject voiceObject;

        public static int clipPosition;

        public static void restartMic()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            _audioSource.clip = Microphone.Start(_selectedDevice, true, 5, AudioSettings.outputSampleRate);
            _audioSource.Play();
        }
        static public void LipSyncStart()
        {
            if (_useMic)
            {
                if (Microphone.devices.Length > 0)
                {
                    voiceObject = new GameObject();
                    _audioSource = voiceObject.AddComponent<AudioSource>();

                    _selectedDevice = Microphone.devices[0].ToString();
                    Debug.Log(_selectedDevice);
                    _audioSource.clip = Microphone.Start(_selectedDevice, true, 5, AudioSettings.outputSampleRate);

                    _audioSource.spatialBlend = 1f;
                    _audioSource.minDistance = 0f;
                    _audioSource.maxDistance = 1f;
                    _audioSource.playOnAwake = true;
                    _audioSource.loop = true;
                    _audioSource.Play();

                    voiceObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    voiceObject.GetComponent<SphereCollider>().enabled = false;
                    voiceObject.GetComponent<MeshRenderer>().enabled = false;
                    voiceObject.transform.position = new Vector3(-66, 12, -82);

                }
                else
                {
                    _useMic = false;
                    Debug.LogError("Micropohone is missing kwjebfiwujeenbfiubwefijbwefijbwefijwbefijwbefijwbefijwbefijwbefijwbef");
                }
            }
        }
        public static GameObject mouth;
        public static int sampleWindow = 64;

        public static float loudnessSensitivity = 100;
        public static float threshold = 0.2f;

        public static AudioClip MicClip;
        public static float GetLoudnessFromMic()
        {
            return AudioClipLoudness(Microphone.GetPosition(Microphone.devices[0]), _audioSource.clip);
        }
        static public float AudioClipLoudness(int clipPosition, AudioClip clip)
        {
            int startPosition = clipPosition - sampleWindow;

            if (startPosition < 0)
                return 0;

            float[] waveData = new float[sampleWindow];
            clip.GetData(waveData, startPosition);

            //compute loudness

            float totalLoudness = 0;

            for (int i = 0; i < sampleWindow; i++)
            {
                totalLoudness += Mathf.Abs(waveData[i]);

            }

            return totalLoudness / sampleWindow;
        }
        
        static string textfilename = "PlayerModelDefault.pmdefault";
        static string split = ",";
        public static void writeToText(int index_, int IsGorilla_, string name)
        {
            
            string text = index_ + split + IsGorilla_ + split + name;
            File.WriteAllText(textSavePath, text);
        }

        public static string[] readFromText(string path)
        {
            
            string text = File.ReadAllText(textSavePath);
            string[] strings = text.Split(',');
            
            
            return strings;

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
                    Debug.Log("Selector Button Pressed");
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
                    else//playermodel is assaigned, and spressed select button
                    {
                        
                        if (assignedIndex == playerIndex)//playermodel to gorilla 
                        {
                            Debug.Log("PlayerModel to Gorilla");
                            PlayerModelController.UnloadModel();
                            IsGorilla = true;
                            player_main_material = null;
                        }
                        else//playermodel to playermodel
                        {
                            Debug.Log("PlayerModel to PlayerModel");
                            PlayerModelController.UnloadModel();
                            player_main_material = null;

                            
                            assignedIndex = playerIndex;
                            
                            
                        }
                    }
                    //writeToText(assignedIndex, Convert.ToInt32(IsGorilla));
                    writeToText(assignedIndex, Convert.ToInt32(IsGorilla), model_text.text);

                    break;
                case 2:
                    playerIndex++; //righjt
                    Debug.Log("Right");
                    
                    if (playerIndex > fileName.Length-1)
                        playerIndex = 0;
                    PlayerModelController.PreviewModel(playerIndex);

                    break;
                case 3:
                    playerIndex--;
                    Debug.Log("Left");
                    if (playerIndex < 0)
                        playerIndex = fileName.Length-1;//10 items but starts from 0 so 0 to 9 = 10 items
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
        
        

        public float voiceDelay = 1;
        public const int samples = 10;

        float[] readings = new float[samples];
        int readIndex = 0;
        float total = 0;
        public float avg = 0;

        public void voiceSmoothing()
        {
            
            total -= readings[readIndex];
            
            readings[readIndex] = loudness;
            // add the reading to the total:
            total = total + readings[readIndex];
            // advance to the next position in the array:
            readIndex++;

            // if we're at the end of the array...
            if (readIndex >= samples)
            {
                // ...wrap around to the beginning:
                
                readIndex = 0;
            }

            // calculate the average:
            avg = total / samples;
            
        }
        public static float loudness;

        public float LipSyncWeight = 0;
        
        public bool LipSyncFlag = true;
        private void Update()
        {
            if (!STARTPLAYERMOD)
            {
                return;
            }
            
            /*loudness = GetLoudnessFromMic() * loudnessSensitivity;

            voiceSmoothing();

            if (avg < threshold)
                avg = 0;

            Vector3 minScale = new Vector3(.1f, .1f, .1f);
            Vector3 maxScale = new Vector3(1f, 1f, 1f);

            voiceObject.transform.localScale = Vector3.Lerp(minScale, maxScale, avg);

            LipSyncWeight = Mathf.Lerp(0, 100, avg);
*/

            if (Keyboard.current.jKey.wasPressedThisFrame)
                SelectButton.GetComponent<PlayerModelButton>().Press();

            if (Keyboard.current.hKey.wasPressedThisFrame)
                LeftButton.GetComponent<PlayerModelButton>().Press();

            if (Keyboard.current.kKey.wasPressedThisFrame)
                RightButton.GetComponent<PlayerModelButton>().Press();

            PlayerModelController.rotationY -= 0.5f;
            //Debug.Log(IsGorilla);

            /*if (GorillaParent.instance.vrrigs.Count >= 1)
            {
                if (Time.time > voiceDelay)
                {

                    voiceDelay += 1;
                    Debug.Log("new time " + voiceDelay);
                    restartMic();


                }

            }
            else
            {
                if (!LipSyncFlag)
                {

                    restartMic();
                    LipSyncFlag = true;
                    Debug.Log("Mic Restart");
                }
            }*/
            if (!IsGorilla)
            {
                playermodel = GameObject.Find("playermodel.body");
                if (playermodel == null)
                {
                    PlayerModelController.LoadModel(playerIndex);
                    PlayerModelController.AssignModel();
                }
            }
            
            
                

            if (PhotonNetwork.InRoom)
                {
                
                if (IsGorilla == true)//in a room, is gorilla model
                {
                    
                    PlayerModelAppearance.ShowOfflineRig();
                    PlayerModelAppearance.ShowOnlineRig();
                    flag_inroom = true;
                    PlayerModelAppearance.flag1 = true;
                    
                    clone_body = null;
                }
                else//in a room, is playermodel
                {
                    if (playermodel != null)
                    {
                        
                        PlayerModelAppearance.HideOfflineRig();
                        PlayerModelAppearance.HideOnlineRig();

                        if (PlayerModelController.CustomColors)
                            PlayerModelAppearance.AssignColor();

                        if (PlayerModelController.GameModeTextures)
                            
                            PlayerModelAppearance.AssignMaterial(playermodel);

                        if (PlayerModelController.LipSync)
                        {
                            playermodel.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, LipSyncWeight);
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
                    
                }
                else//not in a room, is playermodel
                {
                    playermodel = GameObject.Find("playermodel.body");
                    if (playermodel != null)
                    {
                        
                        PlayerModelAppearance.ResetMaterial(playermodel);
                        PlayerModelAppearance.HideOfflineRig();
                        

                        
                        if (PlayerModelController.CustomColors)
                            PlayerModelAppearance.AssignColor();

                        PlayerModelAppearance.AssignMaterial(playermodel);

                        if (PlayerModelController.LipSync)
                        {
                            playermodel.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, LipSyncWeight);
                        }

                    }
                    
                }

            }

        }//fixedupdate
    }
}
