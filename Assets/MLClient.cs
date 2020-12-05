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
    private GameObject[] spheresLeft;
    private GameObject[] spheresRight;
    public GameObject content;
    private Vector3 originalPos;
    private GameObject rightPointer;
    public GameObject rightPointerPrefab;
    private GameObject leftPointer;
    public GameObject leftPointerPrefab;
    public GameObject lockIndicator;
    public float speed = 55;
    public float leng = 10f;
    private float lockTimer = 0;
    private bool gLock = false;
    public float globalScroll = 10f;
    public Vector3 ptrPos;
    public static Hand hand;
    public static Transform mainTransform;
    void Start()
    {
        s.Connect(ep);
        mainTransform = transform;

        spheresLeft = new GameObject[21];
        spheresRight = new GameObject[21];
        for (int i = 0; i < 21; i++)
        {
            spheresLeft[i] = Instantiate(spherePrefab, transform);
            spheresLeft[i].SetActive(false);
            spheresRight[i] = Instantiate(spherePrefab, transform);
            spheresRight[i].SetActive(false);
        }

        rightPointer = Instantiate(rightPointerPrefab, transform);
        leftPointer = Instantiate(leftPointerPrefab, transform);
        rightPointer.SetActive(false);
        leftPointer.SetActive(false);
    }




    void Update()
    {
        try
        {

            int nbytes = s.Receive(buffer);
            string res = Encoding.UTF8.GetString(buffer, 0, nbytes);

            hand = new Hand(res);



            if (hand.hg.right.Count > 0 && hand.hg.left.Count > 0)
            {
                BothHandsHandler(hand);
            }
            if (gLock == false)
            {
                if (hand.hg.right.Count > 0)
                {

                    RightHandHandler(hand);
                    for (int i = 8; i < 9; i++)
                    {
                        spheresLeft[i].SetActive(true);
                    }

                }

                else if (hand.hg.left.Count > 0)
                {

                    LeftHandHandler(hand);
                    for (int i = 0; i < 21; i++)
                    {
                        spheresRight[i].SetActive(true);
                    }

                }
            }


            
            else
            {
                for (int i = 0; i < 21; i++)
                {
                    spheresLeft[i].SetActive(false);
                    spheresRight[i].SetActive(false);
                }
                rightPointer.SetActive(false);
                leftPointer.SetActive(false);

            }

        }
        catch (SocketException e)
        {
            //print(e.Message + e.ErrorCode);
            rightPointer.SetActive(false);
            leftPointer.SetActive(false);
        }
        catch (Exception e)
        {
            //print(e.Message);
            rightPointer.SetActive(false);
            leftPointer.SetActive(false);
        }
    }



    private void BothHandsHandler(Hand hand)
    {
        rightPointer.SetActive(false);
        leftPointer.SetActive(false);

      

        if (gLock == false)
        {

            if (hand.hg.leftGesture == "closed" && hand.hg.rightGesture == "closed")
            {
                DisableGesturesHandler();
                //print("both closed");
            }
            if (hand.hg.leftGesture == "open" && hand.hg.rightGesture == "closed")
            {
                OpacityHandler();
                //print("open-closed");

            }
            else if (hand.hg.leftGesture == "pinch" && hand.hg.rightGesture == "closed")
            {
                RotateObjectHandler();
                //print("pinch-closed");

            }
            else if (hand.hg.leftGesture == "pinch" && hand.hg.rightGesture == "pinch")
            {
                ScaleObjectHandler();
                //print("pinch-pinch");

            }
        }
        else
        {
            if (hand.hg.leftGesture == "open" && hand.hg.rightGesture == "open")
            {
                EnableGestureHandler();
            }
        }
        

    }


    private void ScaleObjectHandler()
    {
        throw new NotImplementedException();
    }

    private void RotateObjectHandler()
    {
        throw new NotImplementedException();
    }

    private void OpacityHandler()
    {
        throw new NotImplementedException();
    }

    private void EnableGestureHandler()
    {
        gLock = false;
        lockIndicator.SetActive(false);
    }
    private void DisableGesturesHandler()
    {
        gLock = true;
        lockIndicator.SetActive(true);

    }

    private void RightHandHandler(Hand hand)
    {
        for (int i = 0; i < 21; i++)
        {
            float px = transform.localScale.x * 10 * hand.hg.right[i].x - transform.localScale.x * 5;
            float py = transform.localScale.z * 10 * hand.hg.right[i].y - transform.localScale.z * 5;
            Vector3 pos = new Vector3(px, py, 0);
            spheresRight[i].SetActive(true);
            spheresRight[i].transform.position = transform.position + pos;
        }

        if (hand.hg.rightGesture == "pinch")
        {
            //print("right pinch");
            rightPointer.SetActive(false);
        }
        else if (hand.hg.rightGesture == "open")
        {
            //print("right open");
            rightPointer.SetActive(false);

        }
        else if (hand.hg.rightGesture == "closed")
        {
            //print("right closed");
            content.transform.position = content.transform.position + RightPointMapped(hand, 8) * Time.deltaTime * globalScroll;
            rightPointer.SetActive(false);

        }
        else if (hand.hg.rightGesture == "point")
        {
            //print("right point");
            rightPointer.SetActive(true);
            leftPointer.SetActive(false);
            rightPointer.transform.position = transform.position + RightPointMapped(hand, 8) + ptrPos;




        }
    }

    private void LeftHandHandler(Hand hand)
    {
        for (int i = 8; i < 9; i++)
        {
            float px = transform.localScale.x * 10 * hand.hg.left[i].x - transform.localScale.x * 5;
            float py = transform.localScale.z * 10 * hand.hg.left[i].y - transform.localScale.z * 5;
            Vector3 pos = new Vector3(px, py, 0);
            spheresLeft[i].SetActive(true);
            spheresLeft[i].transform.position = transform.position + pos;
        }

        if (hand.hg.leftGesture == "pinch")
        {
            //print("left pinch");
            leftPointer.SetActive(false);

        }
        else if (hand.hg.leftGesture == "open")
        {
            //print("left open");
            leftPointer.SetActive(false);

        }
        else if (hand.hg.leftGesture == "closed")
        {
            //print("left closed");
            leftPointer.SetActive(false);


        }
        else if (hand.hg.leftGesture == "point")
        {
            //print("left point");
            leftPointer.SetActive(true);
            rightPointer.SetActive(false);

            leftPointer.transform.position = transform.position + LeftPointMapped(hand, 8) + ptrPos;


            var ints = Instantiate(rightPointer, leftPointer.transform.position, Quaternion.identity, content.transform.parent.transform);
            ints.SetActive(true);

            Destroy(ints, 2f);

        }

    }

    private Vector3 LeftPointMapped(Hand hand, int i)
    {
        float px = transform.localScale.x * 10 * hand.hg.left[i].x - transform.localScale.x * 5;
        float py = transform.localScale.z * 10 * hand.hg.left[i].y - transform.localScale.z * 5;
        Vector3 pos = new Vector3(px, py, 0);
        return pos;
    }

    private Vector3 RightPointMapped(Hand hand, int i)
    {
        float px = transform.localScale.x * 10 * hand.hg.right[i].x - transform.localScale.x * 5;
        float py = transform.localScale.z * 10 * hand.hg.right[i].y - transform.localScale.z * 5;
        Vector3 pos = new Vector3(px, py, 0);
        return pos;
    }
}
