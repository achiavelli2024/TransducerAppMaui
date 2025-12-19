using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;

namespace TransducerAppMaui.Helpers
{
    // LOG
    public class LogEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // IMPORTANTE: para performance de "recent logs"
        // criaremos índice nessa coluna ao abrir o DB
        public DateTime TimestampUtc { get; set; }

        public string Message { get; set; }
    }

    // RESULTADO
    public class ResultEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        public decimal Torque { get; set; }
        public decimal Angle { get; set; }

        public string Text { get; set; }
    }

    public class TracePointEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ResultId { get; set; }
        public int PointIndex { get; set; }
        public double TimeMs { get; set; }
        public double Torque { get; set; }
        public double Angle { get; set; }
    }

    public class DbHelper : IDisposable
    {
        readonly SQLiteConnection _conn;
        readonly object _locker = new object();

        public DbHelper(string dbFileName = "transducer.db3")
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                dbFileName);

            _conn = new SQLiteConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create |
                SQLiteOpenFlags.FullMutex);

            // PRAGMAs seguros (melhoram concorrência e performance)
            // WAL ajuda MUITO quando há leituras e gravações concorrentes (logger gravando e UI lendo)
            try { _conn.Execute("PRAGMA journal_mode=WAL;"); } catch { }
            try { _conn.Execute("PRAGMA synchronous=NORMAL;"); } catch { }
            try { _conn.Execute("PRAGMA temp_store=MEMORY;"); } catch { }
            try { _conn.Execute("PRAGMA cache_size=-2000;"); } catch { } // ~2MB cache

            _conn.CreateTable<LogEntry>();
            _conn.CreateTable<ResultEntry>();
            _conn.CreateTable<TracePointEntry>();

            // ✅ Índices (críticos para "ORDER BY TimestampUtc DESC")
            try { _conn.Execute("CREATE INDEX IF NOT EXISTS idx_logs_timestamputc ON LogEntry(TimestampUtc);"); } catch { }
            try { _conn.Execute("CREATE INDEX IF NOT EXISTS idx_logs_id ON LogEntry(Id);"); } catch { }
        }

        public void InsertLog(LogEntry e)
        {
            if (e == null) return;
            lock (_locker)
            {
                _conn.Insert(e);
            }
        }

        public List<LogEntry> GetRecentLogs(int max = 1000)
        {
            lock (_locker)
            {
                return _conn.Table<LogEntry>()
                            .OrderByDescending(x => x.TimestampUtc)
                            .Take(max)
                            .ToList();
            }
        }

        // (Opcional) Ajuda para UI mostrar “quantos logs existem”
        public int CountLogs()
        {
            lock (_locker)
            {
                return _conn.Table<LogEntry>().Count();
            }
        }


        /// <summary>
        /// Paginação eficiente (Xamarin-like) para logs:
        /// - Primeiro page: beforeId = null => pega os últimos (Id DESC).
        /// - Próximo page: beforeId = menor Id já carregado => pega mais antigos.
        /// </summary>
        public List<LogEntry> GetLogsBeforeId(int? beforeId, int take)
        {
            lock (_locker)
            {
                var q = _conn.Table<LogEntry>();

                if (beforeId.HasValue)
                    q = q.Where(x => x.Id < beforeId.Value);

                return q.OrderByDescending(x => x.Id)
                        .Take(take)
                        .ToList();
            }
        }



        public void ClearAllLogs()
        {
            lock (_locker)
            {
                _conn.DeleteAll<LogEntry>();
            }
        }

        public void InsertResult(ResultEntry e)
        {
            if (e == null) return;
            lock (_locker)
            {
                _conn.Insert(e);
            }
        }

        public List<ResultEntry> GetRecentResults(int max = 1000)
        {
            lock (_locker)
            {
                return _conn.Table<ResultEntry>()
                            .OrderByDescending(x => x.TimestampUtc)
                            .Take(max)
                            .ToList();
            }
        }

        public List<ResultEntry> GetAllResults()
        {
            lock (_locker)
            {
                return _conn.Table<ResultEntry>()
                            .OrderBy(x => x.TimestampUtc)
                            .ToList();
            }
        }

        public void ClearAllResults()
        {
            lock (_locker)
            {
                _conn.DeleteAll<ResultEntry>();
            }
        }

        public void InsertTracePoints(int resultId, List<TracePointEntry> points)
        {
            if (points == null || points.Count == 0) return;

            lock (_locker)
            {
                _conn.RunInTransaction(() =>
                {
                    int idx = 0;
                    foreach (var p in points)
                    {
                        p.ResultId = resultId;
                        p.PointIndex = idx++;
                        _conn.Insert(p);
                    }
                });
            }
        }

        public List<TracePointEntry> GetTracePointsForResult(int resultId)
        {
            lock (_locker)
            {
                return _conn.Table<TracePointEntry>()
                            .Where(tp => tp.ResultId == resultId)
                            .OrderBy(tp => tp.PointIndex)
                            .ToList();
            }
        }

        public int CountTracePointsForResult(int resultId)
        {
            lock (_locker)
            {
                return _conn.Table<TracePointEntry>().Count(tp => tp.ResultId == resultId);
            }
        }

        public void Dispose()
        {
            try
            {
                _conn?.Close();
                _conn?.Dispose();
            }
            catch { }
        }
    }
}