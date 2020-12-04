using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardObjectTrigger : MonoBehaviour
{
    #region Private Props
    bool runOnce = false;

    public float ZPos;
    Vector3 Offset;
    bool Dragging;
    #endregion

    #region Inspector Variables
    public Camera MainCamera;
    [Space]
    [SerializeField]
    public UnityEvent OnBeginDrag;
    [SerializeField]
    public UnityEvent OnEndDrag;
    #endregion
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
        MainCamera = Camera.main;
        ZPos = MainCamera.WorldToScreenPoint(transform.position).z;
    }
    private void OnTriggerStay(Collider other)
    {
        if (MLClient.hand == null) return;

        string rg = MLClient.hand.hg.rightGesture;
        if (rg == "pinch")
        {
            if (!Dragging)
                BeginDrag();
        }
        else
        {
            if (rg == "open")
            {
                EndDrag();
            }
        }
    }
    void Update()
    {
        if (Dragging)
        {

            float px = MLClient.mainTransform.localScale.x * 10 * MLClient.hand.hg.right[8].x - MLClient.mainTransform.localScale.x * 5;
            float py = MLClient.mainTransform.localScale.z * 10 * MLClient.hand.hg.right[8].y - MLClient.mainTransform.localScale.z * 5;
            
            Vector3 pos = new Vector3(px, py, transform.position.z);
            transform.position = Offset + pos;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        runOnce = true;

    }

    private void EndDrag()
    {
        OnEndDrag.Invoke();
        Dragging = false;
    }

    public void BeginDrag()
    {
        OnBeginDrag.Invoke();
        Dragging = true;

        if (MLClient.hand.hg.rightGesture.Length > 0)
        {

            float px = MLClient.mainTransform.localScale.x * 10 * MLClient.hand.hg.right[8].x - MLClient.mainTransform.localScale.x * 5;
            float py = MLClient.mainTransform.localScale.z * 10 * MLClient.hand.hg.right[8].y - MLClient.mainTransform.localScale.z * 5;

            Offset = transform.position - new Vector3(px, py, 0);

        }
    }
}
