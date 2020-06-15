using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHex : MonoBehaviour
{

    public RectTransform img_crosshair;
    public float ray_length;

    public static bool isHexAimedAt = false;
    public static Vector3 hexAimedAt = new Vector3();
    public static Vector3 aimNormal = new Vector3();


    // Start is called before the first frame update
    void Start()
    {

   

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(img_crosshair.position);
        if (Physics.Raycast(ray, out hit, ray_length))
        {
            if (hit.collider.tag == "HexMesh")
            {
                isHexAimedAt = true;
                hexAimedAt = GetHex(hit.point, hit.normal);
                aimNormal = hit.normal;

                MyDebugText.instance.SetText(hexAimedAt.x.ToString() + " " + hexAimedAt.y.ToString() + " " + hexAimedAt.z.ToString());
            }
        }
        else
        {
            MyDebugText.instance.SetText("none");
            isHexAimedAt = false;
        }

    }

    public Vector3 GetHexLocal(Vector3 worldPos, Vector3 normal, Transform chunkTransform)
    {
        return GetHex(worldPos - chunkTransform.position, normal);
    }

    public Vector3 GetHex(Vector3 worldPos, Vector3 normal)
    {
        float hexSize = 1.0f;
        float hexW = Mathf.Sqrt(3) * hexSize;
        float hexH = 2 * hexSize;

        Vector3 pos = worldPos - normal * 0.25f;

        float y = Mathf.Floor(pos.y);

        int zp = Mathf.FloorToInt(pos.z / (hexH * 0.75f));
        float z = zp * hexH * 0.75f;
        int xp = Mathf.FloorToInt(pos.x / (hexW * 0.5f));
        float x = xp * hexW * 0.5f;



        Vector3 p1 = new Vector3();
        Vector3 p2 = new Vector3();

        float xs = x + hexW * 0.5f;
        float zs = z + hexH * 0.75f;

        int xx = 0;
        int yy = 0;
        int zz = 0;

        yy = (int)y;

        if (IsEven(zp))
        {
            if (IsEven(xp))
            {
                p1 = new Vector3(x, y, z);
                p2 = new Vector3(xs, y, zs);
                if (Vector3.Distance(pos, p1) < Vector3.Distance(pos, p2))
                {
                    xx = xp / 2;
                    zz = zp;
                }
                else
                {
                    xx = xp / 2;
                    zz = zp + 1;
                }
            }
            else
            {
                p1 = new Vector3(x, y, zs);
                p2 = new Vector3(xs, y, z);
                if (Vector3.Distance(pos, p1) < Vector3.Distance(pos, p2))
                {
                    xx = (xp - 1) / 2;
                    zz = zp + 1;
                }
                else
                {
                    xx = (xp + 1) / 2;
                    zz = zp;
                }
            }
        }
        else
        {
            if (IsEven(xp))
            {
                p1 = new Vector3(x, y, zs);
                p2 = new Vector3(xs, y, z);
                if (Vector3.Distance(pos, p1) < Vector3.Distance(pos, p2))
                {
                    xx = xp / 2;
                    zz = zp + 1;
                }
                else
                {
                    xx = xp / 2;
                    zz = zp;
                }

            }
            else
            {
                p1 = new Vector3(x, y, z);
                p2 = new Vector3(xs, y, zs);
                if (Vector3.Distance(pos, p1) < Vector3.Distance(pos, p2))
                {
                    xx = (xp - 1) / 2;
                    zz = zp;
                }
                else
                {
                    xx = (xp + 1) / 2;
                    zz = zp + 1;
                }
            }
        }

        return new Vector3(xx, yy, zz);
    }

    bool IsEven(int i)
    {
        return (i % 2 == 0);
    }

    

      
}
