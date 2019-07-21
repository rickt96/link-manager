using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkManager.Services
{
    class CategorieService : IService<Categoria>
    {
        private DatabaseContext _context;

        public CategorieService()
        {
            _context = new DatabaseContext();
            _context.Database.EnsureCreated();

        }

        public Categoria Add(Categoria element)
        {
            _context.Categorie.Add(element);
            _context.SaveChanges();

            return element;
        }

        public Categoria Delete(int id)
        {
            Categoria d = _context.Categorie.Include("Links").First(x => x.IdCategoria == id);
            _context.Links.RemoveRange(d.Links);

            _context.Categorie.Remove(d);
            _context.SaveChanges();

            return d;
        }

        public Categoria Edit(Categoria element)
        {
            Categoria entity = _context.Categorie.FirstOrDefault(i => i.IdCategoria == element.IdCategoria);

            entity.Nome = element.Nome;
            entity.Descrizione = element.Descrizione;
            _context.Categorie.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public List<Categoria> GetAll()
        {
            return _context.Categorie.Include("Links").ToList();
        }

        public Categoria GetById(int id)
        {
            Categoria d = _context.Categorie.FirstOrDefault(x => x.IdCategoria == id);
            return d;
        }

        public List<Categoria> Search(string pattern)
        {
            return _context.Categorie.Where(c => c.Nome.Contains(pattern) || c.Descrizione.Contains(pattern)).Include("Links").ToList();
        }
    }
}
