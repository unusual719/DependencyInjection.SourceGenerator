using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    [Option(sectionKey: "Test2")]
    public record Test2Options
    {
        public int Age { get; set; }
    }
}