using System;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] public DataTypes.MeshSettings MeshSettings;
    [SerializeField] public DataTypes.WorldConfig WorldConfig;
    private MeshHelper meshHelper;
    private ComputeHelper computeHelper;

    [SerializeField] GameObject player;
    public Stopwatch s;

    private void Start()
    {
        meshHelper = FindObjectOfType<MeshHelper>();
        computeHelper = FindObjectOfType<ComputeHelper>();

        if (WorldConfig.tex.texHeight > 8192 || WorldConfig.tex.texWidth > 8192)
        {
            throw new Exception("Recommanded texture resolution is below 8192");
        }
        
        if (MeshSettings.generateAll)
        {
            if (MeshSettings.planeRes > 512)
            {
                throw new Exception("Recommanded plane resolution when generate all is below 512");
            }
        }
        else
        {
            if (MeshSettings.planeRes > 2048)
            {
                throw new Exception("Recommanded plane resolution with culling is below 2048");
            }
        }
        
        computeHelper.GenerateNoise(WorldConfig);
        meshHelper.UpdateHeightMap();
        meshHelper.GenerateMesh(MeshSettings);

        Instantiate(player, Vector3.one * (1 + 0.0001f), Quaternion.Euler(0, 0, 0));
    }
}
