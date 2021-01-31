using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static uint screenWidth = 640;
        public static uint screenHeight = 480;
        public static DoubleBufferedVMWareSVGAII vMWareSVGAII;
        //Bitmap bitmap; Wallpaper
        public static Bitmap programlogo;
        Bitmap bootBitmap;

        int[] cursor = new int[]
            {
                1,0,0,0,0,0,0,0,0,0,0,0,
                1,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,1,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,
                1,2,2,2,2,2,1,0,0,0,0,0,
                1,2,2,2,2,2,2,1,0,0,0,0,
                1,2,2,2,2,2,2,2,1,0,0,0,
                1,2,2,2,2,2,2,2,2,1,0,0,
                1,2,2,2,2,2,2,2,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,1,1,1,1,1,
                1,2,2,2,1,2,2,1,0,0,0,0,
                1,2,2,1,0,1,2,2,1,0,0,0,
                1,2,1,0,0,1,2,2,1,0,0,0,
                1,1,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,0,1,1,0,0,0
            };

        Console console;
        Dock dock;
        public static bool Pressed;

        public static List<App> apps = new List<App>();

        public static Color avgCol;

        protected override void BeforeRun()
        {
            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            bootBitmap = new Bitmap(@"0:\boot.bmp");

            vMWareSVGAII = new DoubleBufferedVMWareSVGAII();
            vMWareSVGAII.SetMode(screenWidth, screenHeight);

            vMWareSVGAII.DoubleBuffer_DrawImage(bootBitmap, screenWidth / 2 - bootBitmap.Width / 2, screenHeight / 2 - bootBitmap.Height / 2);
            vMWareSVGAII.DoubleBuffer_Update();

            //bitmap = new Bitmap(@"0:\timg.bmp"); Wallpaper

            programlogo = new Bitmap(@"0:\program.bmp");

            /*
            uint r = 0;
            uint g = 0;
            uint b = 0;
            for (uint i = 0; i < bitmap.rawData.Length; i++)
            {
                Color color = Color.FromArgb(bitmap.rawData[i]);
                r += color.R;
                g += color.G;
                b += color.B;
            }
            avgCol = Color.FromArgb((int)(r / bitmap.rawData.Length), (int)(g / bitmap.rawData.Length), (int)(b / bitmap.rawData.Length));
            */
            avgCol = Color.DimGray;

            MouseManager.ScreenWidth = screenWidth;
            MouseManager.ScreenHeight = screenHeight;

            console = new Console(400, 300, 40, 40);
            dock = new Dock();

            apps.Add(console);
        }

        protected override void Run()
        {
            switch (MouseManager.MouseState)
            {
                case MouseState.Left:
                    Pressed = true;
                    break;
                case MouseState.None:
                    Pressed = false;
                    break;
            }

            vMWareSVGAII.DoubleBuffer_Clear((uint)Color.Black.ToArgb());

            vMWareSVGAII.DoubleBuffer_DrawImage(bootBitmap, screenWidth / 2 - bootBitmap.Width / 2, screenHeight / 2 - bootBitmap.Height / 2);


            //vMWareSVGAII.DoubleBuffer_DrawImage(bitmap,0,0); Wallpaper

            foreach (App app in apps)
                app.Update();

            dock.Update();

            DrawCursor(vMWareSVGAII, MouseManager.X, MouseManager.Y);

            vMWareSVGAII.DoubleBuffer_Update();
        }

        public void DrawCursor(DoubleBufferedVMWareSVGAII vMWareSVGAII, uint x, uint y)
        {
            for (uint h = 0; h < 19; h++)
            {
                for (uint w = 0; w < 12; w++)
                {
                    if (cursor[h * 12 + w] == 1)
                    {
                        vMWareSVGAII.DoubleBuffer_SetPixel(w + x, h + y, (uint)Color.Black.ToArgb());
                    }
                    if (cursor[h * 12 + w] == 2)
                    {
                        vMWareSVGAII.DoubleBuffer_SetPixel(w + x, h + y, (uint)Color.White.ToArgb());
                    }
                }
            }
        }
    }
}
