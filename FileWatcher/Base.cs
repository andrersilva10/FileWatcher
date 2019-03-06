using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    abstract class Base
    {
        public int? Id { get; set; }

        public Base()
        {

        }
        public Base(int id)
        {
            Id = id;
        }
    }
}
