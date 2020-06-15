using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexChunksManager : MonoBehaviour
{
    public GameObject chunkPfb;

    public static HexChunksManager instance;

    public Vector3Int chunkSize;


    private byte[,,] blocks;

    public int seed;

    public float height_scale;
    public float detail_scale;

    public int level_snow = 28;
    public int level_grass = 24;
    public int level_sand = 22;

    public int base_y = 20;

    public List<Material> materials;

    float hexSize = 1.0f;
    float hexW;
    float hexH;
    float xspace;
    float zspace;

    public int visibleRadius;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        
        hexW = Mathf.Sqrt(3) * hexSize;
        hexH = 2 * hexSize;
        xspace = hexW * 0.5f*2;
        zspace = hexH * 0.75f;


        for (int z = -visibleRadius; z <= visibleRadius; z++)
        {
            for (int x = -visibleRadius; x <= visibleRadius; x++)
            {
                CreateChunkXZ(x, z);
            }
        }
        

    }

    void CreateChunkXZ(int x, int z)
    {
        GameObject o = Instantiate(chunkPfb, new Vector3(x * chunkSize.x * xspace, 0, z * chunkSize.z * zspace), Quaternion.identity) as GameObject;
        o.GetComponent<HexChunk>().CreateChunk(new Vector3Int(x, 0, z));
        o.name = x.ToString() + ":" + z.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
