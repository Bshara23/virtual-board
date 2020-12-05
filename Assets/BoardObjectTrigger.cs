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

    private Vector3 offset;
    bool isDragging;
    bool isScaling;
    private Vector3 originalPos;
    #endregion
    private Vector3 collPos;
    private float orignalDistance;
    private float currentScale;
    private Vector3 originalScale;
    [SerializeField]
    public UnityEvent OnBeginDrag;
    [SerializeField]
    public UnityEvent OnEndDrag;
    [SerializeField]
    public UnityEvent OnBeginScale;
    [SerializeField]
    public UnityEvent OnEndScale;
    public float scaleFactor = 1f;

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
        originalScale = transform.localScale;
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
            OnBeginScale.Invoke();

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

            //transform.position = new Vector3(collPos.x, collPos.y, originalPos.z) + offset
            //transform.position =  offset + new Vector3(collPos.x, collPos.y, originalPos.z); 
        }

        else if (isScaling)
        {
            if (MLClient.hand.hg.right.Count > 0 && MLClient.hand.hg.left.Count > 0)
            {
                float x1 = MLClient.hand.hg.left[8].x;
                float y1 = MLClient.hand.hg.left[8].y;
                float x2 = MLClient.hand.hg.right[8].x;
                float y2 = MLClient.hand.hg.right[8].y;

                currentScale = Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));

                //print(currentScale);

                currentScale = Mathf.Clamp(currentScale, 0.1f, 0.3f) * 5f;

                transform.localScale = originalScale * currentScale * scaleFactor;
                //var toScale = transform.localScale * (currentScale / orignalDistance) * Time.deltaTime * 5f;
                //transform.localScale = toScale.magnitude < 2 ? transform.localScale : toScale;
                transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EndDrag();
    }

    private void EndDrag()
    {
        OnEndDrag.Invoke();
        isDragging = false;
    }

    private void EndScaling()
    {
        OnEndScale.Invoke();
        isScaling = false;
    }

    public void BeginDrag()
    {
        OnBeginDrag.Invoke();
        lastObject = gameObject.GetInstanceID();

        isDragging = true;
        //offset = transform.position - new Vector3(collPos.x, collPos.y, originalPos.z);
        //offset = transform.position - new Vector3(collPos.x, collPos.y, originalPos.z);
        //offset.z = 0;

    }
}
