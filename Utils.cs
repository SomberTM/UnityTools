using UnityEngine;

public class Utils
{

    public static Vector3 GetEmptyVector3() { return new Vector3(0.0f, 0.0f, 0.0f); }
    public static Vector3 ScaleVector3(Vector3 scaling, Vector3 scaler) {
        scaling.x *= scaler.x;
        scaling.y *= scaler.y;
        scaling.z *= scaler.z;
        return scaling;
    }

    public static bool IsVector3Empty(Vector3 test) {
        return (test.x == 0 && test.y == 0 && test.z == 0);
    }

    public static bool TryGetComponentInChild<T>(GameObject parent, out T component) where T : Component {
        T target = parent.GetComponentInChildren<T>();
        component = target;
        if (target != null)
            return true;
        else 
            return false;
    }

    public delegate T Function<T>(T x);
    public static Bounds AbsoluteBounds(Bounds bounds) {
        Function<float> abs = Mathf.Abs;
        Vector3 size = bounds.size;
        Bounds AbsoluteBounds = new Bounds(bounds.center, size);
        AbsoluteBounds.size = new Vector3(abs(size.x), abs(size.y), abs(size.z));
        return AbsoluteBounds;
    }

    public static Vector3 CopyVector3(Vector3 vec) {
        return new Vector3(vec.x, vec.y, vec.z);
    }

    public static Camera ExtractCamera<T>(T Query) where T : MonoBehaviour {
        Camera camera;
        GameObject gameObject = Query.gameObject;
        if (gameObject.TryGetComponent<Camera>(out camera) || TryGetComponentInChild<Camera>(gameObject, out camera))
            return camera;
        return null;
    }

    public static Camera ExtractCamera(GameObject Query) {
        Camera camera;
        if (Query.TryGetComponent<Camera>(out camera) || TryGetComponentInChild<Camera>(Query, out camera))
            return camera;
        return null;
    }

    public static bool ExtractCamera(GameObject Query, out Camera QueryCamera) {
        Camera camera;
        QueryCamera = null;
        if (Query.TryGetComponent<Camera>(out camera) || TryGetComponentInChild<Camera>(Query, out camera)) {
            QueryCamera = camera;
            return true;
        }
        return false;
    }

    public static void Swap<A>(A[] array, int index0, int index1) {
        Utils.Swap(array, array, index0, index1);
    }

    public static void Swap<A>(A[] array1, A[] array2, int index0, int index1) {
        A temp = array1[index0];
        array1[index0] = array2[index1];
        array2[index1] = temp;
    }

}
