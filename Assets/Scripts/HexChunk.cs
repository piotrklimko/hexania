using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
    int size_x = 30;
    int size_y = 100;
    int size_z = 30;


    private byte[,,] blocks;

    float hexSize=1.0f;
    float hexW;
    float hexH;

    int seed;

    float height_scale;
    float detail_scale;

    int level_snow=28;
    int level_grass=24;
    int level_sand=22;

    int base_y = 20;

    //public List<Material> materials;

    int vcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        

        //CreateData();

        //CreateMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateChunk(Vector3Int coords)
    {
        size_x = HexChunksManager.instance.chunkSize.x;
        size_y = HexChunksManager.instance.chunkSize.y;
        size_z = HexChunksManager.instance.chunkSize.z;

        hexW = Mathf.Sqrt(3) * hexSize;
        hexH = 2 * hexSize;

        height_scale = HexChunksManager.instance.height_scale;
        detail_scale = HexChunksManager.instance.detail_scale;

        seed = HexChunksManager.instance.seed;

        CreateData(coords);

        CreateMeshes();
    }

    void CreateData(Vector3Int coords)
    {
        blocks = new byte[size_x, size_y, size_z];

        int j = 1;

        for (int z = 0; z < size_z; z++)
        {
            for (int x = 0; x < size_x; x++)
            {
                //SetBlocks(x, 0, z, j++);
                //if (j == 4) j = 1;
                
                float perlin = Mathf.PerlinNoise((x+size_x*coords.x + seed) / detail_scale, (z +size_z*coords.z+ seed) / detail_scale) * height_scale;
                int y = Mathf.FloorToInt(perlin);

                y = y + base_y;

                if (y >= size_y) y = size_y - 1;
                if (y < 0) y = 0;

                if (y > level_snow)
                {
                    SetBlocks(x, y, z, 1);
                }
                else if (y >= level_grass)
                {
                    SetBlocks(x, y, z, 2);
                }
                else
                {
                    SetBlocks(x, y, z, 3);
                }
            }
        }
    }

    List<byte> ListBlocks()
    {

        List<byte> bytes = new List<byte>();

        for (int x = 0; x < size_x; x++)
        {
            for (int z = 0; z < size_z; z++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    byte i = GetBlocks(x, y, z);

                    bool isAlready = false;
                    foreach (byte k in bytes)
                    {
                        if (k == i)
                        {
                            isAlready = true;
                        }
                    }

                    if (!isAlready)
                    {
                        bytes.Add(i);
                    }
                }
            }
        }

        return bytes;
    }


    void CreateMeshes()
    {
        List<byte> blockTypesInChunk = ListBlocks();

        for (int i = 0; i < blockTypesInChunk.Count; i++)
        {
            int b = blockTypesInChunk[i];
            if (b != 0)
            {
                CreateMesh(b);
            }
        }
    }

    public void ReCreateMeshes()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        CreateMeshes();
    }

    void CreateMesh(int blockNum)
    {
        vcount = 0;
        List<Vector3> temp_verts = new List<Vector3>();
        List<Vector2> temp_uvs = new List<Vector2>();
        List<int> temp_triangles = new List<int>();

        bool quadsAdded = false;

        for (int x = 0; x < size_x; x++)
        {
            for (int z = 0; z < size_z; z++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    int i = GetBlocks(x, y, z);

                    if (i == blockNum)
                    {
                        float evenOdd = (z % 2 == 0) ? 0f : 0.5f * hexW;

                        //Instantiate(tempCubes[i], new Vector3(x*hexW + evenOdd, y, z*hexH*0.75f), Quaternion.Euler(-90f,0f,0f));
                        Vector3 center = new Vector3(x * hexW + evenOdd, y, z * hexH * 0.75f);

                        List<Vector3> hv = new List<Vector3>();
                        hv.Add(PointyHexCorner(center, hexSize, 0));
                        hv.Add(PointyHexCorner(center, hexSize, 1));
                        hv.Add(PointyHexCorner(center, hexSize, 2));
                        hv.Add(PointyHexCorner(center, hexSize, 3));
                        hv.Add(PointyHexCorner(center, hexSize, 4));
                        hv.Add(PointyHexCorner(center, hexSize, 5));

                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 0));
                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 1));
                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 2));
                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 3));
                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 4));
                        hv.Add(PointyHexCorner(center + Vector3.up, hexSize, 5));

                        Vector2[] topQuadUVs = { new Vector2(0, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 1), new Vector2(0, 1)};

                        Vector2[] sideQuadUVs = { new Vector2(1, 0.5f), new Vector2(0, 0.5f),new Vector2(0,0), new Vector2(1, 0)};

                        Vector2[] bottomQuadUVs = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 0.5f), new Vector2(0, 0.5f) };

                        if (GetAdjacentHex(x, y, z, 7) != i)
                        {
                            AddQuad(hv[9], hv[6], hv[11], hv[10], topQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            AddQuad(hv[9], hv[8], hv[7], hv[6], topQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 8) != i)
                        {
                            AddQuad(hv[3], hv[4], hv[5], hv[0], bottomQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            AddQuad(hv[1], hv[2], hv[3], hv[0], bottomQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }

                        if (GetAdjacentHex(x, y, z, 1) != i)
                        {
                            AddQuad(hv[6], hv[7], hv[1], hv[0], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 2) != i)
                        {
                            AddQuad(hv[7], hv[8], hv[2], hv[1], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 3) != i)
                        {
                            AddQuad(hv[8], hv[9], hv[3], hv[2], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 4) != i)
                        {
                            AddQuad(hv[9], hv[10], hv[4], hv[3], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 5) != i)
                        {
                            AddQuad(hv[10], hv[11], hv[5], hv[4], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                        if (GetAdjacentHex(x, y, z, 6) != i)
                        {
                            AddQuad(hv[11], hv[6], hv[0], hv[5], sideQuadUVs, temp_verts, temp_uvs, temp_triangles);
                            quadsAdded = true;
                        }
                    }
                }
            }
        }

        if (quadsAdded)
        {
            GameObject meshGameObject = new GameObject();
            meshGameObject.name = blockNum.ToString();
            meshGameObject.tag = "HexMesh";

            meshGameObject.transform.SetParent(transform, false);

            if (meshGameObject.GetComponent<MeshFilter>() == null)
            {
                meshGameObject.AddComponent(typeof(MeshFilter));
            }
            if (meshGameObject.GetComponent<MeshRenderer>() == null)
            {
                MeshRenderer renderer = meshGameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                renderer.material = HexChunksManager.instance.materials[blockNum - 1];
            }
            if (meshGameObject.GetComponent<MeshCollider>() == null)
            {
                meshGameObject.AddComponent<MeshCollider>();
            }


            Mesh mesh = new Mesh();
            mesh.name = gameObject.name+blockNum.ToString();

            mesh.vertices = temp_verts.ToArray();
            mesh.uv = temp_uvs.ToArray();
            mesh.triangles = temp_triangles.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshGameObject.GetComponent<MeshFilter>().mesh = mesh;
            meshGameObject.GetComponent<MeshCollider>().sharedMesh = meshGameObject.GetComponent<MeshFilter>().mesh;
        }
    }

    Vector3 PointyHexCorner(Vector3 center, float size, int i)
    {
        Vector3 vec = new Vector3(0, 0, size);
       
        Vector3 vec2 = new Vector3(vec.x*Mathf.Cos(-i*60 * Mathf.PI / 180f) - vec.z*Mathf.Sin(-i*60*Mathf.PI/180f),
                                    0,
                                    vec.x * Mathf.Sin(-i * 60 * Mathf.PI / 180f) + vec.z * Mathf.Cos(-i * 60 * Mathf.PI / 180f));
        Vector3 corner = center + vec2;
        return corner;
    }

    public byte GetAdjacentHex(int x, int y, int z, int i)
    {
        byte bl = 0;

        if (i == 7)
        {
            bl = GetBlocks(x, y+1, z);
            return bl;
        }
        else if (i == 8)
        {
            bl = GetBlocks(x, y-1, z);
            return bl;
        }

       

        if (z % 2 == 0)
        {
            switch (i)
            {
                case 1:
                    bl = GetBlocks(x, y, z+1);
                    break;
                case 2:
                    bl = GetBlocks(x+1, y, z);
                    break;
                case 3:
                    bl = GetBlocks(x, y, z-1);
                    break;
                case 4:
                    bl = GetBlocks(x-1, y, z-1);
                    break;
                case 5:
                    bl = GetBlocks(x-1, y, z);
                    break;
                case 6:
                    bl = GetBlocks(x-1, y, z+1);
                    break;
            }
        }
        else
        {
            switch (i)
            {
                case 1:
                    bl = GetBlocks(x+1, y, z + 1);
                    break;
                case 2:
                    bl = GetBlocks(x + 1, y, z);
                    break;
                case 3:
                    bl = GetBlocks(x+1, y, z - 1);
                    break;
                case 4:
                    bl = GetBlocks(x, y, z - 1);
                    break;
                case 5:
                    bl = GetBlocks(x - 1, y, z);
                    break;
                case 6:
                    bl = GetBlocks(x, y, z + 1);
                    break;
            }
        }

        return bl;
    }

    public byte GetBlocks(int x, int y, int z)
    {
        byte bl = 0;

        if (x < 0 || y < 0 || z < 0) return bl;
        if (x >= size_x || y >= size_y || z >= size_z) return bl;
        return blocks[x, y, z];
    }

    public void SetBlocks(int x, int y, int z, int cube)
    {
        if (x < 0 || y < 0 || z < 0) return;
        if (x >= size_x || y >= size_y || z >= size_z) return;
        blocks[x, y, z] = (byte)cube;
    }

    void AddQuad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector2[] uvs,List<Vector3> temp_verts, List<Vector2> temp_uvs, List<int> temp_triangles)
    {
        temp_verts.Add(v0);
        temp_verts.Add(v1);
        temp_verts.Add(v2);
        temp_verts.Add(v3);

        for (int i = 0; i < 4; i++)
        {
            temp_uvs.Add(uvs[i]);
        }

        temp_triangles.Add(vcount + 0);
        temp_triangles.Add(vcount + 2);
        temp_triangles.Add(vcount + 1);
        temp_triangles.Add(vcount + 0);
        temp_triangles.Add(vcount + 3);
        temp_triangles.Add(vcount + 2);

        vcount += 4;
    }

    
}
