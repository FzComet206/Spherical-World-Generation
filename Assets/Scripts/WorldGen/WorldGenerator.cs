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
        
        computeHelper.GenerateNoise(WorldConfig);
        meshHelper.UpdateHeightMap();
        meshHelper.GenerateMesh(MeshSettings);

        Instantiate(player, Vector3.one * (1 + 0.0001f), Quaternion.Euler(0, 0, 0));
    }
}
