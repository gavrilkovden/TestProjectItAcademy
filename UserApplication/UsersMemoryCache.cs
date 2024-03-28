using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApplication
{
    public class UsersMemoryCache
    {
        public MemoryCache Cache  { get; } = new MemoryCache(new MemoryCacheOptions {SizeLimit = 1024 });

    }
}

