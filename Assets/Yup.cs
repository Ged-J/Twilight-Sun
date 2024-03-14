using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yup : MonoBehaviour
{
    private static int once =0;
    // Start is called before the first frame update
    void Start()
    {
        once++;
    }

    // Update is called once per frame
    void Update()
    {
        if (once > 2)
        {
            Destroy(gameObject);
        }
    }
}
