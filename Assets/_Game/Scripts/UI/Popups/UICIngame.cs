using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICIngame : UICanvas {
    private static UICIngame m_Instance;
    public static UICIngame Instance {
        get {
            return m_Instance;
        }
    }
    public UIButtonInput m_UIButtonInput;
    private void Awake() {
        m_Instance = this;
    }


}