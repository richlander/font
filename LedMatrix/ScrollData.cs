using Iot.Device.Matrix;

public record struct ScrollData(Direction Direction = Direction.RightToLeft, int Index = 0, int Count = 0, bool IsCompleted = false);

public static class ScrollDataExtensions
{
    public static ref ScrollData Reverse(this ref ScrollData data)
    {
        if (data.Index is 0)
        {
            if (data.Direction is Direction.LeftToRight)
            {
                data.Direction = Direction.RightToLeft;
            }
            else
            {
                data.Direction = Direction.LeftToRight;
            }
        }

        return ref data;
    }

    public static ref ScrollData Repeat(this ref ScrollData data, int count = 0)
    {
        if (data.Index is 0 && count > 0)
        {
            data.Count++;

            if (data.Count >= count)
            {
                data.IsCompleted = true;
            }
        }

        return ref data;
    }
}
