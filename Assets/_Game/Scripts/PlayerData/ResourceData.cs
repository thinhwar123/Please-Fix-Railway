using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ResourceData {
    public string key;
    public int ownedAmount;
    public ResourceData(string key, int defaultValue) {
        this.key = key;
        this.ownedAmount = defaultValue;
    }
    public void Add(int amount) {
        ownedAmount += amount;
        Save();
    }
    public void Consume(int amount) {
        ownedAmount -= amount;
        Save();
    }
    public void Set(int amount) {
        ownedAmount = amount;
        Save();
    }
    public int GetAmount() {
        return ownedAmount;
    }
    public bool IsEnough(int amount) {
        return ownedAmount >= amount;
    }
    public void Save() {
        PlayerPrefs.SetInt(key, ownedAmount);
    }
    public void Load() {
        ownedAmount = PlayerPrefs.GetInt(key, 0);
    }
}