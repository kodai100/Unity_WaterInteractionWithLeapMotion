using UnityEngine;
using System.Runtime.InteropServices;

namespace Kodai.Fluid.SPH {
    public struct FluidParticle {
        public Vector2 Position;
        public Vector2 Velocity;
    };

    public class Fluid2D : FluidBase<FluidParticle> {

        [SerializeField, Range(3f, 5f)] private float ballRadius = 0.1f;
        [Range(1f, 3f)] public float MouseInteractionRadius = 1f;

        private Vector3 mousePos;
        private bool isMouseDown;

        protected override void InitParticleData(ref FluidParticle[] particles) {
            for (int i = 0; i < NumParticles; i++) {
                particles[i].Velocity = Vector2.zero;
                particles[i].Position = range / 2f + Random.insideUnitCircle * ballRadius;
            }
        }

        protected override void AdditionalCSParams(ComputeShader cs) {

            isMouseDown = true;
            mousePos = LeapMotionController.HandPosition;

            cs.SetVector("_MousePos", mousePos);
            cs.SetFloat("_Radius", MouseInteractionRadius);
            cs.SetBool("_MouseDown", isMouseDown);
        }

    }
}