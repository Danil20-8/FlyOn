using UnityEngine;


namespace Assets.Other
{
    public static class MyVector
    {
        public static Vector3 RandomAxis3()
        {
            Vector3 result = new Vector3(Random.value, Random.value, Random.value);
            result.Normalize();
            return result;
        }
        public static Vector2 RandomAxis2()
        {
            Vector2 result = new Vector2(Random.value, Random.value);
            result.Normalize();
            return result;
        }
    }
}
