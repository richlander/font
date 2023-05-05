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
int w = font.Width * 6;
int width = WindowWidth;
int height = WindowHeight;
Canvas canvas = new Canvas(width, height);
canvas.PixelWidth = 1;
AnsiConsole.Background = Color.Black;
AnsiConsoleMatrix matrix = new(width, height, canvas);
matrix.Fill(0);
AnsiConsole.Write(canvas);
LedMatrix ledMatrix = new LedMatrix(matrix);
int halfWidth = matrix.Width / 2;
int halfHeight = matrix.Height / 2;

// WriteLine(height);

// Console.WriteLine("Attach");
// Console.ReadLine();


bool scrolling = true;
string text = "NHello .NET ... I'm having a scrolling time!";
string text2 = "dotnet";
int columns = 0;
while (scrolling)
{

    scrolling = ledMatrix.ScrollText(text2, font, Direction.LeftToRight, columns, 3);
    scrolling = ledMatrix.ScrollText(text, smallFont, Direction.RightToLeft, columns, 2);
    AnsiConsole.Write(canvas);
    columns++;
    Thread.Sleep(50);
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

    // private int GetAddress(int x, int y) => x + (y * Width);
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
