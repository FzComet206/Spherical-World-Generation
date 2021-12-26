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
        // shader instance
        ComputeShader noises = config.tex.compute;
        
        // data buffers
        ComputeBuffer HToTCurveSample =
            Lib.SampleCurveToBuffer(config.biomes.humidityToTemperatureCurve, config.biomes.curveRes);
        
        int colorCount = config.biomes.biomes.Count;
        ComputeBuffer BiomeColor = new ComputeBuffer(colorCount, sizeof(float) * 4); 
        Color[] colors = new Color[colorCount];
        for (int i = 0; i < colorCount; i++)
        {
            colors[i] = config.biomes.biomes[i].colorCode;
        }
        BiomeColor.SetData(colors);

        // first iteration output
        RenderTexture continentMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        RenderTexture altitudeMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        RenderTexture proximityMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        RenderTexture elevationMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        continentMap.Create();
        altitudeMap.Create();
        proximityMap.Create();
        elevationMap.Create();
        
        // second iteration output
        RenderTexture temperatureMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        RenderTexture humidityMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        temperatureMap.Create();
        humidityMap.Create();
        
        // final output
        RenderTexture biomeMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        biomeMap.Create();
        
        // set data
        int handle = noises.FindKernel("NoiseGenerator");
        
        noises.SetBuffer(handle, "HtoTCurveSample", HToTCurveSample);
        noises.SetBuffer(handle, "BiomeColors", BiomeColor);
        
        noises.SetTexture(handle, "Elevation", elevationMap);
        noises.SetTexture(handle, "ContinentMap", continentMap);
        noises.SetTexture(handle, "Altitude", altitudeMap);
        noises.SetTexture(handle, "Proximity", proximityMap);
        
        noises.SetTexture(handle, "Temperature", temperatureMap);
        noises.SetTexture(handle, "Humidity", humidityMap);
        
        noises.SetTexture(handle, "BiomeMap", biomeMap);
        
        // texture and noises settings
        noises.SetInt("randSeed1", config.noise.seed0);
        noises.SetInt("randSeed2", config.noise.seed1);
        noises.SetInt("randSeed3", config.noise.seed2);
        noises.SetInt("randSeed4", config.noise.seed3);
        noises.SetInt("width", config.tex.texWidth);
        noises.SetInt("height", config.tex.texHeight);
        noises.SetInt("octaves", config.noise.octaves);
        noises.SetInt("scale", config.noise.scale);
        noises.SetFloat("lacunarity", config.noise.lacunarity);
        noises.SetFloat("gain", config.noise.gain);
        noises.SetInt("altitudeScale", config.noise.altitudeTurbulenceScale);
        noises.SetInt("altitudeAmplitude", config.noise.altitudeTurbulenceAmplitude);
        noises.SetFloat("AtoTWeight", config.noise.altitudeToTemperatureWeight);
        noises.SetFloat("AtoHWeight", config.noise.altitudeToHumidityWeight);
        
        // brrrrr
        noises.Dispatch(handle, config.tex.texWidth / 16, config.tex.texHeight / 16, 1);
        
        HToTCurveSample.Dispose();
        BiomeColor.Dispose();
        
        // save
        Lib.DumpRenderTexture(continentMap, Configurations.dirPathContinent, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(altitudeMap, Configurations.dirPathAltitude, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(proximityMap, Configurations.dirPathProximity, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(temperatureMap, Configurations.dirPathTemerature, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(humidityMap, Configurations.dirPathHumidity, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(elevationMap, Configurations.dirPathElevation, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(biomeMap, Configurations.dirPathBiomes, TextureFormat.RGBA32);
    }
    
}
