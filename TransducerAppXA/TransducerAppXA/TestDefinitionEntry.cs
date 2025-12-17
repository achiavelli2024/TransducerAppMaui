using System;
using SQLite;
using Newtonsoft.Json;

namespace TransducerAppXA
{
    // Entidade que representa um TestDefinition salvo no DB Android
    [Table("TestDefinitions")]
    public class TestDefinitionEntry
    {
        // Usamos TestId (string) como chave primária (vinda do servidor)
        [PrimaryKey]
        public string TestId { get; set; }

        public string Name { get; set; }

        public double NominalTorque { get; set; }
        public double MinTorque { get; set; }
        public double MaxTorque { get; set; }

        public int Repetitions { get; set; }

        // campos raw / notes / created_by / timestamp
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public string TimestampUtc { get; set; }

        // guarda o JSON em texto tal qual veio (útil para inspeção/debug)
        public string RawJson { get; set; }

        // Metadata local
        public DateTime ReceivedAtUtc { get; set; }

        public override string ToString()
        {
            // texto amigável para exibir no ListView
            var displayName = string.IsNullOrEmpty(Name) ? $"Teste {TimestampUtc}" : Name;
            return $"{displayName} - Nom:{NominalTorque:F2} Min:{MinTorque:F2} Max:{MaxTorque:F2}";
        }
    }
}