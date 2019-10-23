using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour <T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T) FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private bool IsDontDestroy = true;

    virtual protected void Awake()
    {
        if (!IsDontDestroy)
        {
            return;
        }

        // 他のGameObjectにアタッチされているか調べる.
        // アタッチされている場合は破棄する.
        if (this != Instance)
        {
            Destroy(gameObject);

            return;
        }

        // なんとかManager的なSceneを跨いでこのGameObjectを有効にしたい場合は
        // ↓コメントアウト外してください.
        DontDestroyOnLoad(this.gameObject);
    }
}