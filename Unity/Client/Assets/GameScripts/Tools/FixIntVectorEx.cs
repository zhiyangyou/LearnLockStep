using Fantasy;
using FixMath;

public static class FixIntVectorEx {
    public static CSFixIntVector3 ToCSVector3(this FixIntVector3 v3) {
        var ret = new CSFixIntVector3();
        ret.x = v3.x.IntValue;
        ret.y = v3.y.IntValue;
        ret.z = v3.z.IntValue;
        return ret;
    }

    public static string ToStr(this CSFixIntVector3 v3) {
        return $"CSVector3:({new FixInt(v3.x).ToStringFloat() }, {new FixInt(v3.z).ToStringFloat()}, {new FixInt(v3.z).ToStringFloat()})";
    }

    public static FixIntVector3 ToFixIntVector3(this CSFixIntVector3 v3) {
        var ret = new FixIntVector3 {
            x = new FixInt((long)v3.x),
            y = new FixInt((long)v3.y),
            z = new FixInt((long)v3.z)
        };
        return ret;
    }
}