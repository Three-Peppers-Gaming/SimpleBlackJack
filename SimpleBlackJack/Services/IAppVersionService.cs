using SimpleBlackJack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleBlackJack.Services
{
    public interface IAppVersionService
    {
        public string Version { get; }
    }

}


