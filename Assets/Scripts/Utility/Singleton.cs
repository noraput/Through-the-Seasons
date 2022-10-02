using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance;

    protected virtual void Awake() {
        if (instance != null && instance != this) {
            // Debug.Log("There is already an object of type " + GetType().Name);
            Destroy(this.gameObject);
            return;
        }

        instance = this as T;
    }
}

public abstract class PersistentObject<T> : Singleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
}
