using UnityEngine;

namespace PlayerModel.Utils
{
    public class PlayerModelButton : MonoBehaviour
    {
        public int button;

        public float debounceTime = 0.25f;

        public float touchTime;

        private bool pressedShow = true;

        public bool setColour = true;
        public Vector3 oldpos;
        public Vector3 pos;

        void Start()
        {
            gameObject.layer = 18;
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
            oldpos = transform.localScale;
            pos = transform.localScale;
        }

        public void Press()
        {
            if (!enabled || !(touchTime + debounceTime < Time.time))
            {
                return;
            }

            touchTime = Time.time;

            Plugin.ButtonPress(button);
        }

        void Update()
        {
            transform.localScale = pos;
            if (!enabled || !(touchTime + debounceTime < Time.time))
            {
                if (setColour)
                    gameObject.GetComponent<Renderer>().material.color = Color.red;

                pos = oldpos * 1.075f;
                pressedShow = false;
            }
            else
            {
                if (setColour)
                    gameObject.GetComponent<Renderer>().material.color = Color.white;

                pos = oldpos;
                pressedShow = true;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (pressedShow)
            {
                if (!(collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null))
                    return;

                GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
                if (component != null)
                {
                    GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, component.isLeftHand, 0.05f);
                    GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                }
            }
            Press();
        }
    }
}
