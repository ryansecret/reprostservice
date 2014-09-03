using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WorkQualityReport.Utility
{
    public class DeepCopy
    {
        public static T DeepCopyEntity<T>(T entity) where T:class 
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream stream=new MemoryStream())
            {
                serializer.WriteObject(stream,entity);
                stream.Seek(0, SeekOrigin.Begin);
                return serializer.ReadObject(stream) as T;
            }
 
        }
    }
}
