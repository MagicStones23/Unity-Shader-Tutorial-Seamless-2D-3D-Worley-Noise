using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorleyNoise_Test_Evolution : MonoBehaviour {
    public float evolutionSpeed;
    public WorleyNoise worleyNoise;

    private void FixedUpdate() {
        float time = Time.realtimeSinceStartup;
        if (time % 3 < 1) {
            worleyNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            worleyNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
            worleyNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
        }
        else if (time % 3 < 2) {
            worleyNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
            worleyNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            worleyNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
        }
        else {
            worleyNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            worleyNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
            worleyNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
        }

        worleyNoise.Generate();
    }
}