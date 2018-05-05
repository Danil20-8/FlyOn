using System;


public static class ConcurrentRandomProvider
{
    private static Random random = new Random();

    public static Random GetRandom()
    {
        lock (random)
        {
            return new Random(random.Next());
        }
    }
}
