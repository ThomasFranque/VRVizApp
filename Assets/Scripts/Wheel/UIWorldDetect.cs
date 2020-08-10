using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWorldDetect : MonoBehaviour
{

    private static UIWorldDetect _instance;
    public static UIWorldDetect Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIWorldDetect>();
            }
            return _instance;
        }
    }

    private PointerEventData pointerData;
    private GameObject RootElementUI;

    //returns true if a ui elemend was clicked
    public bool isUIElementRaycasted(string gameObjectName)
    {
        if (RootElementUI == null)
            return false;
        else
        {
            if (gameObjectName == RootElementUI.name)
            {
                RootElementUI = null;
                return true;
            }
            else return false;
        }
    }

    void Update()
    {
        RaycastWorldUI();
    }

    void RaycastWorldUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if (results.Count > 0)
            {

                if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI_1"))
                {

                    string dbg = "Root Element: {0} \n GrandChild Element: {1}";

                    RootElementUI = results[results.Count - 1].gameObject;

                    Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                    ExecuteEvents.Execute(results[0].gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                    //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                    results.Clear();
                }
            }
        }
    }
}