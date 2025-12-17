using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using TransducerAppMaui.Logs;
using TransducerAppMaui.Helpers;




namespace TransducerAppMaui.Helpers
{
    // LOG
    public class LogEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Message { get; set; }
    }

    // RESULTADO
    public class ResultEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Timestamp em UTC
        public DateTime TimestampUtc { get; set; }

        public decimal Torque { get; set; }
        public decimal Angle { get; set; }

        // Texto legível salvo (linha que aparece na lista da tela)
        public string Text { get; set; }
    }

    // Trace point model
    public class TracePointEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // corresponde ao ResultEntry.Id
        public int ResultId { get; set; }

        // índice do ponto na curva (ordem)
        public int PointIndex { get; set; }

        // tempo relativo (ms)
        public double TimeMs { get; set; }

        public double Torque { get; set; }

        public double Angle { get; set; }
    }

    // ===== HELPER DO SQLITE =====
    public class DbHelper : IDisposable
    {
        readonly SQLiteConnection _conn;
        readonly object _locker = new object();

        /// <summary>
        /// Helper de acesso ao banco SQLite do app.
        /// </summary>
        /// <param name="dbFileName">Nome do arquivo de banco. Padrão: transducer.db3</param>
        public DbHelper(string dbFileName = "transducer.db3")
        {
            // Caminho do db local no app (pasta privada)
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                dbFileName);

            // FullMutex para uso thread-safe
            _conn = new SQLiteConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite |
                SQLiteOpenFlags.Create |
                SQLiteOpenFlags.FullMutex);

            // Garante criação das tabelas
            _conn.CreateTable<LogEntry>();
            _conn.CreateTable<ResultEntry>();
            _conn.CreateTable<TracePointEntry>(); // cria tabela de trace points
        }

        // ===================== LOGS =====================

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

        public void ClearAllLogs()
        {
            lock (_locker)
            {
                _conn.DeleteAll<LogEntry>();
            }
        }

        // =================== RESULTADOS ===================

        public void InsertResult(ResultEntry e)
        {
            if (e == null) return;
            lock (_locker)
            {
                _conn.Insert(e);
                // sqlite-net preenche e.Id automaticamente após Insert (AutoIncrement)
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

        /// <summary>
        /// Retorna TODOS os resultados, em ordem crescente de data.
        /// Usado para exportação (CSV etc).
        /// </summary>
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

        // =================== TRACEPOINTS ===================

        // Insere uma lista de pontos associados a um resultId (em uma transação)
        public void InsertTracePoints(int resultId, List<TracePointEntry> points)
        {
            if (points == null || points.Count == 0) return;

            lock (_locker)
            {
                // Uso de RunInTransaction garante atomicidade e é a forma recomendada com sqlite-net
                _conn.RunInTransaction(() =>
                {
                    int idx = 0;
                    foreach (var p in points)
                    {
                        // garante que ResultId e PointIndex estejam corretos
                        p.ResultId = resultId;
                        p.PointIndex = idx++;
                        _conn.Insert(p);
                    }
                });
            }
        }

        // Retorna pontos ordenados por PointIndex para um result
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

        // Conta pontos (útil para evitar duplicação)
        public int CountTracePointsForResult(int resultId)
        {
            lock (_locker)
            {
                return _conn.Table<TracePointEntry>().Count(tp => tp.ResultId == resultId);
            }
        }

        // ==================== DISPOSE ====================

        public void Dispose()
        {
            try
            {
                _conn?.Close();
                _conn?.Dispose();
            }
            catch
            {
                // ignora erros de dispose
            }
        }
    }
}