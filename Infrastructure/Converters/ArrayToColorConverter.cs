using PaintGameMVVM.Infrastructure.Converters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PaintGameMVVM.Infrastructure.Converters
{
    internal class ArrayToColorConverter : MultiConverterBase
    {
        public override object Convert(object[] vv, Type t, object p, CultureInfo c)
        {
            string color = "";
            if(vv.Length == 3) 
            {
                foreach(var item in vv) 
                {
                    color += item.ToString()+",";
                    color = color.Remove(color.Last());
                }
            }
            return color;
        }
    }
}
