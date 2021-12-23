namespace ClickTrack.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ClickTrack.Model;
    using LiteDB;
    using Microsoft.Extensions.Logging;

    public interface IClickRepository
    {
        void Add(string url);

        IEnumerable<Click> GetAll();

        IEnumerable<Click> GetByPage(int entriesPerPage, int pageNumber);

        Click GetByUrl(string url);
    }

    public class ClickRepository : IClickRepository, IDisposable
    {
        private static readonly string ClickDatabase = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "clicks.db");
        private const string CollectionName = "clicks";

        private readonly ILogger m_logger;
        private readonly LiteDatabase m_database;

        public ClickRepository(ILogger<ClickRepository> logger)
        {
            m_logger = logger;
            m_database = new LiteDatabase(new ConnectionString { Filename = ClickDatabase });

            // add index over urls
            ILiteCollection<Click> collection = m_database.GetCollection<Click>(CollectionName);
            collection.EnsureIndex(c => c.Url);

            m_logger.LogInformation($"Click database initialized at {ClickDatabase}");
        }

        public void Add(string url)
        {
            Click click = GetByUrl(url);

            ILiteCollection<Click> collection = m_database.GetCollection<Click>(CollectionName);
            if (click != null)
            {
                click.Clicks++;
                collection.Update(click);
            } else
            {
                Click newClick = new Click
                {
                    Url = url,
                    Clicks = 1,
                    Created = DateTime.Now
                };

                m_logger.LogInformation($"Click for address {url} detected for the first time");
                collection.Insert(newClick);
            }
        }

        public IEnumerable<Click> GetAll()
        {
            ILiteCollection<Click> collection = m_database.GetCollection<Click>(CollectionName);
            return collection.FindAll();
        }


        public IEnumerable<Click> GetByPage(int entriesPerPage, int pageNumber)
        {
            ILiteCollection<Click> collection = m_database.GetCollection<Click>(CollectionName);
            ILiteQueryable<Click> query = collection.Query();
            return query.Limit(entriesPerPage).Offset((pageNumber - 1) * entriesPerPage).ToEnumerable();
        }

        public Click GetByUrl(string url)
        {
            ILiteCollection<Click> collection = m_database.GetCollection<Click>(CollectionName);
            return collection.Find(c => c.Url == url).FirstOrDefault();
        }

        #region IDisposable

        private bool m_disposed;

        public void Dispose()
        {
            if (m_disposed)
            {
                return;
            }

            m_database.Dispose();

            m_logger.LogInformation("Click repository finalized");

            GC.SuppressFinalize(this);

            m_disposed = true;
        }

        #endregion IDisposable
    }
}
