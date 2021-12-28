using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;

public class MeshThread : MonoBehaviour
{
    public float heightScale;
    public int numberOfHeightLayers;
    public float cliffHeight;
    public int vertexArrInitialCount;

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
    
        // below initializes main return data

        Vector3[] vertArr = new Vector3[vertexArrInitialCount];
        Vector2[] uvArr = new Vector2[vertexArrInitialCount];
        
        float dx = (endT.x - startT.x) / (res - 1);
        float dy = (endT.y - startT.y) / (res - 1);

        int width = heightMap.Length - 1;
        int height = heightMap[0].Length - 1;

        int numVertex = 0;

        // for each square, generate 8 vertex index and 8 vertex position
        for (int i = 0; i < numberOfHeightLayers; i++)
        {
            float ty = startT.y;
            
            for (int y = 0; y < res; y ++)
            {
                float tx = startT.x;
                
                for (int x = 0; x < res; x ++)
                {
                    // get position for current vertex (square)
                    Node topLeft = InitializePoints(config, face, tx, ty, width, height, i);
                    Node centerTop = InitializePoints(config, face, tx + 0.5f * dx, ty, width, height, i);
                    Node topRight = InitializePoints(config, face, tx + dx, ty, width, height, i);
                    Node centerRight = InitializePoints(config, face, tx + dx, ty + 0.5f * dy, width, height, i);
                    
                    Node bottomRight = InitializePoints(config, face, tx + dx, ty + dy, width, height, i);
                    Node centerBot = InitializePoints(config, face, tx + 0.5f * dx, ty + dy, width, height, i);
                    Node bottomLeft = InitializePoints(config, face, tx, ty + dy, width, height, i);
                    Node centerLeft = InitializePoints(config, face, tx, ty + 0.5f * dy, width, height, i);

                    // ignore edge
                    if (x != res - 1 && y != res - 1)
                    {
                        Square march = new Square(centerLeft, centerTop, centerRight, centerBot,
                            topLeft, topRight, bottomRight, bottomLeft);
                        numVertex = march.March(vertArr, uvArr, numVertex, cliffHeight);
                    }
                    
                    tx += dx;
                }
                ty += dy;
            }
        }

        // truncate array
        Vector3[] verticies = new Vector3[numVertex];
        Vector2[] uvs = new Vector2[numVertex];

        
        Array.Copy(vertArr, verticies, numVertex);
        Array.Copy(uvArr, uvs, numVertex);
        
        int[] triangles = new int[numVertex];
        for (int i = 0; i < numVertex; i++)
        {
            triangles[i] = i;
        }
        
        // below initializes sea mesh data
        int seaRes = res;
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
        
        DataTypes.ChunkData data = new DataTypes.ChunkData(verticies, triangles, uvs, config.index, seaVert, seaTri, seaUvs);
        return data;
    }

    private Node InitializePoints(DataTypes.ChunkConfig config, Vector3 face, float tx, float ty, int width, int height, int i)
    {
        Vector3 pos = face + (tx - 0.5f) * 2 * config.axisA + (ty - 0.5f) * 2 * config.axisB;
        Vector3 posReal= Lib.PointOnCubeToPointOnSphere(pos);
        Vector2 c = Lib.PointToCoordinate(posReal).ToUV();
        int u = Mathf.FloorToInt(c.x * (width - 1));
        int v = Mathf.FloorToInt(c.y * (height - 1));
        int h = Mathf.CeilToInt(heightMap[u][v].r * numberOfHeightLayers);
        
        return new Node(posReal * (1 + 0.001f * i * heightScale), c , h, i, i + 1);
    }
}
