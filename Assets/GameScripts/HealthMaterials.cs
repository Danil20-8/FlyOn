using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HealthMaterials: MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    int nMaterials = 8;

    Material[] materials;

    static HealthMaterials current;

    void Awake()
    {
        current = this;

        materials = new Material[nMaterials];

        for(int i = 0; i < nMaterials; i++)
        {
            var mat = Instantiate(material);
            mat.color = Color.Lerp(material.color, Color.red, 1f / nMaterials * (nMaterials - i));
            materials[i] = mat;
        }
    }

    public static Material GetMaterial(float t)
    {
        return current.materials[(int)((current.materials.Length - 1) * t)];
    }
}

