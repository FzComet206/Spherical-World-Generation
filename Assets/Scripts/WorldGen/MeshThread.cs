using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MeshThread : MonoBehaviour
{
    public float heightScale;
    
    public Queue<DataTypes.MapThreadInfo> threadInfoQueue = new Queue<DataTypes.MapThreadInfo>();
    public Color[][] heightMap;

    public void RequestMapData(Action<DataTypes.ChunkData> callback, DataTypes.ChunkConfig config)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback, config);
        };
    
        new Thread(threadStart).Start();
    }

    void MapDataThread(Action<DataTypes.ChunkData> callback, DataTypes.ChunkConfig config)
    {
        DataTypes.ChunkData data = GenerateChunk(config);
    
        lock (threadInfoQueue)
        {
            threadInfoQueue.Enqueue(new DataTypes.MapThreadInfo(callback, data));
        }                  
    }

    public IEnumerator ThreadQueueUpdate()
    {
        while (true)
        {
            if (threadInfoQueue.Count > 0)
            {
                for (int i = 0; i < threadInfoQueue.Count; i++)
                {
                    DataTypes.MapThreadInfo threadInfo = threadInfoQueue.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
            yield return null;
        }
    }

    DataTypes.ChunkData GenerateChunk(DataTypes.ChunkConfig config)
    {
        Vector3 face = config.face;
        Vector2 startT = config.startT;
        Vector2 endT = config.endT;
        int res = config.chunkRes;
    
        // below initializes main mesh data
        int vertLength = res * res;
        int triLength = (res - 1) * (res - 1) * 6;

        Vector3[] vertices = new Vector3[vertLength];
        int[] triangles = new int[triLength];
        Vector3[] flatVert = new Vector3[triLength];
        Vector2[] uvs = new Vector2[vertLength];
        Vector2[] flatUvs = new Vector2[triLength];
        
        int triIndex = 0;
        float ty = startT.y;
        float dx = (endT.x - startT.x) / (res - 1);
        float dy = (endT.y - startT.y) / (res - 1);

        int width = heightMap.Length - 1;
        int height = heightMap[0].Length - 1;

        for (int y = 0; y < res; y++)
        {
            float tx = startT.x;
            
            for (int x = 0; x < res; x++)
            {
                int i = x + y * res;

                Vector3 pointOnUnitCube = face + (tx - 0.5f) * 2 * config.axisA + (ty - 0.5f) * 2 * config.axisB;
                Vector3 pointOnSphere = Lib.PointOnCubeToPointOnSphere(pointOnUnitCube);

                Vector2 c = Lib.PointToCoordinate(pointOnSphere).ToUV();
                uvs[i] = c;
                
                int u = Mathf.FloorToInt(c.x * width);
                int v = Mathf.FloorToInt(c.y * height);

                float h = heightMap[u][v].r;
                
                h = Mathf.FloorToInt(h * 10);
                h *= 0.2f;
                // h = Mathf.SmoothStep(0, 1, h);
                
                h = h * 0.001f * heightScale;
               
                pointOnSphere *= h + 1;
                vertices[i] = pointOnSphere;
                
                if (x != res - 1 && y != res - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + res + 1;
                    triangles[triIndex + 2] = i + res;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + res + 1;
                    triIndex += 6;
                }

                tx += dx;
            }
            ty += dy;
        }
    
        // this applies flat shading
        // Vector2[] flatShadedUvs = new Vector2[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            flatVert[i] = vertices[triangles[i]];
            flatUvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }
        
        // below initializes sea mesh data
        int seaRes = res / 4;
        int seaVertLength = seaRes * seaRes;
        int seaTriLength = (seaRes - 1) * (seaRes - 1) * 6;

        Vector3[] seaVert = new Vector3[seaVertLength];
        int[] seaTri = new int[seaTriLength];
        Vector2[] seaUvs = new Vector2[seaVertLength];

        float tys = startT.y;
        int seaTriIndex = 0;
        float dxs = (endT.x - startT.x) / (seaRes - 1);
        float dys = (endT.y - startT.y) / (seaRes - 1);
        
        for (int y = 0; y < seaRes; y++)
        {
            float txs = startT.x;
            
            for (int x = 0; x < seaRes; x++)
            {
                int i = x + y * seaRes;

                Vector3 pointOnUnitCube = face + (txs - 0.5f) * 2 * config.axisA + (tys - 0.5f) * 2 * config.axisB;
                Vector3 pointOnSphere = Lib.PointOnCubeToPointOnSphere(pointOnUnitCube);

                seaVert[i] = pointOnSphere * (1 + (0.0005f) * heightScale);
                seaUvs[i] = Lib.PointToCoordinate(pointOnSphere).ToUV();
                
                if (x != seaRes - 1 && y != seaRes - 1)
                {
                    seaTri[seaTriIndex] = i;
                    seaTri[seaTriIndex + 1] = i + seaRes + 1;
                    seaTri[seaTriIndex+ 2] = i + seaRes;

                    seaTri[seaTriIndex + 3] = i;
                    seaTri[seaTriIndex + 4] = i + 1;
                    seaTri[seaTriIndex + 5] = i + seaRes + 1;
                    seaTriIndex += 6;
                }
                txs += dxs;
            }
            tys += dys;
        }
        
        DataTypes.ChunkData data = new DataTypes.ChunkData(flatVert, triangles, flatUvs, config.index, seaVert, seaTri, seaUvs);
        return data;
    }
}
