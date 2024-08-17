using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UiManager
{

    private static UiManager instance = null;

    private UiManager() {

    }

    public static UiManager Instance {
        get {
            if (instance == null)
            { instance = new UiManager();
            }
            return instance; }
    }
}