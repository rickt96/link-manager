using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkManager.Services
{
    class Service
    {
        protected DatabaseContext _context;

        public Service()
        {
            string path = new ConfigManager().GetKey("FileName");
            _context = new DatabaseContext(path);
            _context.Database.EnsureCreated();
        }
    }
}
