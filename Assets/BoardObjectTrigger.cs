using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardObjectTrigger : MonoBehaviour
{
    #region Private Props
    bool runOnce = false;

    public static int lastObject;

    public Vector3 offset;
    bool isDragging;
    bool isScaling;
    private Vector3 originalPos;
    #endregion
    private Vector3 collPos;
    private float orignalDistance;
    private float currentScale;

    private void OnTriggerEnter(Collider other)
    {
        if (!runOnce)
        {
            string tag = "HandKeyPoint";
            print(other.tag);
            runOnce = !runOnce;
        }
    }
  

    void Start()
    {
        originalPos = transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (MLClient.hand == null) return;

        string rg = MLClient.hand.hg.rightGesture;
        if (rg == "pinch")
        {
            string rl = MLClient.hand.hg.leftGesture;
            collPos = other.transform.position;

            if (rl == "pinch")
            {
                BeginScaling();
            }

            else if (!isDragging)
            {
                BeginDrag();
                EndScaling();
            }
        }
        else if (rg == "open")
        {
            EndDrag();
            EndScaling();
        }
        else
        {
            EndScaling();
        }
        
    }

    private void BeginScaling()
    {
      

        
        if (!isScaling)
        {
            float x1 = MLClient.hand.hg.left[8].x;
            float y1 = MLClient.hand.hg.left[8].y;
            float x2 = MLClient.hand.hg.right[8].x;
            float y2 = MLClient.hand.hg.right[8].y;

            currentScale = Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));
            orignalDistance = currentScale;
            isScaling = true;
        }
    }

    void Update()
    {
        if (MLClient.hand == null) return;

        if (isDragging && lastObject == gameObject.GetInstanceID())
        {
            transform.position =  new Vector3(collPos.x, collPos.y, originalPos.z); 
            //transform.position =  offset + new Vector3(collPos.x, collPos.y, originalPos.z); 
        }

        else if (isScaling)
        {
            float x1 = MLClient.hand.hg.left[8].x;
            float y1 = MLClient.hand.hg.left[8].y;
            float x2 = MLClient.hand.hg.right[8].x;
            float y2 = MLClient.hand.hg.right[8].y;

            currentScale = Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));
            transform.localScale *= (currentScale - orignalDistance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EndDrag();
    }

    private void EndDrag()
    {
        isDragging = false;
    }

    private void EndScaling()
    {
        isScaling = false;
    }

    public void BeginDrag()
    {
        lastObject = gameObject.GetInstanceID();

        isDragging = true;
        //offset = transform.position - new Vector3(collPos.x, collPos.y, originalPos.z);
    }
}
