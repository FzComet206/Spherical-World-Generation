using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshHelper: MonoBehaviour
{
    // local variables
    private int renderD;
    private Vector3 prev;
    private List<GameObject> meshPool = new List<GameObject>();
    private Dictionary<int, GameObject> meshDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> seaDictionary = new Dictionary<int, GameObject>();
    private DataTypes.ChunkConfig[] genCullConfig;
    
    public float speed;
    public Vector3 playerPos = Vector3.zero;
    public List<DataTypes.ChunkConfig> renderingChunk;
    public Stack<DataTypes.ChunkConfig> genConfig;
    
    private MeshThread meshThread;
    private WorldGenerator worldGen;
    
    public Material meshMaterial;
    public Transform meshParent;
    public Material seaMaterial;
    public Transform seaParent;

    private List<Vector3> facesDir = new List<Vector3>()
    {
        Vector3.up,
        Vector3.down, 
        Vector3.forward,
        Vector3.back,
        Vector3.right,
        Vector3.left
    };

    private void Start()
    {
        meshThread = FindObjectOfType<MeshThread>();
        worldGen = FindObjectOfType<WorldGenerator>();
    }
    public void GenerateMesh(DataTypes.MeshSettings settings)
    {
        int numSubDivisions = settings.numSubdivisions;
        int planeRes = settings.planeRes;
        
        genConfig = new Stack<DataTypes.ChunkConfig>();
        genCullConfig = new DataTypes.ChunkConfig[numSubDivisions * numSubDivisions * 6];
        renderingChunk = new List<DataTypes.ChunkConfig>();
        
        int i = 0;
        
        float faceCoveragePerSubFace = 1f / numSubDivisions;
        int chunkRes = planeRes / numSubDivisions;
        DataTypes.RenderDistance RenderDistance = settings.RenderDistance;
        bool generateAllMesh = settings.generateAll;
        
        foreach (Vector3 dir in facesDir)
        {
            Vector3 axisA = new Vector3(dir.y, dir.z, dir.x);
            Vector3 axisB = Vector3.Cross(dir, axisA);
                
            for (int y = 0; y < numSubDivisions; y++)
            {
                for (int x = 0; x < numSubDivisions; x++)
                {
                    Vector2 startT = new Vector2(x, y) * faceCoveragePerSubFace;
                    Vector2 endT = startT + Vector2.one * faceCoveragePerSubFace;

                    // below few lines compute center vector for each chunk
                    float dx = (endT.x - startT.x) / (chunkRes - 1);
                    float dy = (endT.y - startT.y) / (chunkRes - 1);
                
                    float tx = startT.x + dx * (chunkRes / 2f - 1f);
                    float ty = startT.y + dy * (chunkRes / 2f - 1f);

                    Vector3 center = dir + (tx - 0.5f) * 2 * axisA + (ty - 0.5f) * 2 * axisB;
                    center = Lib.PointOnCubeToPointOnSphere(center);
                        
                    // set
                    DataTypes.ChunkConfig c = new DataTypes.ChunkConfig(chunkRes, startT, endT, dir, axisA, axisB, center, i, false);
                    genCullConfig[i] = c;
                    genConfig.Push(c);
                    i++;
                }
            }
        }
        
        switch (RenderDistance)
        {
            case DataTypes.RenderDistance.low: renderD = 9;
                break;
            case DataTypes.RenderDistance.medium: renderD = 25;
                break;
            case DataTypes.RenderDistance.high: renderD = 49;
                break;
        }

        if (generateAllMesh)
        {
            StartCoroutine(MeshThreadUpdate());
        }
        else
        {
            StartCoroutine(GenerateWithCulling());
        }
        
        StartCoroutine(meshThread.ThreadQueueUpdate());
    }
    
    
    void DetectAndUpdateChunks()
    {
        // update rendering chunk
        HashSet<int> l = new HashSet<int>();

        while (l.Count < renderD)
        {
            int smallest = 0;
            
            for (int i = 0; i < genCullConfig.Length; i++)
            {
                float dist = (playerPos - genCullConfig[i].center).magnitude;
                float currSmallest = (playerPos - genCullConfig[smallest].center).magnitude;
                
                if (dist < currSmallest && !l.Contains(i)) smallest = i;
            }
            
            l.Add(smallest);
        }
        
        foreach (var x in l)
        {
            if (!genCullConfig[x].active)
            {
                genCullConfig[x].active = true;
                meshThread.RequestMapData(OnChunkDataReceived, genCullConfig[x]);
            }
        }

        foreach (var v in meshDictionary)
        {
            if (!l.Contains(v.Key))
            {
                genCullConfig[v.Key].active = false;
                Destroy(v.Value);
                Destroy(seaDictionary[v.Key]);
            }
        }
    }

    IEnumerator GenerateWithCulling()
    {
        while (true)
        {
            if ((playerPos - prev).magnitude > 0.01f)
            {
                DetectAndUpdateChunks();
            }
            prev = playerPos;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateHeightMap()
    {
        int w = worldGen.WorldConfig.tex.texWidth;
        int h = worldGen.WorldConfig.tex.texHeight;
        Texture2D tex = Lib.ReadFromPng(Configurations.dirPathN);
        meshThread.heightMap = new Color[w][];
        for (int i = 0; i < w; i++)
        {
            meshThread.heightMap[i] = new Color[h];
            for (int j = 0; j < h; j++)
            {
                meshThread.heightMap[i][j] = tex.GetPixel(i, j);
            }
        }
    }

    void OnChunkDataReceived(DataTypes.ChunkData data)
    {
        // obj is the game object for each chunk, and is populated with layers of marching squared meshes
        GameObject obj = new GameObject("Chunk", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        for (int i = 0; i < data.triangles.Count; i++)
        {
            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = data.verticies[i];
            mesh.triangles = data.triangles[i];
            mesh.uv = data.Uvs[i];
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
            GameObject l = new GameObject("Layer", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            l.GetComponent<MeshFilter>().sharedMesh = mesh;
            l.GetComponent<MeshRenderer>().material = meshMaterial;
            l.GetComponent<MeshCollider>().sharedMesh = mesh;
            l.transform.parent = obj.transform;
        }
        meshDictionary[data.index] = obj;
        
        
        Mesh sea = new Mesh();
        sea.indexFormat = IndexFormat.UInt32;
        sea.vertices = data.seaVertices;
        sea.triangles = data.seatriangles;
        sea.uv = data.seaUvs;
        
        sea.RecalculateNormals();
        sea.RecalculateBounds();
        
        GameObject seaPiece = new GameObject("SeaChunk", typeof(MeshFilter), typeof(MeshRenderer));
        seaPiece.GetComponent<MeshFilter>().sharedMesh = sea;
        seaPiece.GetComponent<MeshRenderer>().material = seaMaterial;
        seaPiece.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;

        seaPiece.transform.parent = seaParent.transform;

        seaDictionary[data.index] = seaPiece;
    }
    
    IEnumerator MeshThreadUpdate()
    {
        while (genConfig.Count > 0)
        {
            while (meshThread.threadInfoQueue.Count < 50)
            {
                try
                {
                    meshThread.RequestMapData(OnChunkDataReceived, genConfig.Pop());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
