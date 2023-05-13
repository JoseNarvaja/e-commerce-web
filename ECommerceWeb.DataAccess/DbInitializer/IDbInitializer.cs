using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceWeb.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
