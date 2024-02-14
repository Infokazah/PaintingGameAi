using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PaintGameMVVM.Infrastructure.Converters.Base
{
    internal abstract class MultiConverterBase : IMultiValueConverter
    {
        public abstract object Convert(object[] vv, Type t, object p, CultureInfo c);
        public virtual object[] ConvertBack(object v, Type[] tt, object p, CultureInfo c)=> throw new NotImplementedException("Обратное преобразование не поддерживается");
    }
}
