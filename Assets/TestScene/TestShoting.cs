using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestShoting : MonoBehaviour
{
    [SerializeField]
    BillboardShots[] shots;

    void Update()
    {
        for(int i = 0; i < 50; i++)
            foreach(var s in shots)
            s.Fire(new TestShot(), transform.position, new Vector3(Random.value, Random.value, Random.value).normalized, 1200, 3, null);

        foreach (var s in shots)
            s.UpdateMesh();
    }
}