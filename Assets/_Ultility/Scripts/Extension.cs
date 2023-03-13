using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace MyExtension
{
    public static class Extension
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
        public static string ToStringReadable(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds < 1) return "0s";
            return string.Format("{0}{1}{2}{3}",
                timeSpan.Days > 0 ? $"{timeSpan.Days}d " : "",
                timeSpan.Hours > 0 ? $"{timeSpan.Hours}h " : "",
                timeSpan.Minutes > 0 ? $"{timeSpan.Minutes}m " : "",
                timeSpan.Seconds > 0 ? $"{timeSpan.Seconds}s " : "");
        }
    } 
}
