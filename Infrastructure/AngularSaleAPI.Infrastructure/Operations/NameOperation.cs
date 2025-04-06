using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Infrastructure.Operations
{
    public static class NameOperation
    {
        public static string CharacterRegulatory(string name)
        {
            string source = @"îığüşöçĞÜŞİÖÇâß\!'^+%&/()=?_@€¨~,æÆ;:<>|. ";
            string destination = @"iigusocGUSIOC                           --";

            for (int i = 0; i < source.Length; i++)
            {
                name = name.Replace(source[i], destination[i]);
            }
            return name;
        } 
    }
}
