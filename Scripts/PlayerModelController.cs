using DitzelGames.FastIK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace PlayerModel.Player
{
    public class PlayerModelController : MonoBehaviour
    {
        static public GameObject misc_orb;

        static public string playermodel_head = "playermodel.head";
        static public string playermodel_torso = "playermodel.torso";

        static public string playermodel_lefthand = "playermodel.lefthand";
        static public string playermodel_righthand = "playermodel.righthand";


        static public string player_info_stream;

        static public string[] player_info;

        static public string playermodel_name;
        static public string playermodel_author;

        static public bool CustomColors;
        static public string colorMat;

        static public bool GameModeTextures;
        static public string gameMat;

        static public GameObject player_preview;
        static public string namegui;

        public static float rotationY = -60f;
        public static float localPositionY = 0.15f;

        public static int gamemat_index;
        public static int colormat_index;

        public static bool LipSync = false;

        static public void PreviewModel(int index)
        {
            if (player_preview != null)
                Destroy(player_preview);

            string path = Path.Combine(Plugin.playerpath, Plugin.fileName[index]);
            //Debug.Log(path);

            AssetBundle playerbundle = AssetBundle.LoadFromFile(path);
            GameObject assetplayer = playerbundle.LoadAsset<GameObject>("playermodel.ParentObject");
            if (playerbundle != null && assetplayer != null)
            {
                var parentAsset = Instantiate(assetplayer);

                playerbundle.Unload(false);

                player_info_stream = parentAsset.GetComponent<Text>().text;
                //Debug.Log(player_info_stream);
                player_info = player_info_stream.Split('$');

                parentAsset.GetComponent<Text>().enabled = false;

                playermodel_name = player_info[0];
                playermodel_author = player_info[1];

                player_body = parentAsset.transform.GetChild(0).gameObject.transform.Find("playermodel.body").gameObject;
                List<Material> material_list = player_body.GetComponent<SkinnedMeshRenderer>().materials.ToList();

                Material[] material_array = material_list.ToArray();

                player_preview = new GameObject("playemodel.preview");
                var meshFilter = player_preview.AddComponent<MeshFilter>();
                Mesh originalMesh = player_body.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                meshFilter.mesh = originalMesh;
                MeshRenderer rend = player_preview.AddComponent<MeshRenderer>();//easy code really
                rend.materials = material_array;
                player_preview.transform.localScale = player_body.transform.localScale;
                pos = Plugin.misc_preview.transform.position;
                player_preview.transform.position = PlayerModel.Plugin.misc_preview.transform.position;

                Quaternion rot = Quaternion.Euler(-90f, -60f, 0f);
                player_preview.transform.rotation = rot;

                Plugin.model_text.text = playermodel_name.ToUpper(); ;
                Plugin.author_text.text = playermodel_author.ToUpper();

                //new playermodel info here:
                if (player_info.Length > 4)
                {
                    Plugin.mat_preview[0] = null;
                    Plugin.SetMainDisplayButtonMaterial(Plugin.mat_preview[0]);
                    bool checkmat = true;
                    bool gamemodetex = bool.Parse(player_info[3]); //if gamemodetextures are changable
                    string gameMatname = player_info[5];
                    //Debug.Log("gamemode texture material: "+ gameMatname);

                    if (gamemodetex)//player_info[5] is the gameMat assigned material name
                    {
                        //Debug.Log("Mat Search start");
                        for (int i = 0; i < material_array.Length; i++)//cycles mat list to match material name to assign gameMat changing
                        {
                            //Debug.Log(material_array[i].name);
                            if (gameMatname == material_array[i].name)
                            {
                                Plugin.mat_preview[0] = material_array[i];
                                Plugin.SetMainDisplayButtonMaterial(material_array[i]);
                                checkmat = false;
                                break;
                            }
                            //possible bug here: if the gameMatname isnt found in the preview material list, it would be assigned to mat_preview[0] etc
                            //will make sure to have the text_info have the material name in the project, will add error logs in project

                        }
                        if (checkmat)
                        {
                            Debug.LogError("Material Reference not Found");
                        }
                    }

                    if (player_info.Length == 7)
                    {
                        if (bool.Parse(player_info[6]))
                        {
                            //Debug.Log("This Playermodel has lipsync on");
                        }
                    }

                    else
                    {
                        Plugin.mat_preview[0] = player_preview.GetComponent<MeshRenderer>().material;
                        Plugin.SetMainDisplayButtonMaterial(player_preview.GetComponent<MeshRenderer>().material);

                    }
                }
                else
                {
                    Plugin.mat_preview[0] = player_preview.GetComponent<MeshRenderer>().material;
                    Plugin.SetMainDisplayButtonMaterial(player_preview.GetComponent<MeshRenderer>().material);

                }




                player_preview.AddComponent<SpinYouBaboon>();

                Destroy(parentAsset);
            }
        }

        public static Vector3 pos;
        static public GameObject player_body;

        static public void UnloadModel()
        {

            GameObject.Find("RightHandTriggerCollider").GetComponent<SphereCollider>().enabled = true;
            GameObject.Find("LeftHandTriggerCollider").GetComponent<SphereCollider>().enabled = true;

            GameObject playermodel = GameObject.Find("playermodel.ParentObject(Clone)");
            if (playermodel != null)
            {
                //Debug.Log("Unloading Playermodel");
                GameObject headbone = GameObject.Find(playermodel_head);
                GameObject HandRight = GameObject.Find(playermodel_lefthand);
                GameObject HandLeft = GameObject.Find(playermodel_righthand);
                GameObject offsetL = GameObject.Find("offsetL");
                GameObject offsetR = GameObject.Find("offsetR");
                GameObject root = GameObject.Find(playermodel_torso);
                GameObject LeftTarget = GameObject.Find("playermodel.lefthandpos" + " Target");
                GameObject RightTarget = GameObject.Find("playermodel.righthandpos" + " Target");
                GameObject poleR = GameObject.Find("poleR");
                GameObject poleL = GameObject.Find("poleL");

                digit_L.Clear();
                digit_R.Clear();

                Destroy(poleR);
                Destroy(poleL);
                Destroy(LeftTarget);
                Destroy(RightTarget);
                Destroy(root);
                Destroy(HandLeft);
                Destroy(offsetL);
                Destroy(offsetR);
                Destroy(HandRight);
                Destroy(headbone);
                Destroy(playermodel);
            }
        }

        public static GameObject offsetL;
        public static GameObject offsetR;
        public static GameObject HandLeft;
        public static GameObject HandRight;
        public static GameObject root;
        public static GameObject headbone;
        public static GameObject headtarget;
        public static GameObject poleR;
        public static GameObject poleL;

        public static List<GameObject> digit_R = new List<GameObject>();
        public static List<GameObject> digit_L = new List<GameObject>();


        public static Quaternion headoffset;
        static public void LoadModel(int index)
        {
            //Debug.Log("Loading Playermodel: "+index);
            LipSync = false;

            AssetBundle playerbundle = AssetBundle.LoadFromFile(Path.Combine(PlayerModel.Plugin.playerpath, PlayerModel.Plugin.fileName[index]));
            if (playerbundle != null)
            {
                GameObject assetplayer = playerbundle.LoadAsset<GameObject>("playermodel.ParentObject");
                if (assetplayer != null)
                {
                    var parentAsset = Instantiate(assetplayer);

                    playerbundle.Unload(false);

                    player_info_stream = parentAsset.GetComponent<Text>().text;
                    //Debug.Log(player_info_stream);
                    player_info = player_info_stream.Split('$');

                    parentAsset.GetComponent<Text>().enabled = false;
                    //Debug.Log(player_info[0]);
                    CustomColors = bool.Parse(player_info[2]);

                    GameModeTextures = bool.Parse(player_info[3]);

                    offsetL = new GameObject("offsetL");
                    offsetR = new GameObject("offsetR");

                    HandLeft = GameObject.Find(playermodel_lefthand);

                    HandRight = GameObject.Find(playermodel_righthand);

                    root = GameObject.Find(playermodel_torso);

                    headbone = GameObject.Find(playermodel_head);
                    headtarget = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/head").gameObject;


                    PlayerModelAppearance.playermats = GameObject.Find("playermodel.body").GetComponent<SkinnedMeshRenderer>().materials;


                    if (player_info.Length > 4)//new version of PlayerModel V2
                    {

                        colorMat = player_info[4];
                        gameMat = player_info[5];

                        for (int i = 0; i < PlayerModelAppearance.playermats.Length; i++)
                        {
                            //find Color Material
                            if (colorMat == PlayerModelAppearance.playermats[i].name)
                            {
                                colormat_index = i;
                                //Debug.Log("Assigned Color material to: "+ i +" " + PlayerModelAppearance.playermats[i]);
                            }

                            //find GameTexture material
                            if (gameMat == PlayerModelAppearance.playermats[i].name)
                            {
                                Plugin.player_main_material = PlayerModelAppearance.playermats[i];
                                gamemat_index = i;
                                //Debug.Log("Assigned Main mat: "+ Plugin.player_main_material + " to: " + i + " " + PlayerModelAppearance.playermats[i]);
                            }
                        }

                        if (player_info.Length == 7) //PlayerModel v3
                        {
                            modelVersion = 2;
                            //Debug.Log("playermodel v3 lipsync");
                            if (bool.Parse(player_info[6]))
                            {
                                LipSync = true;
                            }
                        }
                    }
                    else
                    {//PlayerModel V1
                        gamemat_index = 0;
                        colormat_index = 0;
                        Plugin.player_main_material = PlayerModelAppearance.playermats[0]; //saves playermodel material
                    }


                }
            }
        }

        public static int modelVersion;
        static public void AssignModel()
        {
            //Debug.Log("playermodel assigned");
            if (player_info.Length > 4)
            {
                modelVersion = 1;
            }
            else
            {
                modelVersion = 0;
            }
            //Debug.Log("Model Version: " + modelVersion);

            GameObject hand_l = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").gameObject;
            //Debug.Log(hand_l);
            GameObject hand_r = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").gameObject;
           // Debug.Log(hand_r);
            GameObject bodyrig = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body").gameObject;
            //Debug.Log(bodyrig);
            //Debug.Log("CHECK1");
            offsetL.transform.SetParent(hand_l.transform, false);

            offsetR.transform.SetParent(hand_r.transform, false);

            poleR = new GameObject("poleR");
            poleR.transform.SetParent(root.transform, false);
            poleL = new GameObject("poleL");
            poleL.transform.SetParent(root.transform, false);

            poleL.transform.localPosition = new Vector3(-5f, -5f, -10);
            poleR.transform.localPosition = new Vector3(5f, -5f, -10);

            GameObject lefthandpos = new GameObject("playermodel.lefthandpos");
            GameObject righthandpos = new GameObject("playermodel.righthandpos");

            GameObject lefthandparent = HandLeft.transform.parent.gameObject;
            GameObject righthandparent = HandRight.transform.parent.gameObject;

            lefthandpos.transform.SetParent(lefthandparent.transform, false);
            righthandpos.transform.SetParent(righthandparent.transform, false);

            lefthandpos.transform.SetPositionAndRotation(HandLeft.transform.position, HandLeft.transform.rotation);
            righthandpos.transform.SetPositionAndRotation(HandRight.transform.position, HandRight.transform.rotation);

            HandLeft.transform.SetPositionAndRotation(hand_l.transform.position, hand_l.transform.rotation);
            HandRight.transform.SetPositionAndRotation(hand_r.transform.position, hand_r.transform.rotation);

            Quaternion rotL = Quaternion.Euler(HandLeft.transform.localRotation.eulerAngles.x, HandLeft.transform.localRotation.eulerAngles.y, HandLeft.transform.localRotation.eulerAngles.z + 20f);
            Quaternion rotR = Quaternion.Euler(HandRight.transform.localRotation.eulerAngles.x, HandRight.transform.localRotation.eulerAngles.y, HandRight.transform.localRotation.eulerAngles.z - 20f);

            HandLeft.transform.position = hand_l.transform.position;
            HandRight.transform.position = hand_r.transform.position;

            HandLeft.transform.localRotation = rotL;
            HandRight.transform.localRotation = rotR;

            HandLeft.transform.SetParent(hand_l.transform, true);
            HandRight.transform.SetParent(hand_r.transform, true);

            //get each digit on each hand (parent bone of each digit)

            if (modelVersion > 0)
            {
                assignDigit(HandLeft, digit_L);
                assignDigit(HandRight, digit_R);
                root.AddComponent<fingermovement>();
            }

            HandLeft = lefthandpos;
            HandRight = righthandpos;

            HandLeft.AddComponent<FastIKFabric>();
            HandLeft.GetComponent<FastIKFabric>().Target = offsetL.transform;
            HandLeft.GetComponent<FastIKFabric>().Pole = poleL.transform;

            HandRight.AddComponent<FastIKFabric>();
            HandRight.GetComponent<FastIKFabric>().Target = offsetR.transform;
            HandRight.GetComponent<FastIKFabric>().Pole = poleR.transform;
            root.transform.SetParent(bodyrig.transform, false);
            root.transform.localRotation = Quaternion.Euler(0f, 0.0f, 0.0f);

            headbone.transform.localRotation = headtarget.transform.localRotation;
            headoffset = headtarget.transform.localRotation;
            headbone.transform.SetParent(headtarget.transform, true);
            headbone.transform.localRotation = Quaternion.Euler(headoffset.x - 8, headoffset.y, headoffset.z);
            headbone.transform.localPosition = new Vector3(0, 0, 0);
            if (player_info.Length == 7)
            {
                //Debug.Log("scaling for v3 model");
                Vector3 scale = new Vector3(100, 100, 100);
                root.transform.localScale = scale;
                headbone.transform.localScale = scale;
            }

        }
        static public void assignDigit(GameObject hand, List<GameObject> digits)
        {
            List<GameObject> local = new List<GameObject>();

            foreach (Transform fingies in hand.transform)
            {
                local.Add(fingies.gameObject);
            }
            int index = 0;
            int mid = 0;
            int thumb = 0;
            //reorder list to index, middle, thumb
            for (int i = 0; i < hand.transform.childCount; i++)
            {
                if (hand.transform.GetChild(i).name.Contains("index"))
                {
                    index = i;
                }
                if (hand.transform.GetChild(i).name.Contains("middle"))
                {
                    mid = i;
                }
                if (hand.transform.GetChild(i).name.Contains("thumb"))
                {
                    thumb = i;
                }
            }

            digits.Add(hand.transform.GetChild(index).gameObject);
            digits.Add(hand.transform.GetChild(mid).gameObject);
            digits.Add(hand.transform.GetChild(thumb).gameObject);
        }
        float currentTime;


        public class SpinYouBaboon : MonoBehaviour
        {
            float to;
            void Update()
            {
                to = Mathf.Lerp(to, localPositionY, 0.35f * 0.1f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(-90f, rotationY, 0);
                transform.position = pos + new Vector3(0, to, 0);
            }
        }

        public class fingermovement : MonoBehaviour
        {
            float remapvalue = -75.0f; //degrees
            public float rightGrip;
            public float rightTrigger;
            public bool rightSecondary;

            public float leftGrip;
            public float leftTrigger;
            public bool leftSecondary;

            public readonly XRNode lNode = XRNode.LeftHand;
            public readonly XRNode rNode = XRNode.RightHand;

            List<GameObject> objs = new List<GameObject>();

            bool ready = false;


            void Start()
            {
                //Debug.Log("add smoothing script");
                for (int i = 0; i < digit_L.Count; i++)
                {
                    digit_L[i].AddComponent<smoothing>();
                    digit_R[i].AddComponent<smoothing>();
                    objs.Add(digit_L[i]);

                }
                for (int i = 0; i < digit_L.Count; i++)
                {
                    objs.Add(digit_R[i]);

                }

                for (int i = 0; i < objs.Count; i++)
                {
                    ResetTransforms(objs[i]);
                    ResetTransforms(objs[i].transform.GetChild(0).gameObject);
                    ResetTransforms(objs[i].transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                }

                ready = true;
            }
            void Update()
            {
                if (ready)
                {
                    InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftGrip);
                    InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftTrigger);
                    InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftSecondary);

                    InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out rightGrip);
                    InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out rightTrigger);
                    InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out rightSecondary);

                    digit_L[0].GetComponent<smoothing>().input = leftTrigger;
                    digit_L[1].GetComponent<smoothing>().input = leftGrip;
                    digit_L[2].GetComponent<smoothing>().input = Convert.ToSingle(leftSecondary);

                    digit_R[0].GetComponent<smoothing>().input = rightTrigger;
                    digit_R[1].GetComponent<smoothing>().input = rightGrip;
                    digit_R[2].GetComponent<smoothing>().input = Convert.ToSingle(rightSecondary);


                    //Debug.Log(Convert.ToSingle(leftSecondary));
                    fingermove(digit_L[0], digit_L[0].GetComponent<smoothing>().avg);
                    fingermove(digit_L[1], digit_L[1].GetComponent<smoothing>().avg);
                    fingermove(digit_L[2], digit_L[2].GetComponent<smoothing>().avg);

                    fingermove(digit_R[0], digit_R[0].GetComponent<smoothing>().avg);
                    fingermove(digit_R[1], digit_R[1].GetComponent<smoothing>().avg);
                    fingermove(digit_R[2], digit_R[2].GetComponent<smoothing>().avg);
                }

            }

            void fingermove(GameObject parent, float input)//parent digit bone, float value from vr controller input (0.0->1.0)
            {
                float angle = Remap(input, remapvalue);//converts normalize value to relative angle to bone
                float angle2 = Remap(input, remapvalue);
                Vector3 localAngle = parent.transform.localEulerAngles;

                parent.transform.localEulerAngles = new Vector3(angle, localAngle.y, localAngle.z);//parent bone

                Vector3 localangle1 = parent.transform.GetChild(0).GetChild(0).localEulerAngles;

                parent.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(angle2, localangle1.y, localangle1.z);//middle bone

                Vector3 localangle2 = parent.transform.GetChild(0).GetChild(0).GetChild(0).localEulerAngles;

                parent.transform.GetChild(0).GetChild(0).GetChild(0).localEulerAngles = new Vector3(angle2, localangle2.y, localangle2.z);//end bone

            }

            float Remap(float source, float targetTo)
            {

                float sourceTo = 1;
                float sourceFrom = 0;
                float targetFrom = 0;
                return targetFrom + (source - sourceFrom) * (targetTo - targetFrom) / (sourceTo - sourceFrom);
            }
            void ResetTransforms(GameObject obj)
            {
                GameObject newParent = new GameObject();
                newParent.name = "newParent_" + obj.name;
                newParent.transform.SetParent(obj.transform.parent, false);
                newParent.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);
                newParent.transform.localScale = obj.transform.localScale;
                obj.transform.SetParent(newParent.transform, true);
            }

        }

        public class smoothing : MonoBehaviour
        {

            const int samples = 3;
            float[] readings = new float[samples];
            int index = 0;
            float total = 0;
            public float avg = 0;

            public float input;

            void Start()
            {
                for (int i = 0; i < readings.Length; i++)
                {
                    readings[i] = 0;
                }
            }

            void Update()
            {
                total -= readings[index];
                readings[index] = input;
                total += readings[index];
                index++;

                if (index >= samples)
                {
                    index = 0;
                }

                avg = total / samples;
            }
        }

    }

}