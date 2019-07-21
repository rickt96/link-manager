using Microsoft.EntityFrameworkCore;        //da aggiungere
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    //da aggiungere
using System.ComponentModel.DataAnnotations.Schema; //da aggiungere
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkManager
{
    public class DatabaseContext : DbContext
    {
        string dbPath;

        public DatabaseContext(string dbPath="db.sqlite")
        {
            this.dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source="+dbPath);
            
        }

        public DbSet<Link> Links { get; set; }
        public DbSet<Categoria> Categorie { get; set; }
    }

    public class Link
    {
        [Key]
        public int IdLink { get; set; }
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public string URL { get; set; }

        public int? IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }

    }

    public class Categoria
    {
        [Key]
        public int? IdCategoria { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }

        public ICollection<Link> Links { get; set; }
    }
}
