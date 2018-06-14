using System.Collections.Generic;

public class SectorObject
{
    public SectorObjects type;
    public float x;
    public float y;
    public float z;

    public SectorObject(SectorObjects type, float x, float z)
    {
        this.type = type;
        this.x = x;
        this.z = z;
    }

    public static bool Occupied(List<SectorObject> list, float x, float z)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].x == x && list[i].z == z)
            {
                return true;
            }
        }
        return false;
    }
}
