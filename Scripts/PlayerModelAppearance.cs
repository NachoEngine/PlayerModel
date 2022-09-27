using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace PlayerModel.Player
{
    public class PlayerModelAppearance : MonoBehaviour
    {
        public static List<GameObject> playerGameObjects = new List<GameObject>();
        public static GameObject serverGameObject;

        static public void ShowOnlineRig()
        {
            GameObject clone_face = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = false;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = false;
        }

        static public void HideOnlineRig()
        {
            GameObject clone_face = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = true;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = true;
        }

        static public void HideOfflineRig()
        {
            GameObject gorillaface = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/head/gorillaface").gameObject;
            gorillaface.GetComponent<Renderer>().forceRenderingOff = true; // turns model off

            GameObject gorillabody = GorillaTagger.Instance.offlineVRRig.mainSkin.gameObject;
            gorillabody.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;

            /*GameObject gorillachest = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/gorillachest").gameObject;
            gorillachest.GetComponent<Renderer>().material = mat_;
            System.Console.WriteLine("Chest material set to: " + gorillachest.GetComponent<Renderer>().material.name);
*/
        }

        static public void ShowOfflineRig()
        {
            GameObject gorillaface = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/head/gorillaface").gameObject;
            gorillaface.GetComponent<Renderer>().forceRenderingOff = false; // turns model on

            GameObject gorillabody = GorillaTagger.Instance.offlineVRRig.mainSkin.gameObject;
            gorillabody.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;

            /*GameObject gorillachest = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/gorillachest").gameObject;
            gorillachest.GetComponent<Renderer>().material = mat_;
            System.Console.WriteLine("Chest material set to: " + gorillachest.GetComponent<Renderer>().material.name);
*/
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