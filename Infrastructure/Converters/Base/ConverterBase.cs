using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PaintGameMVVM.Infrastructure.Converters.Base
{
    abstract class ConverterBase : IValueConverter
    {
        public abstract object Convert(object v, Type t, object p, CultureInfo c);

        public virtual object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new Exception("Не переопределен метод конвертера");
    }
}
