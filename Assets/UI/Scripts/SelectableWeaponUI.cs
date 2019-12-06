﻿using UnityEngine;
using System.Collections;

public class SelectableWeaponUI : WeaponUI {
    [SerializeField] GameObject SelectionMarker;
    public void SetSelection(bool b) {
        SelectionMarker.SetActive(b && item != null);
    }
}
