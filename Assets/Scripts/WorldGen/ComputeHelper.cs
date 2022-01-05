using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ComputeHelper : MonoBehaviour
{
    public void GenerateNoise(DataTypes.WorldConfig config)
    {
        // shader instance
        ComputeShader noises = config.tex.compute;
        
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
        
        RenderTexture heightMap = new RenderTexture(config.tex.texWidth, config.tex.texHeight, 1) {enableRandomWrite = true};
        heightMap.Create();

        // sample all the curves into one array and pass into gpu
        int res = config.biomes.curveRes;
        int l = config.biomes.biomes.Count;
        float[] allBiomeCurves = new float[res * l];
        for (int i = 0; i < config.biomes.biomes.Count; i++)
        {
            for (int j = 0; j < res; j++)
            {
                allBiomeCurves[i * res + j] = config.biomes.biomes[i].heightCurve.Evaluate(j / (float) res);
            }
        }
        
        ComputeBuffer biomeCurves = new ComputeBuffer(res * l, sizeof(float));
        biomeCurves.SetData(allBiomeCurves);

        // set data
        int handle = noises.FindKernel("NoiseGenerator");
        int handle1 = noises.FindKernel("HeightGenerator");
        
        noises.SetTexture(handle, "Elevation", elevationMap);
        noises.SetTexture(handle, "ContinentMap", continentMap);
        noises.SetTexture(handle, "Altitude", altitudeMap);
        noises.SetTexture(handle, "Proximity", proximityMap);
        noises.SetTexture(handle, "Temperature", temperatureMap);
        noises.SetTexture(handle, "Humidity", humidityMap);
        
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
        noises.SetInt("CurveRes", config.biomes.curveRes);
        
        noises.SetFloat("t1", 0.1f);
        noises.SetFloat("t2", 0.25f);
        noises.SetFloat("t3", 0.5f);
        noises.SetFloat("t4", 0.5f);
        noises.SetFloat("t5", 0.7f);
        noises.SetFloat("t6", 0.75f);
        noises.SetFloat("h8", 0.5f);
        noises.SetFloat("h9", 0.6f);
        noises.SetFloat("h10", 0.75f);
        noises.SetFloat("h11", 1f);
        noises.SetFloat("h12", 1f);
        noises.SetFloat("h13", 1f);
        
        // brrrrr
        noises.Dispatch(handle, config.tex.texWidth / 16, config.tex.texHeight / 16, 1);
        
        noises.SetBuffer(handle1, "BiomeCurves", biomeCurves);
        noises.SetTexture(handle1, "Proximity", proximityMap);
        noises.SetTexture(handle1, "ContinentMap", continentMap);
        noises.SetTexture(handle1, "Temperature", temperatureMap);
        noises.SetTexture(handle1, "Humidity", humidityMap);
        noises.SetTexture(handle1, "HeightMap", heightMap);
        
        noises.Dispatch(handle1, config.tex.texWidth / 16, config.tex.texHeight / 16, 1);
        
        biomeCurves.Dispose();
        // save
        Lib.DumpRenderTexture(continentMap, Configurations.dirPathContinent, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(altitudeMap, Configurations.dirPathAltitude, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(proximityMap, Configurations.dirPathProximity, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(elevationMap, Configurations.dirPathElevation, TextureFormat.RGBA32);
        Lib.DumpRenderTexture(temperatureMap, Configurations.dirPathTemerature, TextureFormat.R16);
        Lib.DumpRenderTexture(humidityMap, Configurations.dirPathHumidity, TextureFormat.R16);
        Lib.DumpRenderTexture(heightMap, Configurations.dirPathHeight, TextureFormat.R16);
    }
    
}
