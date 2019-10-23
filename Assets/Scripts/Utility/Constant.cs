﻿using UnityEngine;

internal static class Constant {
    public static readonly int MAX_LEVEL = 3;
    public static readonly int CITIZEN_COUNT = 49;
    public static readonly float MIN_X = 1f;
    public static readonly float MAX_X = 67f;
    public static readonly float MIN_Z = -49f;
    public static readonly float MAX_Z = 19f;
    public static readonly Vector3 DEFAULT_CAM_POS = new Vector3 (34.1f, 50f, -19.4f);
    public static readonly Vector3 PLAY_CAM_POS = new Vector3 (34.1f, 10.3f, -19.4f);
    public static readonly Vector3 CRIMINAL_POS = new Vector3 (35.23f, 48.7f, -16.4f);
    public static readonly Vector3 SHOT_CAM_POS = new Vector3 (-0.08f, 1f, 0.06f);
    public static readonly int[] TIME_LIMIT_ARRAY = new int[3]{
        30,
        25,
        20
    };
    public static readonly int[] MAKE_CITIZEN_COUNT = new int[3]{
        10,
        25,
        40
    };
}