using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] public DataTypes.MeshSettings MeshSettings;
    [SerializeField] public DataTypes.WorldConfig WorldConfig;
    private MeshHelper meshHelper;
    private ComputeHelper computeHelper;

    public Stopwatch s;

    private void Start()
    {
        s = Stopwatch.StartNew();
        meshHelper = FindObjectOfType<MeshHelper>();
        computeHelper = FindObjectOfType<ComputeHelper>();
        
        computeHelper.GenerateNoise(WorldConfig);
        meshHelper.UpdateHeightMap();
        meshHelper.GenerateMesh(MeshSettings);
    }
}
