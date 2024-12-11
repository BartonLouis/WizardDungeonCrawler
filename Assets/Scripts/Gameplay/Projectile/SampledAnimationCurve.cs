using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CustomTypes {
    public struct SampledAnimationCurve : System.IDisposable {
        NativeArray<float> sampledFloat;

        public SampledAnimationCurve(AnimationCurve ac, int samples, float scale) {
            sampledFloat = new NativeArray<float>(samples, Allocator.Persistent);
            float timeFrom = ac.keys[0].time;
            float timeTo = ac.keys[ac.keys.Length - 1].time;
            float timeStep = (timeTo - timeFrom) / (samples - 1);

            for (int i = 0; i < samples; i++) {
                sampledFloat[i] = scale * ac.Evaluate(timeFrom + (i * timeStep));
            }
        }

        public void Dispose() {
            sampledFloat.Dispose();
        }

        public float EvaluateLerp(float time) {
            int len = sampledFloat.Length - 1;
            float clamp01 = time < 0 ? 0 : (time > 1 ? 1 : time);
            float index = (clamp01 * len);
            int floorIndex = (int)math.floor(index);
            if (index == len) {
                return sampledFloat[len];
            }

            float lowerValue = sampledFloat[floorIndex];
            float higherValue = sampledFloat[floorIndex + 1];
            return math.lerp(lowerValue, higherValue, math.frac(index));
        }
    }
}