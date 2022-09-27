using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DitzelGames.FastIK;

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
        static public bool GameModeTextures;

        static public GameObject player_preview;
        static public string namegui;

        public static float rotationY = -60f;
        public static float localPositionY = 0.15f;

        static public void PreviewModel(int index)
        {
            if (player_preview != null)
                Destroy(player_preview);

            string path = Path.Combine(Plugin.playerpath, Plugin.fileName[index]);
            Debug.Log(path);

            AssetBundle playerbundle = AssetBundle.LoadFromFile(path);
            GameObject assetplayer = playerbundle.LoadAsset<GameObject>("playermodel.ParentObject");
            if (playerbundle != null && assetplayer != null)
            {
                var parentAsset = Instantiate(assetplayer);

                playerbundle.Unload(false);

                player_info_stream = parentAsset.GetComponent<Text>().text;
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

                Plugin.mat_preview[0] = player_preview.GetComponent<MeshRenderer>().material;
                Plugin.SetMainDisplayButtonMaterial(player_preview.GetComponent<MeshRenderer>().material);

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

        public static Quaternion headoffset;
        static public void LoadModel(int index)
        {


            AssetBundle playerbundle = AssetBundle.LoadFromFile(Path.Combine(PlayerModel.Plugin.playerpath, PlayerModel.Plugin.fileName[index]));
            if (playerbundle != null)
            {
                GameObject assetplayer = playerbundle.LoadAsset<GameObject>("playermodel.ParentObject");
                if (assetplayer != null)
                {
                    var parentAsset = Instantiate(assetplayer);

                    playerbundle.Unload(false);

                    player_info_stream = parentAsset.GetComponent<Text>().text;
                    player_info = player_info_stream.Split('$');

                    parentAsset.GetComponent<Text>().enabled = false;

                    CustomColors = bool.Parse(player_info[2]);

                    GameModeTextures = bool.Parse(player_info[3]);

                    offsetL = new GameObject("offsetL");
                    offsetR = new GameObject("offsetR");

                    HandLeft = GameObject.Find(playermodel_lefthand);

                    HandRight = GameObject.Find(playermodel_righthand);

                    root = GameObject.Find(playermodel_torso);

                    headbone = GameObject.Find(playermodel_head);
                    headtarget = GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body/head");

                    PlayerModel.Plugin.player_main_material = GameObject.Find("playermodel.body").GetComponent<SkinnedMeshRenderer>().material; //saves playermodel material
                }
            }
        }

        static public void AssignModel()
        {
            /*GameObject left_finger = GameObject.Find("playermodel.left_finger");
            GameObject right_finger = GameObject.Find("playermodel.right_finger");

            if (left_finger != null && right_finger != null)
            {
                left_finger.AddComponent<SphereCollider>().radius = 0.01f;
                right_finger.AddComponent<SphereCollider>().radius = 0.01f;
                left_finger.layer = 10;
                right_finger.layer = 10;

                GameObject.Find("RightHandTriggerCollider").GetComponent<SphereCollider>().enabled = false;
                GameObject.Find("LeftHandTriggerCollider").GetComponent<SphereCollider>().enabled = false;
            }*/

            GameObject hand_l = GameObject.Find("hand.L");
            GameObject hand_r = GameObject.Find("hand.R");

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

            HandLeft = lefthandpos;
            HandRight = righthandpos;


            HandLeft.AddComponent<FastIKFabric>();
            HandLeft.GetComponent<FastIKFabric>().Target = offsetL.transform;
            HandLeft.GetComponent<FastIKFabric>().Pole = poleL.transform;

            HandRight.AddComponent<FastIKFabric>();
            HandRight.GetComponent<FastIKFabric>().Target = offsetR.transform;
            HandRight.GetComponent<FastIKFabric>().Pole = poleR.transform;
            root.transform.SetParent(GameObject.Find("Global/Local VRRig/Local Gorilla Player/rig/body").transform, false);
            root.transform.localRotation = Quaternion.Euler(0f, 0.0f, 0.0f);

            headbone.transform.localRotation = headtarget.transform.localRotation;
            headoffset = headtarget.transform.localRotation;
            headbone.transform.SetParent(headtarget.transform, true);
            headbone.transform.localRotation = Quaternion.Euler(headoffset.x - 8, headoffset.y, headoffset.z);

        }

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
    }

}