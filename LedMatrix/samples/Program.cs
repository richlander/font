// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning disable CS1591
// using System.Device.I2c;
using Iot.Device.Matrix;
using Iot.Device.Graphics;

// using I2cDevice first = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[0]));
// using I2cDevice second = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[1]));
// using I2cDevice third = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[2]));
// IMatrix matrix = new MicroDotPhat30x7(first, second, third);
ConsoleMatrix matrix = new();
BdfFont font = BdfFont.Load(@"../../../iot/src/devices/RGBLedMatrix/samples/fonts/5x7.bdf");
LedMatrix ledMatrix = new LedMatrix(matrix, font);
int width = matrix.Width - 1;
int height = matrix.Height - 1;
int halfWidth = matrix.Width / 2;
int halfHeight = matrix.Height / 2;

ledMatrix.DrawLetter('h', 2, 1);
matrix.Print();
Console.WriteLine();

// font.GetCharData('g', out ReadOnlySpan<ushort> charData);




// Console.WriteLine(font.Width);
// foreach (ushort s in charData)
// {
//     PrintAsBinary(s);
//     Console.WriteLine();
// }

// void PrintAsBinary(ushort value)
// {
    //     int bit = font.Width > 8 ? 0x8000 : 0x80;
    //     for (int i = 0; i < font.Width; i++)
    //     {
    //         char val = (value & bit) > 0 ? '*' : '.';
    //         Console.Write(val);
    //         bit = bit >> 1;
    //     }
// }

// void PrintAsBinary2(ushort value)
// {
//     int bit = 0x8000;
//     for (int i = 0; i < font.Width; i++)
//     {
//         int val = value << (8 + i);
//         char c = (val & bit) > 0 ? '*' : '.';
//         Console.Write(c);
//     }
// }

public class ConsoleMatrix : IMatrix
{
    private int[] _data = new int[64];
    public int this[int x, int y]
    {
        get => _data[GetAddress(x, y)];
        set => _data[GetAddress(x, y)] = value;
    }

    public int Width => 8;

    public int Height => 8;

    public void Fill(int value)
    {
        _data.AsSpan().Fill(1);
    }

    public void Print()
    {
        foreach (int index in Enumerable.Range(0, 64))
        {
            int offset = index % Width;

            if (offset is 0)
            {
                Console.WriteLine();
            }

            char c = _data[index] is 0 ? '.' : '*';
            Console.Write(c);
        }
    }

    private int GetAddress(int x, int y) => x + (y * Width);
}