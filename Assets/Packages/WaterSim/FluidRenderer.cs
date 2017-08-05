using UnityEngine;
using System.Collections;

namespace Kodai.Fluid.SPH {

    [RequireComponent(typeof(Fluid2D))]
    public class FluidRenderer : MonoBehaviour {

        public Fluid2D solver;

        public Material RenderParticleMat;
        public Camera RenderCamera;

        public bool Enable = true;
        public bool DiscoColor = true;
        public Color WaterColor;

        void OnRenderObject() {

            if (Enable && solver.Simulate) DrawParticle();
        }

        void DrawParticle() {

            

            Material m = RenderParticleMat;

            var inverseViewMatrix = RenderCamera.worldToCameraMatrix.inverse;

            m.SetPass(0);
            m.SetMatrix("_InverseMatrix", inverseViewMatrix);
            if (DiscoColor) {
                m.SetColor("_WaterColor", Color.HSVToRGB((Time.realtimeSinceStartup / 5f) % 1, 1, 1));
            } else {
                m.SetColor("_WaterColor", WaterColor);
            }
            m.SetBuffer("_ParticlesBuffer", solver.ParticlesBufferRead);
            m.SetBuffer("_ParticlesDensityBuffer", solver.ParticleDensitiesBuffer);
            Graphics.DrawProcedural(MeshTopology.Points, solver.NumParticles);
        }
    }
}