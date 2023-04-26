// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning disable CS1591
// using System.Device.I2c;
using Iot.Device.Matrix;
using Iot.Device.Graphics;
using System.Device.Gpio;
using static System.Console;
using System.Diagnostics;

// using I2cDevice first = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[0]));
// using I2cDevice second = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[1]));
// using I2cDevice third = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[2]));
// IMatrix matrix = new MicroDotPhat30x7(first, second, third);
BdfFont font = BdfFont.Load(@"../../../iot/src/devices/RGBLedMatrix/samples/fonts/10x20.bdf");
int w = font.Width * 6;
ConsoleMatrix matrix = new(WindowWidth, WindowHeight);
LedMatrix ledMatrix = new LedMatrix(matrix, font);
int width = matrix.Width - 1;
int height = matrix.Height - 1;
int halfWidth = matrix.Width / 2;
int halfHeight = matrix.Height / 2;

// Console.WriteLine("Attach");
// Console.ReadLine();

ledMatrix.ScrollingText("Richard Eric Lander", 50);
// ledMatrix.DrawLetter('R', 60, 0);

Console.WriteLine();

public class ConsoleMatrix : IMatrix
{
    public ConsoleMatrix(int width, int height)
    {
        Width = width;
        Height = height;
    }
    public PinValue this[int x, int y]
    {
        get => PinValue.Low;
        set => SetPixel(x, y, value);
    }

    public int Width {get; init;}

    public int Height {get; init;}

    public void Fill(int value)
    {
    }

    void SetPixel(int x, int y, PinValue value)
    {
        SetCursorPosition(x,y);
        Write(value == PinValue.High ? '*' : ' ');
    }

    // private int GetAddress(int x, int y) => x + (y * Width);
}