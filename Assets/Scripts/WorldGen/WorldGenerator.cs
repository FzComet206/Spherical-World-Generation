using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private DataTypes.MeshSettings MeshSettings;
    [SerializeField] private DataTypes.WorldConfig WorldConfig;
    private MeshHelper meshHelper;
    private ComputeHelper computeHelper;

    private void Start()
    {
        meshHelper = FindObjectOfType<MeshHelper>();
        computeHelper = FindObjectOfType<ComputeHelper>();
        meshHelper.GenerateMesh(MeshSettings, computeHelper.GenerateNoise(WorldConfig));
    }
    


    void GenerateTexture()
    {
        
    }

}
