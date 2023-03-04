using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {
        UI_Game.Instance.OpenUI<UICChapter>(UIID.UICChapter);
    }
}
