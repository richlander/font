using Iot.Device.Matrix;

public record struct ScrollData(Direction Direction = Direction.RightToLeft, int Index = 0, int Count = 0, bool IsComplete = false, bool CanReset = true);

public static class ScrollDataExtensions
{
    public static ScrollData Reverse(this ScrollData data)
    {
        if (!data.CanReset)
        {
            return data;
        }

        if (data.IsComplete || data.Index is 0)
        {
            data.IsComplete = false;
            data.Index = 0;

            if (data.Direction is Direction.LeftToRight)
            {
                data.Direction = Direction.RightToLeft;
            }
            else
            {
                data.Direction = Direction.LeftToRight;
            }
        }

        return data;
    }

    public static ScrollData Count(this ScrollData data, int count)
    {
        if (data.IsComplete && data.Count < count)
        {
            data.Count++;
        }

        return data;
    }

    public static ScrollData Repeat(this ScrollData data, int count = 0)
    {
        if (!data.CanReset)
        {
            return data;
        }

        if (data.IsComplete && count > 0)
        {
            data.Count++;

            if (data.Count >= count)
            {
                data.CanReset = false;
                return data;
            }
        }

        if (data.IsComplete)
        {
            data.Reset();
        }

        return data;
    }

    public static void Reset(this ref ScrollData data)
    {
        if (data.CanReset)
        {
            data.IsComplete = false;
            data.Index = 0;
        }
    }
}