using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] public DataTypes.MeshSettings MeshSettings;
    [SerializeField] public DataTypes.WorldConfig WorldConfig;
    private MeshHelper meshHelper;
    private ComputeHelper computeHelper;

    private void Start()
    {
        meshHelper = FindObjectOfType<MeshHelper>();
        computeHelper = FindObjectOfType<ComputeHelper>();
        
        computeHelper.GenerateNoise(WorldConfig);
        meshHelper.UpdateHeightMap();
        meshHelper.GenerateMesh(MeshSettings);
    }
}
