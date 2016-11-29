using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactsFP.DataView
{
    class ViewUtil
    {
        public static bool IsValidEmailId(string inputEmail)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(inputEmail);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static byte[] ImageToByte(ImageSource img) 
        {
            byte[] b;
            try
            {
                StreamImageSource streamImgSrc = (StreamImageSource)img;
                System.Threading.CancellationToken canToken = System.Threading.CancellationToken.None;
                Task<Stream> task = streamImgSrc.Stream(canToken);
                Stream stream = task.Result;
                BinaryReader br = new BinaryReader(stream);
                b = br.ReadBytes((int)stream.Length);
                return b;
            }
            catch { }
            return null;
        }
    }
}
