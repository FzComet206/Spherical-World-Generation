using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataTypes
{
    [Serializable]
    public struct TextureConfig
    {
        public ComputeShader compute;
        public int texWidth;
        public int texHeight;
    }
    
    [Serializable]
    public struct NoiseConfig
    {
        public int seed0;
        public int seed1;
        public int seed2;
        
        public int scale;
        public int octaves;
        public float gain;
        public float lacunarity;

        public int altitudeTurbulenceScale;
        public int altitudeTurbulenceAmplitude;

        public float altitudeToTemperatureWeight;
        public float altitudeToHumidityWeight;
    }

    [Serializable]
    public struct BiomeConfig
    {
        public AnimationCurve humidityToTemperatureCurve;
        public int curveRes;
        public List<Biome> biomes;
    } 
    
    [Serializable] 
    public struct WorldConfig
    {
        public TextureConfig tex;
        public NoiseConfig noise;
        public BiomeConfig biomes;
    }
    
    [Serializable]
    public enum RenderDistance 
    {
        low,
        medium,
        high
    }
    
    [Serializable]
    public struct Biome
    {
        public string name;
        public AnimationCurve heightCurve;
        public Color colorCode;
    }
    
    [Serializable]
    public struct MeshSettings
    {
        public int planeRes;
        public int numSubdivisions;
        public bool generateAll;
        public RenderDistance RenderDistance;
    }
    
    public struct ChunkConfig
    {
        public int chunkRes;
        public Vector2 startT;
        public Vector2 endT;
        public Vector3 face;

        public Vector3 axisA;
        public Vector3 axisB;

        public Vector3 center;

        public int index;
        public bool active;

        public ChunkConfig(int chunkRes, Vector2 startT, Vector2 endT, Vector3 face, Vector3 axisA, Vector3 axisB,
            Vector3 center, int index, bool active)
        {
            this.chunkRes = chunkRes;
            this.startT = startT;
            this.endT = endT;
            this.face = face;
            this.axisA = axisA;
            this.axisB = axisB;
            this.center = center;
            this.active = active;
            this.index = index;
        }
    }
    
    public struct MapThreadInfo
    {
        public readonly Action<ChunkData> callback;
        public readonly ChunkData parameter;

        public MapThreadInfo(Action<ChunkData> callback, ChunkData parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
    
    // adjust this
    public struct ChunkData
    {
        public Vector3[] verticies;
        public int[] triangles;
        public Vector2[] Uvs;

        public Vector3[] seaVertices;
        public int[] seatriangles;
        public Vector2[] seaUvs;

        public int index;

        public ChunkData(Vector3[] verticies, int[] triangles, Vector2[] Uvs, int index, Vector3[] seaVertices, int[] seatriangles, Vector2[] seaUvs)
        {
            this.verticies = verticies;
            this.triangles = triangles;
            this.Uvs = Uvs;
            this.index = index;
            this.seaVertices = seaVertices;
            this.seatriangles = seatriangles;
            this.seaUvs = seaUvs;
        }
    }
}
