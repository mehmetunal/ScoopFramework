using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.Interface
{
    public interface IJavaScriptSerializer
    {
        string Serialize(object value);
    }
}
