using UnityEngine;
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
            //gorillaface.GetComponent<Renderer>().forceRenderingOff = true; // turns model off
            gorillaface.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 0.0f));
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
            //gorillaface.GetComponent<Renderer>().forceRenderingOff = false; // turns model on
            gorillaface.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1.0f));
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


        static public void AssignColor()
        {
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;
            gorillabody = GameObject.Find("Global/Local VRRig/Local Gorilla Player/gorilla");

            if (colormat_index != mat_index || playermats[mat_index] == Plugin.player_main_material)
            {
                if(playermats[colormat_index] != null)
                {
                    playermats[colormat_index].color = gorillabody.GetComponent<Renderer>().material.color;
                }
                
            }

        }

        static public void ResetMaterial(GameObject playermodel)
        {
            
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;

            if (playermats != null)
            {
                if (playermats[mat_index] != Plugin.player_main_material)
                {
                    playermats[mat_index] = Plugin.player_main_material;
                }
            }
        }

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

        public static int mat_index;
        public static int colormat_index;
        public static Material premat;
        public static Material[] playermats;//used to store reference of selected playermodel to assign back
        static public void AssignMaterial(GameObject clone_body, GameObject playermodel)
        {
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;

            if (clone_body != null && playermodel != null)
            {
                if(clone_body.GetComponent<Renderer>().material.name == "darkfur 1(Clone) (Instance)")
                {
                    if(playermats[mat_index] != Plugin.player_main_material)
                    {
                        playermats[mat_index] = Plugin.player_main_material;
                    }
                    
                }
                else
                {
                    if(playermats[mat_index] != clone_body.GetComponent<Renderer>().material)
                    {
                        playermats[mat_index] = clone_body.GetComponent<Renderer>().material;
                    }
                }

                playermodel.GetComponent<SkinnedMeshRenderer>().materials = playermats;
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