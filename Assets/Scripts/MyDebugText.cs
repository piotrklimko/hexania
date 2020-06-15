using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyDebugText : MonoBehaviour
{
    public static MyDebugText instance;

    TextMeshProUGUI tmp;

    private void Awake()
    {
        instance = this;
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string tx)
    {
        tmp.text = tx;
    }


    
}
