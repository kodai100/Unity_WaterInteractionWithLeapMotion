using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrefsGUI;
using Kodai.Fluid.SPH;
using UnityEngine.Video;

public class Manager : SingletonMonoBehaviour<Manager> {

    [SerializeField] private GameObject cameraObj;
    [SerializeField] private GameObject videoPlayerObj;

    [SerializeField] private Fluid2D fluidSim;
    [SerializeField] private FluidRenderer fluidRenderer;

    [SerializeField] private bool isGUIOpend;

    private RealWaterPostEffect waterRenderer;
    private MoviePostEffect movieRenderer;
    private VideoPlayer videoPlayer;
    
    private bool coroutineCalled;

    void Start () {
        isGUIOpend = false;

        videoPlayer = videoPlayerObj.GetComponent<VideoPlayer>();

        waterRenderer = cameraObj.GetComponent<RealWaterPostEffect>();
        movieRenderer = cameraObj.GetComponent<MoviePostEffect>();

        waterRenderer.enabled = true;
        movieRenderer.enabled = false;
	}

	void Update () {

        if (Input.GetKeyUp(KeyCode.D)) {
            isGUIOpend = isGUIOpend ? false : true;
        }

        if (LeapMotionController.IsScreenSaverMode) {

            if (!videoPlayer.isPlaying) {

                // スクリーンセーバーモードでかつビデオが停止している場合、1度だけコルーチンを呼んで処理抜けする
                if (!coroutineCalled) {
                    videoPlayer.Play();
                    videoPlayer.time = 15d;

                    StartCoroutine(DelayPlay());
                }
            } 

        } else {

            // スクリーンセーバーモードではない場合

            // ビデオが再生されていたら停止してポストエフェクトを切り替える
            if (videoPlayer.isPlaying) {
                videoPlayer.Pause();
                SwapImageEffects();
            }

            fluidSim.Simulate = true;
            
        }
        
    }
    
    void SwapImageEffects() {
        waterRenderer.enabled = waterRenderer.enabled ? false : true;
        movieRenderer.enabled = movieRenderer.enabled ? false : true;
    }

    IEnumerator DelayPlay() {

        coroutineCalled = true;

        // videoPlayer.timeが即時に反映されないので1秒待つ
        yield return new WaitForSeconds(1f);

        // 1秒待った後にポストエフェクトを切り替える
        SwapImageEffects();

        // シミュレーションを停止する
        fluidSim.Simulate = false;

        coroutineCalled = false;
    }




    // ------------------------------------------------------------------------
    // GUI
    // ------------------------------------------------------------------------

    private PrefsBool isUseLeap = new PrefsBool("Use Leap Motion ?", false);
    private PrefsFloat viscosity = new PrefsFloat("Viscosity", 1);
    private PrefsFloat handRadius = new PrefsFloat("Hand Radius", 1);
    private PrefsVector2 range = new PrefsVector2("Range", new Vector2(10, 10));
    private PrefsVector2 gravity = new PrefsVector2("Gravity", new Vector2(0, -10));

    private Rect windowRect = new Rect();

    private void OnGUI() {

        if (isGUIOpend) {
            windowRect = GUILayout.Window(GetHashCode(), windowRect, (id) => {
                OnGUIInternal();
                GUI.DragWindow();
            },
            "Manager",
            GUILayout.MinWidth(MinWidth));
        }

        SetGUIValues();
    }

    void SetGUIValues() {

        LeapMotionController.IsUseLeap = isUseLeap;
        fluidSim.Viscosity = viscosity;
        fluidSim.MouseInteractionRadius = handRadius;
        fluidSim.Range = range;
        fluidSim.Gravity = gravity;

    }

    void OnGUIInternal() {

        isUseLeap.OnGUI();

        GUILayout.Space(10);

        GUILayout.Label("Water Sim");
        viscosity.OnGUISlider(1, 80);
        handRadius.OnGUISlider(1, 3);
        range.OnGUISlider(new Vector2(5, 5), new Vector2(20, 20));
        gravity.OnGUISlider(new Vector2(-10, -10), new Vector2(10, 10));

        if (GUILayout.Button("Save")) Prefs.Save();
        if (GUILayout.Button("DeleteAll")) Prefs.DeleteAll();
    }

    float MinWidth { get { return 500f; } }
}
