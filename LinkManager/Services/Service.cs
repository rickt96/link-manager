namespace LinkManager.Services
{
    class Service
    {
        protected DatabaseContext _context;

        /// <summary>
        /// istanzia una istanza del context caricando il file specificato in app.config
        /// </summary>
        public Service()
        {
            string path = new ConfigManager().GetKey("FileName");

            _context = new DatabaseContext(path);
            _context.Database.EnsureCreated();
        }
    }
}
