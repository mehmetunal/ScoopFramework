using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoopFramework.SessionContainer.Model
{
    public class SessionModel<T> 
    {
        public string Key { get; set; }
        public List<T> Value { get; set; }
    }
}
