using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePostEffect : MonoBehaviour {

    [SerializeField] private Shader shader;
    [SerializeField] private RenderTexture movieTex;

    private Material material;

    void Awake() {
        if (material == null) {
            material = new Material(shader);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetTexture("_VideoTex", movieTex);
        Graphics.Blit(source, destination, material);
    }


}
