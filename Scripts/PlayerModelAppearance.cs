using System.Collections.Generic;
using UnityEngine;

namespace PlayerModel.Player
{
    public class PlayerModelAppearance : MonoBehaviour
    {
        public static GameObject gorillabody;
        public static GameObject gorillaface;
        public static GameObject gorillachest;

        public static GameObject serverGameObject;

        static public void ShowOnlineRig()
        {
            GameObject clone_face = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = false;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = false;
        }

        static public void HideOnlineRig()
        {
            GameObject clone_face = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/head/gorillaface");
            GameObject clone_body = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
            GameObject clone_chest = GameObject.Find("Player Objects/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/gorillachest");

            clone_face.GetComponent<MeshRenderer>().forceRenderingOff = true;
            clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;
            clone_chest.GetComponent<MeshRenderer>().forceRenderingOff = true;
        }
        static public void HideOfflineRig()
        {
            //Debug.Log("hidding gorilla");
            int LayerCameraIgnore = LayerMask.NameToLayer("Bake");//hidden layer
            
            gorillaface.layer = LayerCameraIgnore;
            gorillabody.layer = LayerCameraIgnore;
            gorillachest.layer = LayerCameraIgnore;

        }

        static public void ShowOfflineRig()
        {
            //Debug.Log("showing gorilla");
            int LayerCameraShow = LayerMask.NameToLayer("Default");

            
            gorillaface.layer = LayerCameraShow;
            
            gorillabody.layer = LayerCameraShow;
            
            gorillachest.layer = LayerCameraShow;

        }


        static Color gorillacolor;
        static public bool flag1 = true;
        static public Renderer rendGorilla;
        static public Material mat;

        public static Color GetColor()
        {
            float r = PlayerPrefs.GetFloat("redValue");
            float g = PlayerPrefs.GetFloat("greenValue");
            float b = PlayerPrefs.GetFloat("blueValue");
            return new Color(r, g, b);


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
                    //Debug.Log("reset mat");
                    playermodel.GetComponent<SkinnedMeshRenderer>().materials = playermats;
                    //Debug.Log(playermats);
                }
            }
        }



        public static int mat_index;
        public static int colormat_index;
        public static Material premat;
        public static Material[] playermats;//used to store reference of selected playermodel to assign back


        static public void AssignMaterial(GameObject playermodel)
        {
            mat_index = PlayerModelController.gamemat_index;
            colormat_index = PlayerModelController.colormat_index;
            
            if (gorillabody != null) //if in a lobby
            {
                if (gorillabody.GetComponent<Renderer>().material.name == "darkfur 1(Clone) (Instance)")//if not main mat
                {
                    if (playermats[mat_index] != Plugin.player_main_material)
                    {
                        playermats[mat_index] = Plugin.player_main_material;
                        //Debug.Log("Playermodel main mat assigned");
                    }

                }
                else
                {
                    playermats[mat_index] = gorillabody.GetComponent<Renderer>().material; //lava ice rockstones
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