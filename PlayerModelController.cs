using BepInEx;
using System;
using UnityEngine;
using Utilla;
using DitzelGames.FastIK;
using System.Reflection;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;


public class PlayerModelController : MonoBehaviour
{
    static public GameObject misc_orb;

    static public string playermodel_head = "playermodel.head";
    static public string playermodel_torso = "playermodel.torso";

    static public string playermodel_lefthand = "playermodel.lefthand";
    static public string playermodel_righthand = "playermodel.righthand";
    static public GameObject playermodel_preview;
    static public string namegui;
    static public void PreviewModel(int index)
    {
        
        Text mytext = misc_orb.AddComponent<Text>();
        mytext.text = PlayerModel.Plugin.fileName[index];
    }
    

    
    static public void UnloadModel(int index)
    {
        GameObject playermodel = GameObject.Find(PlayerModel.Plugin.fileName[index] + "(Clone)");
        if(playermodel != null)
        {
            GameObject headbone = GameObject.Find(playermodel_head);
            GameObject HandRight = GameObject.Find(playermodel_lefthand);
            GameObject HandLeft = GameObject.Find(playermodel_righthand);
            GameObject offsetL =  GameObject.Find("offsetL");
            GameObject offsetR =  GameObject.Find("offsetR");
            GameObject root = GameObject.Find(playermodel_torso);
            GameObject LeftTarget = GameObject.Find(HandRight.name + " Target");
            GameObject RightTarget = GameObject.Find(HandLeft.name + " Target");

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
        else
        {

            Debug.LogError("Cant unload specified Model:");
            //Debug.LogError(PlayerModel.Plugin.fileName[index]);
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
        if(playerbundle != null)
        {
            GameObject assetplayer = playerbundle.LoadAsset<GameObject>(PlayerModel.Plugin.fileName[index]);
            if(assetplayer != null)
            {
                Instantiate(assetplayer);

                playerbundle.Unload(false);

                Debug.Log("Attempting to load " + PlayerModel.Plugin.fileName[index] + " model");
                
                offsetL = new GameObject("offsetL");
                offsetR = new GameObject("offsetR");
                
                HandLeft = GameObject.Find(playermodel_lefthand);
                
                HandRight = GameObject.Find(playermodel_righthand);
                
                root = GameObject.Find(playermodel_torso);
                
                headbone = GameObject.Find(playermodel_head);
                headtarget = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/head");
                
            }
        }
    }

    static public void AssignModel()
    {
        offsetL.transform.SetParent(GameObject.Find("hand.L").transform, false);
        offsetR.transform.SetParent(GameObject.Find("hand.R").transform, false);
        offsetL.transform.localRotation = Quaternion.Euler(180f, 180f, 90f);
        offsetR.transform.localRotation = Quaternion.Euler(180f, 180f, -90f);
        poleR = new GameObject("poleR");
        poleR.transform.SetParent(root.transform, false);
        poleL = new GameObject("poleL");
        poleL.transform.SetParent(root.transform, false);
        
        poleL.transform.localPosition = new Vector3(-5f, -5f, -10);
        poleR.transform.localPosition = new Vector3(5f, -5f, -10);

        HandLeft.AddComponent<FastIKFabric>();
        HandLeft.GetComponent<FastIKFabric>().Target = offsetL.transform;
        HandLeft.GetComponent<FastIKFabric>().Pole = poleL.transform;

        HandRight.AddComponent<FastIKFabric>();
        HandRight.GetComponent<FastIKFabric>().Target = offsetR.transform;
        HandRight.GetComponent<FastIKFabric>().Pole = poleR.transform;
        root.transform.SetParent(GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body").transform, false);
        root.transform.localRotation = Quaternion.Euler(0f, 0.0f, 0.0f);

        headbone.transform.localRotation = headtarget.transform.localRotation;
        headoffset = headtarget.transform.localRotation;
        headbone.transform.SetParent(headtarget.transform, true);
        headbone.transform.localRotation = Quaternion.Euler(headoffset.x - 8, headoffset.y, headoffset.z);

    }
}
