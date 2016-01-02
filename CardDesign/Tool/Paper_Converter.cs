using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Paper_Converter : CustomCreationConverter<My_News>
    {
        public override My_News Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
