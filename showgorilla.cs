using BepInEx;
using System;
using UnityEngine;
using Utilla;
using DitzelGames.FastIK;
using System.Reflection;
using System.IO;
using Photon.Pun;

namespace showgorilla
{
    public class Showgorilla : MonoBehaviour
    {

        static public void ShowOnlineRig()
        {
            GameObject clone_face = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = false;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = false;
        }

        static public void HideOnlineRig()
        {
            GameObject clone_face = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = true;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = true;
        }

        static public void HideOfflineRig()
        {
            GameObject gorillaface = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/head/gorillaface");
            gorillaface.GetComponent<Renderer>().forceRenderingOff = true; // turns model off

            GameObject gorillabody = GameObject.Find("OfflineVRRig/Actual Gorilla/gorilla");
            gorillabody.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;


            GameObject gorillachest = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/gorillachest");
            gorillachest.GetComponent<Renderer>().forceRenderingOff = true;
 
        }

        static public void ShowOfflineRig()
        {
            GameObject gorillaface = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/head/gorillaface");
            gorillaface.GetComponent<Renderer>().forceRenderingOff = false; // turns model on

            GameObject gorillabody = GameObject.Find("OfflineVRRig/Actual Gorilla/gorilla");
            gorillabody.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;


            GameObject gorillachest = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/gorillachest");
            gorillachest.GetComponent<Renderer>().forceRenderingOff = false;

        }
        static Vector4 gorillacolor;
        static Vector4 last_gorillacolor;
        static public bool flag1 = true;
        static public GameObject gorillabody;
        static public Renderer rendGorilla;

        
        static public Material mat;
        static public void AssignColor(GameObject playermodel)//IsGorilla is false
        {
            gorillabody = GameObject.Find("OfflineVRRig/Actual Gorilla/gorilla");
            //Debug.Log(gorillabody.name);
            rendGorilla = gorillabody.GetComponent<Renderer>();
            gorillacolor = rendGorilla.material.color;

            
            
            
            
            playermodel.GetComponent<SkinnedMeshRenderer>().material.SetVector("_Color", gorillacolor);
            last_gorillacolor = gorillacolor;
            
            
            
            

            
            if (last_gorillacolor != gorillacolor)
            {
                last_gorillacolor = gorillacolor;
                
                last_gorillacolor = gorillacolor;
                playermodel.GetComponent<SkinnedMeshRenderer>().material.SetVector("_Color", gorillacolor);
                
            }


        }
        static public void AssignMaterial(GameObject clone_body, GameObject playermodel)
        {
            if(clone_body != null && playermodel != null)
            {
                if (clone_body.GetComponent<Renderer>().material.name == "infected (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_lava_prop", 1);
                }
                else
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_lava_prop", 0);
                }
                // rock stones material assign
                if (clone_body.GetComponent<Renderer>().material.name == "It (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_rock_prop", 1);
                }
                else
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_rock_prop", 0);
                }
                // ice material assign
                if (clone_body.GetComponent<Renderer>().material.name == "ice (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_ice_prop", 1);
                }
                else
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_ice_prop", 0);
                }
            }
            else
            {
                if(clone_body == null)
                {
                    Debug.Log("clone_body is null");
                }
                if(playermodel == null)
                {
                    Debug.Log("playermodel is null");
                }
                
            }
           
        }

        static public void ResetMaterial(GameObject playermodel)
        {
            if(playermodel != null)
            {
                playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_lava_prop", 0);
                playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_rock_prop", 0);
                playermodel.GetComponent<SkinnedMeshRenderer>().material.SetInt("_ice_prop", 0);
            }
            
        }
    }
}