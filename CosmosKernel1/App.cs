using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    public class App
    {
        public readonly uint baseWidth;
        public readonly uint baseHeight;
        public readonly uint width;
        public readonly uint height;

        public uint dockX;
        public uint dockY;
        public uint dockWidth = 40;
        public uint dockHeight = 30;

        public uint baseX;
        public uint baseY;
        public uint x;
        public uint y;
        public string name;

        int px;
        int py;
        bool lck = false;

        bool pressed;
        public bool visible = false;

        const int MoveBarHeight = 22;

        public int _i = 0;

        public App(uint width, uint height, uint x = 0, uint y = 0)
        {
            this.baseWidth = width;
            this.baseHeight = height;
            this.baseX = x;
            this.baseY = y;

            this.x = x + 2;
            this.y = y + MoveBarHeight;
            this.width = width - 4;
            this.height = height - MoveBarHeight - 1;
        }

        public void Update()
        {
            if (_i != 0)
            {
                _i--;
            }

            if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
            {
                Kernel.vMWareSVGAII._DrawACSIIString(name, (uint)Color.White.ToArgb(), (uint)(dockX - ((name.Length * 8) / 2) + dockWidth / 2), dockY - 20);
            }

            if (MouseManager.MouseState == MouseState.Left && _i == 0)
            {
                if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
                {
                    visible = !visible;
                    _i = 60;
                }
            }

            if (Kernel.Pressed)
            {
                if (MouseManager.X > baseX && MouseManager.X < baseX + baseWidth && MouseManager.Y > baseY && MouseManager.Y < baseY + MoveBarHeight)
                {
                    this.pressed = true;
                    if (!lck)
                    {
                        px = (int)((int)MouseManager.X - this.baseX);
                        py = (int)((int)MouseManager.Y - this.baseY);
                        lck = true;
                    }
                }
            }
            else
            {
                this.pressed = false;
                lck = false;
            }

            if (!visible)
                goto end;

            if (this.pressed)
            {
                this.baseX = (uint)(MouseManager.X - px);
                this.baseY = (uint)(MouseManager.Y - py);

                this.x = (uint)(MouseManager.X - px + 2);
                this.y = (uint)(MouseManager.Y - py + MoveBarHeight);
            }

            /*
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x, _y, _width, _height, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 1, _y + 1, _width - 2, 20, (uint)Color.FromArgb(0, 0, 135).ToArgb());
            */
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(baseX, baseY, baseWidth, baseHeight, (uint)Color.White.ToArgb());
            Kernel.vMWareSVGAII.DoubleBuffer_DrawRectangle((uint)Kernel.avgCol.ToArgb(), (int)baseX, (int)baseY, (int)baseWidth, (int)baseHeight);

            Kernel.vMWareSVGAII._DrawACSIIString(name, (uint)Color.Black.ToArgb(), baseX + 2, baseY + 2);
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 22, _y, 1, 22, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(x, y, 20, 20, (uint)Color.Gray.ToArgb());
            _Update();

        end:;
        }

        public virtual void _Update()
        {
        }
    }
}
