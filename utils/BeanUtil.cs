using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.utils
{
    internal class BeanUtil
    {

        public  static void CopyProperties<TBase, TDerived>(TBase source, TDerived destination)
        {
            // 获取所有父类的属性
            PropertyInfo[] baseProperties = typeof(TBase).GetProperties();

            // 遍历每个属性并赋值给子类
            foreach (PropertyInfo property in baseProperties)
            {
                // 找到子类中对应的属性
                PropertyInfo derivedProperty = typeof(TDerived).GetProperty(property.Name);

                // 如果找到了，则赋值
                if (derivedProperty != null && derivedProperty.CanWrite)
                {
                    object value = property.GetValue(source);
                    derivedProperty.SetValue(destination, value);
                }
            }
        }
    }
}
