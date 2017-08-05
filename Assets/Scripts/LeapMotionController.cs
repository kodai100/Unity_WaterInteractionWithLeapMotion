using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionController : SingletonMonoBehaviour<LeapMotionController> {

    [SerializeField] private new Camera camera;
    [SerializeField] private GameObject leapProviderObj;
    [SerializeField] private GameObject handObj;

    private LeapServiceProvider leapProvider;
    private static bool isUseleap;
    private static Vector3 handPosition;
    private bool prevDetected;
    private float waitedTime;
    private static bool isScreenSaverMode;

    #region Accessor
    public static bool IsUseLeap {
        get { return isUseleap; }
        set { isUseleap = value; }
    }

    public static Vector3 HandPosition {
        get { return handPosition; }
        set { handPosition = value; }
    }

    public static bool IsScreenSaverMode {
        get { return isScreenSaverMode; }
    }
    #endregion

    void Start() {
        leapProvider = leapProviderObj.GetComponent<LeapServiceProvider>();
        handPosition = new Vector3(100, 100, 0);
        handObj.transform.position = new Vector3(100, 100, 0);
    }

    Hand GetRightHand(Frame frame) {

        Hand rightHand = null;

        foreach (Hand hand in frame.Hands) {
            if (hand.IsRight) {
                rightHand = hand;
                waitedTime = 0;
                prevDetected = true;
                isScreenSaverMode = false;
                break;
            }
        }

        return rightHand;
    }

    void Update() {

        if (isUseleap) {

            // Detects right hand
            Hand rightHand = GetRightHand(leapProvider.CurrentFrame);
            
            if (rightHand == null) {

                // If leap cant detects a right hand previous frame and this frame
                if (!prevDetected) {
                    waitedTime += Time.deltaTime;

                    // If there is no user interaction for 20 sec, the process mode will be set to "Screen Saver Mode"
                    if (waitedTime > 20f) {
                        isScreenSaverMode = true;
                    }
                }

                prevDetected = false;
            } else {

                // Center of hand
                Vector3 tmp = rightHand.PalmPosition.ToVector3();

                // Map to world coordinate
                Vector3 result = new Vector3(5 + tmp.x * 20, 5 + tmp.z * 20, 0);

                handPosition = result;

            }

        } else {
            // Use mouse input position instead of leap's hand position
            Vector3 position = Input.mousePosition;
            position.z = 10f;
            handPosition = camera.ScreenToWorldPoint(position);
        }

        handObj.transform.position = handPosition;
    }
    
}
