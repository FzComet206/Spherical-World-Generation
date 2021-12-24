using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ComputeHelper : MonoBehaviour
{
    public void GenerateNoise(DataTypes.WorldConfig config)
    {
        ComputeShader noises = config.tex.compute;
        RenderTexture noiseMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        noiseMap.Create();
        
        int handle0 = noises.FindKernel("NoiseGenerator");
        noises.SetTexture(handle0, "NoiseMap", noiseMap);
        
        noises.SetInt("width", config.tex.texWidth);
        noises.SetInt("height", config.tex.texHeight);
        noises.SetInt("scale", config.noise.scale);
        noises.SetInt("randSeed1", config.noise.seed0);
        noises.SetInt("randSeed2", config.noise.seed1);
        noises.SetInt("randSeed3", config.noise.seed2);
        noises.SetInt("octaves", config.noise.octaves);
        
        noises.SetFloat("lacunarity", config.noise.lacunarity);
        noises.SetFloat("gain", config.noise.gain);
        
        noises.Dispatch(handle0, config.tex.texWidth / 16, config.tex.texHeight / 16, 1);

        Lib.DumpRenderTexture(noiseMap, Configurations.dirPathN, TextureFormat.RGBA32);
    }
    
}
