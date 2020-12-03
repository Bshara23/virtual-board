using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Web;
using System.Runtime.InteropServices;
using System;
using System.Net.Sockets;
using System.Net;
public class MLClient : MonoBehaviour
{
    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);

    byte[] buffer = new byte[4096];

    public GameObject spherePrefab;
    private GameObject[] spheres;

    void Start()
    {
        s.Connect(ep);

        spheres = new GameObject[21];
        for (int i = 0; i < 21; i++)
        {
            spheres[i] = Instantiate(spherePrefab, transform);
        }
    }


    void Update()
    {
        try
        {
            //s.Send(Encoding.ASCII.GetBytes("abc"), SocketFlags.None);

            int nbytes = s.Receive(buffer);
            string res = Encoding.UTF8.GetString(buffer, 0, nbytes);
            //ArrayList json = JsonUtility.FromJson<string>(res);

            if (res[0] == '[')
            {
                Hand hand = new Hand(res);

                if (hand.left.Length > 0)
                {
                    for (int i = 0; i < hand.left.Length; i++)
                    {
                        float px = transform.localScale.x * 10 * hand.left[i, 0] - transform.localScale.x * 5;
                        float py = transform.localScale.z * 10 * hand.left[i, 1] - transform.localScale.z * 5;
                        Vector3 pos = new Vector3(px, py, 0);
                        spheres[i].SetActive(true);
                        spheres[i].transform.position = transform.position + pos;
                    }
                    

                }
                else
                {
                    for (int i = 0; i < 21; i++)
                    {
                        spheres[i].SetActive(false);
                    }

                }
            }
            else
            {
                for (int i = 0; i < 21; i++)
                {
                    spheres[i].SetActive(false);
                }

            }

        }
        catch (SocketException e)
        {
            print(e.Message + e.ErrorCode);
        }
        catch (Exception e)
        {
            print(e.Message);
        }
    }
}
