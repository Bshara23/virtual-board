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
using Newtonsoft.Json;

public class MLClient : MonoBehaviour
{
    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);

    byte[] buffer = new byte[4096];

    public GameObject spherePrefab;
    private GameObject[] spheres;
    public Transform note;
    private Vector3 originalPos;
    public float speed = 55;
    public float leng = 10f;
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

            int nbytes = s.Receive(buffer);
            string res = Encoding.UTF8.GetString(buffer, 0, nbytes);

            Hand hand = new Hand(res);


            print(hand.hg.right.Count);
            if (hand.hg.right.Count > 0)
            {

                for (int i = 8; i < 9; i++)
                {
                    float px = transform.localScale.x * 10 * hand.hg.right[i].x - transform.localScale.x * 5;
                    float py = transform.localScale.z * 10 * hand.hg.right[i].y - transform.localScale.z * 5;
                    Vector3 pos = new Vector3(px, py, 0);
                    spheres[i].SetActive(true);
                    spheres[i].transform.position = transform.position + pos;
                }

                if (hand.hg.rightGesture == "pinch")
                {
                    print("pinch");
                    note.localScale = new Vector3(-5, 5, 0);
                }
                else if (hand.hg.rightGesture == "open")
                {
                    print("open");
                    note.localScale = new Vector3(-3, 3, 0);
                }
                else if (hand.hg.rightGesture == "closed")
                {
                    print("closed");
                    note.localScale = new Vector3(-5, 5, 0);

                    note.position = new Vector3((float)Math.Sin(Time.time) * leng + originalPos.x + transform.position.x, originalPos.y + transform.position.y, originalPos.z + transform.position.z);
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
