// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning disable CS1591
// using System.Device.I2c;
using Iot.Device.Matrix;
using Iot.Device.Graphics;
using System.Device.Gpio;
using static System.Console;
using System.Diagnostics;
using Spectre.Console;

// using I2cDevice first = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[0]));
// using I2cDevice second = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[1]));
// using I2cDevice third = I2cDevice.Create(new I2cConnectionSettings(busId: 1, MicroDotPhat30x7.I2cAddresses[2]));
// IMatrix matrix = new MicroDotPhat30x7(first, second, third);
BdfFont font = BdfFont.Load(@"../../../iot/src/devices/RGBLedMatrix/samples/fonts/10x20.bdf");
BdfFont smallFont = BdfFont.Load(@"../../../iot/src/devices/RGBLedMatrix/samples/fonts/4x6.bdf");
int width = WindowWidth;
int height = WindowHeight;
Canvas canvas = new Canvas(width, height);
canvas.PixelWidth = 1;
AnsiConsole.Background = Color.Black;
AnsiConsoleMatrix matrix = new(width, height, canvas);
matrix.Fill(0);
AnsiConsole.Write(canvas);
LedMatrix ledMatrix = new LedMatrix(matrix);

// WriteLine(height);

Console.WriteLine("Attach");
Console.ReadLine();

// ledMatrix.DrawLetter('A', font, -3, -6);
// ledMatrix.DrawLetter('A', font, width - 8, height - 12);
// ledMatrix.DrawLetter('A', font, width - 8, -6);
// ledMatrix.DrawLetter('A', font,-3, height - 12);


AnsiConsole.Write(canvas);

string textOne = "dotnet";
string textTwo = "Hello .NET ... scrolling!";
ScrollData lineOne = new(Direction.LeftToRight);
ScrollData lineTwo = new(Direction.RightToLeft);

while (!lineOne.IsCompleted)
{
    if (lineTwo.Index % 3 is not 0)
    {
        ledMatrix.ScrollText(textOne, font, ref lineOne, 3).Repeat(2).Reverse();
    }

    ledMatrix.ScrollText(textTwo, smallFont, ref lineTwo, 2).Repeat();
    AnsiConsole.Write(canvas);
    Thread.Sleep(45);
}

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
}

public class AnsiConsoleMatrix : IMatrix
{
    Canvas _canvas;
    public AnsiConsoleMatrix(int width, int height, Canvas canvas)
    {
        Width = width;
        Height = height;
        _canvas = canvas;
        _canvas.PixelWidth = 1;
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
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                SetPixel(w, h, value);
            }
        }
    }

    void SetPixel(int x, int y, PinValue value)
    {
        _canvas.SetPixel(x, y, value == PinValue.Low ? Color.Black : Color.White);
    }
}
