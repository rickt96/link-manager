using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkManager.Services
{
    class LinksService : Service, IService<Link>
    {
        

        public Link Add(Link element)
        {
            _context.Links.Add(element);
            _context.SaveChanges();

            return element;
        }

        public Link Delete(int id)
        {
            Link d = _context.Links.First(x => x.IdLink == id);
            _context.Links.Remove(d);
            _context.SaveChanges();

            return d;
        }

        public Link Edit(Link element)
        {
            Link entity = _context.Links.FirstOrDefault(i => i.IdLink == element.IdLink);

            entity.Titolo = element.Titolo;
            entity.Descrizione = element.Descrizione;
            entity.URL = element.URL;
            entity.IdCategoria = element.IdCategoria;
            _context.Links.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public List<Link> GetAll()
        {
            return _context.Links.ToList();
        }

        public Link GetById(int id)
        {
            Link d = _context.Links.FirstOrDefault(x => x.IdLink == id);
            return d;
        }

        public List<Link> GetAllByIdCategoria(int IdCategoria)
        {
            return _context.Links.Where(l => l.IdCategoria == IdCategoria).ToList();
        }
    }
}
