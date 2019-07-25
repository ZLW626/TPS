using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Generate01Map : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 direction;
    private Ray shootRay;
    private RaycastHit hitPoint;
    private int shootableMask;

    // Start is called before the first frame update
    void Start()
    {
        origin.x = 0.5f;
        origin.y = -10.0f;
        origin.z = 0.5f;

        direction = new Vector3(0.0f, 1.0f, 0.0f);
        shootRay.direction = direction;

        shootableMask = LayerMask.GetMask("Shootable");

        StreamWriter sw = new StreamWriter("01Map.txt");

        int count = 0;
        for (int i = 0;i < 100;++i)
        {
            for(int j = 0;j < 100;++j)
            {
                if(i == 0 || j == 0 || i == 99 || j == 99)
                {
                    sw.Write('1');
                    sw.Write(' ');
                    sw.Write(' ');
                    count++;
                    continue;
                }
                shootRay.origin = origin + new Vector3(i * 1f, 0f, j * 1f);

                if (Physics.Raycast(shootRay, out hitPoint, 30, shootableMask))
                {
                    sw.Write('1');
                    count++;
                }
                else
                {
                    sw.Write('0');
                    count++;
                }
                sw.Write(' ');
                sw.Write(' ');
                
            }
            sw.Write('\n');
        }

        Debug.Log("completed!" + count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
