using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    class Console : App
    {
        int textEachLine;
        public string text = string.Empty;
        public string _text = string.Empty;
        int Lines { get => _text.Split('\n').Length; }

        public Console(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            //ASC16 = 16*8
            textEachLine = (int)width / 8;
            name = "Console";
        }

        public override void _Update()
        {
            KeyEvent keyEvent;
            if (KeyboardManager.TryReadKey(out keyEvent))
            {
                switch (keyEvent.Key)
                {
                    case ConsoleKeyEx.Enter:
                        this.text += "\n";
                        break;
                    case ConsoleKeyEx.Backspace:
                        if (this.text.Length != 0)
                        {
                            this.text = this.text.Remove(this.text.Length - 1);
                        }
                        break;
                    default:
                        this.text += keyEvent.KeyChar;
                        break;
                }
            }

            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(x, y, width, height, (uint)Color.Black.ToArgb());

            if (text.Length != 0)
            {
                _text = string.Empty;
                int i = 0;
                int l = 0;
                foreach (char c in text)
                {
                    if (this.height / ASC16.fontSize.Height - 1 < l)
                    {
                        break;
                    }

                    _text += c;
                    i++;
                    if (i + 1 == textEachLine || c == '\n')
                    {
                        if (c != '\n')
                        {
                            _text += "\n";
                        }
                        i = 0;
                        l++;
                    }
                }
            }
            if (this.Lines == 1)
            {
                Kernel.vMWareSVGAII._DrawACSIIString(text + "_", (uint)Color.White.ToArgb(), x, y);
            }
            else
            {
                Kernel.vMWareSVGAII._DrawACSIIString(_text + "_", (uint)Color.White.ToArgb(), x, y);
            }
        }
    }
}
