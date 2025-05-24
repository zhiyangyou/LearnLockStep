using Fantasy;
using UnityEngine;

public static class CSVectorEx {
    public static CSVector3 ToCSVector3(this Vector3 v3) {
        return new CSVector3() {
            x = v3.x,
            y = v3.y,
            z = v3.z
        };
    }

    public static Vector3 ToVector3(this CSVector3 v3) {
        return new Vector3(v3.x, v3.y, v3.z);
    }
}