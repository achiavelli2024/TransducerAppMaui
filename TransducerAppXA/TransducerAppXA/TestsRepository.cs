using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace TransducerAppXA
{
    // Repositório simples (síncrono) usando sqlite-net (SQLiteConnection)
    public class TestsRepository : IDisposable
    {
        readonly string _dbPath;
        SQLiteConnection _conn;
        bool _initialized = false;

        public TestsRepository(string dbPath)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));
        }

        public void Initialize()
        {
            if (_initialized) return;
            _conn = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            _conn.CreateTable<TestDefinitionEntry>();
            _initialized = true;
        }

        public void InsertOrReplace(TestDefinitionEntry entry)
        {
            if (!_initialized) Initialize();
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            // InsertOrReplace is supported by sqlite-net
            _conn.InsertOrReplace(entry);
        }

        public List<TestDefinitionEntry> GetAll()
        {
            if (!_initialized) Initialize();
            return _conn.Table<TestDefinitionEntry>().OrderByDescending(t => t.ReceivedAtUtc).ToList();
        }

        public void Delete(string testId)
        {
            if (!_initialized) Initialize();
            _conn.Delete<TestDefinitionEntry>(testId);
        }

        public void Dispose()
        {
            try { _conn?.Close(); } catch { }
            try { _conn?.Dispose(); } catch { }
            _conn = null;
            _initialized = false;
        }
    }
}