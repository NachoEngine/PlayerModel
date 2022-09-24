using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace PlayerModel.Player
{
    public class PlayerModelAppearance : MonoBehaviour
    {
        public static List<GameObject> playerGameObjects = new List<GameObject>();
        public static GameObject serverGameObject;
        public static void SetRigRenderering(bool setTo, bool online)
        {
            if (online)
            {
                if (!PhotonNetwork.InRoom)
                    return;

                if (serverGameObject == null)
                    return;

                serverGameObject.transform.GetChild(0).Find("gorilla").GetComponent<SkinnedMeshRenderer>().forceRenderingOff = !setTo;
                serverGameObject.transform.GetChild(0).Find("rig/body/gorillachest").GetComponent<Renderer>().forceRenderingOff = !setTo;
                serverGameObject.transform.GetChild(0).Find("rig/body/head/gorillaface").GetComponent<Renderer>().forceRenderingOff = !setTo;
                return;
            }

            if (playerGameObjects.Count == 0)
                return;

            foreach(GameObject playerObject in playerGameObjects)
            {
                if (playerObject.GetComponent<SkinnedMeshRenderer>() != null)
                    playerObject.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = !setTo;

                if (playerObject.GetComponent<Renderer>() != null)
                    playerObject.GetComponent<Renderer>().forceRenderingOff = !setTo;
            }
        }


        static Color gorillacolor;
        
        static public bool flag1 = true;
        static public GameObject gorillabody;
        static public Renderer rendGorilla;

        
        static public Material mat;
        static public void AssignColor(GameObject playermodel)
        {
            gorillabody = GameObject.Find("Global/Local VRRig/Local Gorilla Player/gorilla");
            rendGorilla = gorillabody.GetComponent<Renderer>();
            gorillacolor = rendGorilla.material.color;
            
            playermodel.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", gorillacolor);

        }

        static public void ResetMaterial(GameObject playermodel) => playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.player_main_material;

        static bool IsBattleMat(GameObject tbod)
        {
            if(tbod.GetComponent<Renderer>().material.name == "bluealive (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "bluehit (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "bluestunned (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "orangealive (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "orangehit (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "orangestunned (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "paintsplattersmallblue (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "paintsplattersmallorange (Instance)")
            {
                return true;
            }
            return false;
        }

        static bool IsBattleMat2(GameObject tbod)
        {
            if (tbod.GetComponent<Renderer>().material.name == "paintsplattersmallblue (Instance)")
            {
                return true;
            }
            if (tbod.GetComponent<Renderer>().material.name == "paintsplattersmallorange (Instance)")
            {
                return true;
            }
            return false;
        }

        static public void AssignMaterial(GameObject clone_body, GameObject playermodel)
        {
            if(clone_body != null && playermodel != null)
            {
                playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.player_main_material;

                if (clone_body.GetComponent<Renderer>().material.name == "infected (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.mat_preview[1];
                }
                else if (clone_body.GetComponent<Renderer>().material.name == "It (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.mat_preview[2];
                }
                else if (clone_body.GetComponent<Renderer>().material.name == "ice (Instance)")
                {
                    playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.mat_preview[3];
                }
                else
                {
                    if (IsBattleMat(clone_body) == true)
                    {
                        if (IsBattleMat2(clone_body) == true)
                        {
                            playermodel.GetComponent<SkinnedMeshRenderer>().material = clone_body.GetComponent<Renderer>().material;
                        }
                        else
                        {
                            playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.player_main_material;
                            playermodel.GetComponent<SkinnedMeshRenderer>().material.color = clone_body.GetComponent<Renderer>().material.color;
                        }
                    }
                    else
                    {
                        playermodel.GetComponent<SkinnedMeshRenderer>().material = PlayerModel.Plugin.player_main_material;
                    }
                }

            }
            else
            {
                if(clone_body == null)
                {
                    Debug.LogError("clone_body is null");
                }
                if(playermodel == null)
                {
                    Debug.LogError("playermodel is null");
                }
                
            }
           
        }

        
    }
}