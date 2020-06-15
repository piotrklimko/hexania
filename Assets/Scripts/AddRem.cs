using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRem : MonoBehaviour
{

    public int current;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    HexChunk GetChunkWithHexCoords(Vector3 coords)
    {
        int chunk_x = Mathf.FloorToInt(coords.x / HexChunksManager.instance.chunkSize.x);
        int chunk_z = Mathf.FloorToInt(coords.z / HexChunksManager.instance.chunkSize.z);
        string s = chunk_x.ToString() + ":" + chunk_z.ToString();
        HexChunk hc = GameObject.Find(s).GetComponent<HexChunk>();

        return hc;
    }

    Vector3Int HexCoordsWithinChunk(Vector3 coords)
    {
        int x = (int)coords.x % HexChunksManager.instance.chunkSize.x;
        int y = (int)coords.y % HexChunksManager.instance.chunkSize.y;
        int z = (int)coords.z % HexChunksManager.instance.chunkSize.z;
        if (x < 0) x = HexChunksManager.instance.chunkSize.x - Mathf.Abs(x);
        if (z < 0) z = HexChunksManager.instance.chunkSize.z - Mathf.Abs(z);

        return new Vector3Int(x, y, z);
    }

    public void RemoveHex()
    {
        if (AimHex.isHexAimedAt)
        {

            HexChunk hc = GetChunkWithHexCoords(AimHex.hexAimedAt);
            Vector3Int v = HexCoordsWithinChunk(AimHex.hexAimedAt);

            hc.SetBlocks(v.x, v.y, v.z, 0);
            hc.ReCreateMeshes();
        }
    }

    public void AddHex()
    {

        bool isHexAimedAt = AimHex.isHexAimedAt;

        int dir = 0;
        if (isHexAimedAt)
        {

            Vector3 hexAimedAt = AimHex.hexAimedAt;
            Vector3 aimNormal = AimHex.aimNormal;

            if (aimNormal.y == 0)
            {
                if (aimNormal.x > 0 && aimNormal.z > 0) dir = 1;
                if (aimNormal.x > 0 && aimNormal.z == 0) dir = 2;
                if (aimNormal.x > 0 && aimNormal.z < 0) dir = 3;
                if (aimNormal.x < 0 && aimNormal.z < 0) dir = 4;
                if (aimNormal.x < 0 && aimNormal.z == 0) dir = 5;
                if (aimNormal.x < 0 && aimNormal.z > 0) dir = 6;
            }
            else if (aimNormal.y > 0)
            {
                dir = 7;
            }
            else if (aimNormal.y < 0)
            {
                dir = 8;
            }

            Vector3 hexToAdd = hexAimedAt;

            if (hexAimedAt.z % 2 == 0)
            {
                switch (dir)
                {
                    case 1:
                        hexToAdd = hexAimedAt + new Vector3(0, 0, 1);
                        break;
                    case 2:
                        hexToAdd = hexAimedAt + new Vector3(1, 0, 0);
                        break;
                    case 3:
                        hexToAdd = hexAimedAt + new Vector3(0, 0, -1);
                        break;
                    case 4:
                        hexToAdd = hexAimedAt + new Vector3(-1, 0, -1);
                        break;
                    case 5:
                        hexToAdd = hexAimedAt + new Vector3(-1, 0, 0);
                        break;
                    case 6:
                        hexToAdd = hexAimedAt + new Vector3(-1, 0, 1);
                        break;
                    case 7:
                        hexToAdd = hexAimedAt + new Vector3(0, 1, 0);
                        break;
                    case 8:
                        hexToAdd = hexAimedAt + new Vector3(0, -1, 0);
                        break;
                }
            }
            else
            {
                switch (dir)
                {
                    case 1:
                        hexToAdd = hexAimedAt + new Vector3(1, 0, 1);
                        break;
                    case 2:
                        hexToAdd = hexAimedAt + new Vector3(1, 0, 0);
                        break;
                    case 3:
                        hexToAdd = hexAimedAt + new Vector3(1, 0, -1);
                        break;
                    case 4:
                        hexToAdd = hexAimedAt + new Vector3(0, 0, -1);
                        break;
                    case 5:
                        hexToAdd = hexAimedAt + new Vector3(-1, 0, 0);
                        break;
                    case 6:
                        hexToAdd = hexAimedAt + new Vector3(0, 0, 1);
                        break;
                    case 7:
                        hexToAdd = hexAimedAt + new Vector3(0, 1, 0);
                        break;
                    case 8:
                        hexToAdd = hexAimedAt + new Vector3(0, -1, 0);
                        break;
                }
            }

            HexChunk hc = GetChunkWithHexCoords(hexToAdd);
            Vector3Int v = HexCoordsWithinChunk(hexToAdd);

            hc.SetBlocks(v.x, v.y, v.z, current);
            hc.ReCreateMeshes();
        }
    }
}
