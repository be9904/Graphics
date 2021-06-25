using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mesh2SDFConverterTool : EditorWindow
{
    string sdfAssetName = "No Name";
    MeshFilter selectedMeshFilter = null;
    float voxelSize = 0.5f;
    bool sampleRandomPoints = false;
    bool smoothNormals = true;
    Material voxelMaterial;
    Material closestPointMaterial;

    [MenuItem("SDF/Mesh To SDF")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        Mesh2SDFConverterTool window = (Mesh2SDFConverterTool)EditorWindow.GetWindow(typeof(Mesh2SDFConverterTool));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        sdfAssetName = EditorGUILayout.TextField("Name", sdfAssetName);
        selectedMeshFilter = EditorGUILayout.ObjectField("Mesh", selectedMeshFilter, typeof(MeshFilter), true) as MeshFilter;
        voxelSize = EditorGUILayout.FloatField("Voxel Size", voxelSize);
        sampleRandomPoints = EditorGUILayout.Toggle("Sample Random Points", sampleRandomPoints);
        smoothNormals = EditorGUILayout.Toggle("Smooth Normals", smoothNormals);

        GUILayout.Label("Debug Settings", EditorStyles.boldLabel);
        voxelMaterial = EditorGUILayout.ObjectField("Voxel Material", voxelMaterial, typeof(Material), true) as Material;
        closestPointMaterial = EditorGUILayout.ObjectField("Closest Point On Mesh Material", closestPointMaterial, typeof(Material), true) as Material;

        if(GUILayout.Button("Generate SDF Asset"))
        {
            if (selectedMeshFilter == null)
                return;

            MeshToSDFProcessorSettings settings = new MeshToSDFProcessorSettings();
            settings.outputFilePath = EditorUtility.SaveFilePanel("Output SDF Asset", "", "", "sdf");
            if (settings.outputFilePath != null && settings.outputFilePath.Length > 0)
            {
                settings.assetName = sdfAssetName;
                settings.voxelSize = voxelSize;
                settings.sampleRandomPoints = sampleRandomPoints;
                settings.smoothNormals = smoothNormals;
                settings.voxelMaterial = voxelMaterial;
                settings.closestPointMaterial = closestPointMaterial;

                if (MeshToSDFProcessor.Convert(settings, selectedMeshFilter))
                    Debug.Log(string.Format("Created SDF asset \"{0}\" sucessfully!", sdfAssetName));
                else
                    Debug.LogError(string.Format("Failed to created SDF asset \"{0}\"!", sdfAssetName));
            }
        }
    }
}
