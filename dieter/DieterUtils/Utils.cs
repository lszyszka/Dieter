using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace dieter.DieterUtils
{
    class Utils
    {
        public static int GetIdFromUGrid(UniformGrid uniformGrid)
        {
            TextBlock tb = (TextBlock)uniformGrid.FindName("IdTBlock");
            return Int32.Parse(tb.Text);
        }
    }
}
