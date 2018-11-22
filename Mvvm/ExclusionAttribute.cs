using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Mvvm
{
    /// <summary>
    /// 本ｸﾗｽはBindableBase内でDisposeしたくないﾌﾟﾛﾊﾟﾃｨの属性として使用します。
    /// ﾂﾘｰ構造で親ｲﾝｽﾀﾝｽをﾌﾟﾛﾊﾟﾃｨに持つｸﾗｽ等を想定し、その場合、親ｲﾝｽﾀﾝｽのﾌﾟﾛﾊﾟﾃｨに
    /// 本属性を割り付けるとDisposeしなくなります。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExclusionAttribute : Attribute
    {

    }
}
