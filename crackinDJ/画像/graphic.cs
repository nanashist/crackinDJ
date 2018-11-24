using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

public class graphic
{
    public static int  bmp2grahic(Bitmap bmp)
    {
        int handle = 0;
        // 動的に作成したBitmapからグラフィックを作成
        using (MemoryStream ms = new MemoryStream())
        {
            bmp.Save(ms, ImageFormat.Bmp);
            byte[] buff = ms.ToArray();

            unsafe
            {
                fixed (byte* p = buff)
                {
                    DX.SetDrawValidGraphCreateFlag(DX.TRUE);
                    DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.TRUE);

                    handle = DX.CreateGraphFromMem((System.IntPtr)p, buff.Length);

                    DX.SetDrawValidGraphCreateFlag(DX.FALSE);
                    DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.FALSE);
                }
            }
        }
        return handle;
    }

}
