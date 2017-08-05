using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWaterPostEffect : MonoBehaviour {

    [SerializeField] private Shader shader;
    [SerializeField] private Texture2D backgroundTex;

    [SerializeField] private Vector3 lightVec = new Vector3(-2f, -0.5f, 0f);
    [SerializeField] private Vector3 viewVec = new Vector3(0, 0, 1);
    [SerializeField] private Vector4 lightness = new Vector4(0.3f, 0.11f, 0.59f, 0.0f);
    [SerializeField, Range(0f, 1f)] private float refraction = 0.5f;

    private Material material;

    void Awake() {
        if (material == null) {
            material = new Material(shader);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetTexture("_Background", backgroundTex);
        material.SetVector("_LightVec", lightVec);
        material.SetVector("_ViewVec", viewVec);
        material.SetVector("_Lightness", lightness);
        material.SetFloat("_Refraction", refraction);
        Graphics.Blit(source, destination, material);
    }
    

}
