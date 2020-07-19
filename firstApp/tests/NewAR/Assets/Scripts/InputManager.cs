using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject crossHair;

    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private Touch touch;
    private Pose pose;

    // // // Start is called before the first frame update
    void Start()
    {
        // get the comonents
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        crossHair = transform.GetChild(0).gameObject;

        //Hide the placement 
        crossHair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), _hits, TrackableType.Planes);
        if (_hits.Count > 0)
        {
            crossHair.transform.position = _hits[0].pose.position;
            crossHair.transform.rotation = _hits[0].pose.rotation;

            if (!crossHair.activeInHierarchy)
                crossHair.SetActive(true);
        }
        touch = Input.GetTouch(0);

        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began)
            return;
        
        if (IsPointerOverUI(touch))
            return;        
        
        
        Instantiate(DataHandler.Instance.furniture, pose.position, pose.rotation);

    }
    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    // void CrossHairCalculation()
    // {
    //     //Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
    //     //Ray ray = arCam.ScreenPointToRay(origin);

    //     _raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), _hits, TrackableType.Planes);
    //     if (_hits.Count > 0)
    //     {
    //         crossHair.transform.position = _hits[0].pose.position;
    //         crossHair.transform.rotation = _hits[0].pose.rotation;

    //         if (!crossHair.activeInHierarchy)
    //             crossHair.SetActive(true);
    //     }
    //     // if (_raycastManager.Raycast(ray,_hits))
    //     // {
    //     //     Pose pose = _hits[0].pose;
    //     //     crossHair.transform.position = pose.position;
    //     //     crossHair.transform.eulerAngles = new Vector3(90, 0, 0);   
    //     // }     
    // }
}
