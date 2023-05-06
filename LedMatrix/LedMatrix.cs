// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using Iot.Device.Graphics;

namespace Iot.Device.Matrix
{
    /// <summary>
    /// Provides basic functionality for IMatrix
    /// </summary>
    public class LedMatrix
    {
        private IMatrix _matrix;

        /// <summary>
        /// Initialize LedMatrix.
        /// </summary>
        public LedMatrix(IMatrix matrix)
        {
            _matrix = matrix;
        }

        // Scroll text from right to left
        public ScrollData ScrollText(ReadOnlySpan<char> text, BdfFont font, ScrollData data, int y = 0)
        {
            if (data.IsComplete)
            {
                return data;
            }

            data.IsComplete = ScrollText(text, font, data.Index, data.Direction, y);
            
            if (!data.IsComplete)
            {
                data.Index++;
            }

            return data;
        }

        // Scroll text from right to left
        public bool ScrollText(ReadOnlySpan<char> text, BdfFont font, int index, Direction direction, int y = 0)
        {
            int textWidth = font.Width * text.Length;
            int columns = textWidth + _matrix.Width;
            int cursor = direction is Direction.RightToLeft ? _matrix.Width - index : index - textWidth;
            int negativeWidth = 0 - font.Width;
            int start = 0;

            if (index > columns)
            {
                return true;
            }

            if (direction is Direction.RightToLeft && cursor < negativeWidth)
            {
                start = cursor / negativeWidth;
                cursor += (font.Width * start);
            }
            else if (direction is Direction.LeftToRight && cursor > _matrix.Width)
            {
                int w1 = cursor - _matrix.Width;
                int w2 = negativeWidth - _matrix.Width;
                start = w1 / w2;
                cursor -= (font.Width * start); 
            }

            // bool reverse = direction is Direction.RightToLeft ? false : true;
            DrawText(text.Slice(start), font, false, cursor, y);
            return false;
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawText(ReadOnlySpan<char> value, BdfFont font, bool reverse = false, int x = 0, int y = 0)
        {
            int rollingX = x;
            int negativeWidth = font.Width * -1;

            foreach(char c in value)
            {
                if (rollingX >= _matrix.Width)
                {
                    break;
                }
                
                DrawLetter(c, font, rollingX, y);
                rollingX += reverse ? negativeWidth : font.Width;
            }
        }

        /// <summary>
        /// Draw text on the LED matrix.
        /// </summary>
        public void DrawLetter(char value, BdfFont font, int x = 0, int y = 0)
        {
            int width = _matrix.Width;
            int height = _matrix.Height;

            // Get font data
            font.GetCharData(value, out ReadOnlySpan<ushort> charData);

            // Calculate dimensions
            int firstRowToDraw = y < 0 ? Math.Abs(y) : 0;
            int lastRowToDraw = y + font.Height > height ? height - y : charData.Length;
            int firstColumnToDraw = x < 0 ? Math.Abs(x) : 0;
            int lastColumnToDraw = x + font.Width > width ? width - x : font.Width;

            for (int j = firstRowToDraw; j < lastRowToDraw; j++)
            {
                int bit = font.Width > 8 ? 0x8000 : 0x80;
                ushort glyph = charData[j];

                if (firstColumnToDraw > 0)
                {
                    bit >>= firstColumnToDraw;
                }

                for (int i = firstColumnToDraw; i < lastColumnToDraw; i++)
                {
                    _matrix[x + i, y + j] = (glyph & bit) is 0 ? 0 : 1;
                    bit >>= 1;
                }
            }
        }
    }
}
