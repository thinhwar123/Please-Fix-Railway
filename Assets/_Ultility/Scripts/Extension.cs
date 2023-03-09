using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyExtension
{
    public class Extension
    {
        public static bool m_IsTouchingUI;
        public static bool IsPointerOverUIGameObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    m_IsTouchingUI = true;
                    return true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && m_IsTouchingUI)
            {
                m_IsTouchingUI = false;
                return true;
            }

            return m_IsTouchingUI;
        }
    } 
}
