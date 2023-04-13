![文章封面](https://user-images.githubusercontent.com/129722386/231022430-f6ca4338-a209-4bd8-a65e-9db894751e3b.png)

* [How to use](#how-to-use)
* [Main parameter](#main-parameter)
* [Template](#template)
* [Tutorial](#unity-shader-tutorial-seamless-2d-3d-worley-noise)
  * [Algorithm](#algorithm)
  * [Variant](#variant)
  * [Continuity](#continuity)
  * [FBM](#fbm)
* [My Social Media](#my-social-media)

# How to use

This project was created in Unity2021.3 

Click Generate to create noise texture

Click SaveToDisk to save noise texture to disk

![示例 主界面](https://user-images.githubusercontent.com/129722386/231022479-49d29131-0ec3-4ffa-ad3c-c825318db210.png)

# Main parameter

  SaveToDiskPath

  Resolution

  Frequency

  Is3D

  IsTilable: Should noise texture be seamless，make sure Resolution / Frequency is integer, for instance 256 / 4，256 / 8

  FbmIteration

  RemapTo01: Remap noise value to [0,1]

  Invert: Invert noise value
  
  ReturnType: noise style

  Evolution

Tip: 3D noise texture takes up a lot of memory and GPU resources, resolution should be no more than 256 (unless your graphic card is really good)

# Template

### Default 2D

![示例 默认2D](https://user-images.githubusercontent.com/129722386/231022787-be307d67-f464-4429-9a7b-7477deb800fa.png)

### ReturnType = IrregularRock

![示例 返回类型 2D](https://user-images.githubusercontent.com/129722386/231025149-67bcdf66-5ab3-4d0d-a1c3-e8f951a0e45a.png)

### Enable Invert

![示例 反相 2D](https://user-images.githubusercontent.com/129722386/231025199-c0fa750b-e895-4e85-9b81-72651e632e97.png)

### Disable/Enable Tilable

![示例 不连续 2D](https://user-images.githubusercontent.com/129722386/231022800-2d3f0982-356d-461c-bc17-215208603a7b.png)

![示例 四方连续 2D](https://user-images.githubusercontent.com/129722386/231022807-f4937466-189e-45ba-8390-9880117663e7.png)

### Enable RemapTo01, FbmIteration = 8

![示例 FMB 2D](https://user-images.githubusercontent.com/129722386/231022829-b81333a7-81b3-4681-b8c7-a5bbb855b315.png)

### Evolution, need assistance of C# script

```csharp
public class PerlinNoise_Test_Evolution : MonoBehaviour {
    public float evolutionSpeed;
    public PerlinNoise perlinNoise;

    private void FixedUpdate() {
        float time = Time.realtimeSinceStartup;
        if (time % 3 < 1) {
            perlinNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            perlinNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
            perlinNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
        }
        else if (time % 3 < 2) {
            perlinNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
            perlinNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            perlinNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
        }
        else {
            perlinNoise.evolution.x += Time.fixedDeltaTime * evolutionSpeed * 1.0f;
            perlinNoise.evolution.y += Time.fixedDeltaTime * evolutionSpeed * 0.5f;
            perlinNoise.evolution.z += Time.fixedDeltaTime * evolutionSpeed * 0.75f;
        }

        perlinNoise.Generate();
    }
}
```

https://user-images.githubusercontent.com/129722386/231022846-34cda4cb-9b7d-46b3-ae8a-7c72123d75e1.mp4

### Default 3D

![示例 默认3D](https://user-images.githubusercontent.com/129722386/231022897-b64218e9-0de0-4c3a-a694-ebe14a03494a.png)

### Disable/Enable Tilable

![示例 不连续 3D](https://user-images.githubusercontent.com/129722386/231022919-173a2637-7dec-4181-b017-7b46f11f565b.png)

![示例 六方连续3D](https://user-images.githubusercontent.com/129722386/231022946-4eedd492-eaf7-41e3-868f-038000642db2.png)

### Enable RemapTo01, FbmIteration = 8

![示例 FBM 3D](https://user-images.githubusercontent.com/129722386/231022965-4977515b-e0a7-4b0a-bad1-d09797edd6bb.png)



<br/><br/>

# Unity-Shader-Tutorial-Seamless-2D-3D-Worley-Noise

# Algorithm

### Let's take a look at a 256x256 worley noise

![教程0](https://user-images.githubusercontent.com/129722386/231023240-96ac8575-8f44-48da-8c9a-74f23227943a.png)

### Split it into 16 blocks

![教程1](https://user-images.githubusercontent.com/129722386/231023279-b8802462-8e74-4dcb-9e8c-e742c6276c2b.png)

### Draw blocks outside the texture

![教程2](https://user-images.githubusercontent.com/129722386/231023399-7876ff75-d460-4d96-b73d-727e35248773.png)

### Put a random point in each block

![教程3](https://user-images.githubusercontent.com/129722386/231023457-fa5969d5-7159-44cb-b6e2-db6b489435b2.png)

### All pixels are calculated in the same way, let's take pixel P for example

### Find out P's block

![教程4](https://user-images.githubusercontent.com/129722386/231023489-68e0ce71-64f2-4588-b4cd-a0a6499665a7.png)

### Calculate distances between P ans it's surrounding random points

```csharp
float GetDistance(float3 pnt0, float3 pnt1) {
    return distance(pnt0, pnt1);
}

float distances[9];

distances[0] = GetDistance(P, A);
distances[1] = GetDistance(P, B);
distances[2] = GetDistance(P, C);
distances[3] = GetDistance(P, D);
distances[4] = GetDistance(P, E);
distances[5] = GetDistance(P, F);
distances[6] = GetDistance(P, G);
distances[7] = GetDistance(P, H);
distances[8] = GetDistance(P, I);
```

### Sort the distances we have calculated

```csharp
float closestDistance = 999999;
float secondClosestDistance = 999999;
float thirdClosestDistance = 999999;

for(int iii = 0; iii < 9; iii++) {
    float tempDistance = distances[iii];

    if(tempDistance < closestDistance ) {
        thirdClosestDistance = secondClosestDistance ;
        secondClosestDistance = closestDistance ;
        closestDistance = tempDistance;
    }
    else if(tempDistance < secondClosestDistance ) {
        thirdClosestDistance = secondClosestDistance ;
        secondClosestDistance = tempDistance;
    }
    else if(tempDistance < thirdClosestDistance ) {
        thirdClosestDistance = tempDistance;
    }
}
```

### Texture is 256x256, 16 blocks were occupied, 64x64 for each block

```csharp
float noiseValue = closestDistance / 64;
```
### It's all done, we have worley noise value for P!

# Variant

### We can change noise style by modify calculation

```csharp
float GetDistance(float3 pnt0, float3 pnt1) {
    float3 vec = vec0 - vec1;
    return abs(vec .x) + abs(vec .y);
}
```

![教程6-1](https://user-images.githubusercontent.com/129722386/231024976-5f5ad160-0eaf-4014-8a98-85ba1a449852.png)

```csharp
float noiseValue = (secondClosestDistance - closestDistance) / 64;
```

![教程6-2](https://user-images.githubusercontent.com/129722386/231025024-39e8ebdd-237a-4579-a439-e1db2567a355.png)

# Continuity

### Take P1 and P2 for example

![教程5](https://user-images.githubusercontent.com/129722386/231024257-7f2344ac-c46c-4bb6-8304-192812878832.png)

### noise value of P1, P2 is determined by it's surrounding random points, so as long as relative position of random points are equal, it can connect seamlessly

![教程6](https://user-images.githubusercontent.com/129722386/231024714-8fc4995b-c9ca-4e49-b3d7-2cb37f67302e.png)

# FBM

### WorleyNoiseA, frequency = 4

![教程7](https://user-images.githubusercontent.com/129722386/231025385-4762ffc5-dff4-4fea-ae31-518f5690037c.png)

### WorleyNoiseB, frequency = 8

![教程8](https://user-images.githubusercontent.com/129722386/231025395-be306440-0a06-4cf8-b9e1-28ff3d71ceda.png)

### WorleyNoiseC, frequency = 16

![教程9](https://user-images.githubusercontent.com/129722386/231025404-7bb60acb-5c5b-44cc-9ce6-01135603933c.png)

### FBM = WorleyNoiseA + WorleyNoiseB * 0.5 + WorleyNoiseC * 0.25 ans so on, endup something like this

![教程10](https://user-images.githubusercontent.com/129722386/231025414-d84309d7-f26f-4866-889f-10f53022f265.png)

### Congratulations, you've learned basic worley noise!

# My Social Media

### Twitter : https://twitter.com/MagicStone23

### YouTube : https://www.youtube.com/channel/UCBUXiYqkFy0g6V0mVH1kESw

### zhihu : https://www.zhihu.com/people/shui-guai-76-84

### Bilibili : https://space.bilibili.com/423191063?spm_id_from=333.1007.0.0
