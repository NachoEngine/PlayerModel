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
            Debug.Log("hidding gorilla");
            int LayerCameraIgnore = LayerMask.NameToLayer("Bake");//hidden layer

            GameObject gorillaface = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/head/gorillaface").gameObject;
            gorillaface.layer = LayerCameraIgnore;
            GameObject gorillabody = GorillaTagger.Instance.offlineVRRig.mainSkin.gameObject;
            gorillabody.layer = LayerCameraIgnore;
            GameObject gorillachest = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/gorillachest").gameObject;
            gorillachest.layer = LayerCameraIgnore;


        }

        static public void ShowOfflineRig()
        {
            int LayerCameraIgnore = LayerMask.NameToLayer("Default");

            GameObject gorillaface = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/head/gorillaface").gameObject;
            gorillaface.layer = LayerCameraIgnore;
            GameObject gorillabody = GorillaTagger.Instance.offlineVRRig.mainSkin.gameObject;
            gorillabody.layer = LayerCameraIgnore;
            GameObject gorillachest = GorillaTagger.Instance.offlineVRRig.mainSkin.transform.parent.Find("rig/body/gorillachest").gameObject;
            gorillachest.layer = LayerCameraIgnore;

        }


        static Color gorillacolor;
        
        static public bool flag1 = true;
        
        static public Renderer rendGorilla;

        
        static public Material mat;
        public static GameObject gorillabody;
        public static Color GetColor()
        {
            float r = PlayerPrefs.GetFloat("redValue");
            float g = PlayerPrefs.GetFloat("greenValue");
            float b = PlayerPrefs.GetFloat("blueValue");
            return new Color(r,g,b);

            
        }
        static public void AssignColor()
        {
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;

            //Debug.Log("color assigned");
            if (colormat_index != mat_index || playermats[mat_index] == Plugin.player_main_material)
            //if color mat is not the same as game textures mat
            //or
            //if playermodel game textures mat is the main mat
            {
                if (playermats[colormat_index] != null)
                {
                    playermats[colormat_index].color = GetColor();
                    Plugin.playermodel.GetComponent<SkinnedMeshRenderer>().materials = playermats;

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
                    Debug.Log("reset mat");
                    playermodel.GetComponent<SkinnedMeshRenderer>().materials = playermats;
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
        

        static public void AssignMaterial(GameObject playermodel)
        {
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;
            GameObject clone_body = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            if(clone_body != null) //if in a lobby
            {
                if (clone_body.GetComponent<Renderer>().material.name == "darkfur 1(Clone) (Instance)")//if not main mat
                {
                    if (playermats[mat_index] != Plugin.player_main_material)
                    {
                        playermats[mat_index] = Plugin.player_main_material;
                        Debug.Log("Playermodel main mat assigned");
                    }

                }
                else
                {
                    playermats[mat_index] = clone_body.GetComponent<Renderer>().material; //lava ice rockstones
                }

                
            }
            else
            {
                playermats[mat_index] = Plugin.player_main_material;
            }
            playermodel.GetComponent<SkinnedMeshRenderer>().materials = playermats;
            

            
            
           
        }

        
    }
}