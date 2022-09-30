using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteMailKit.Entities
{
    public class RegistroEntity
    {
        public Guid Id { get; set; }
        public string Operacao { get; set; }
        public string Cliente { get; set; }
        public string RDVI { get; set; }
        public string DI { get; set; }
        public string StatusProcesso { get; set; }
        public string DataHoraProgramacao { get; set; }
        public string DataHoraLiberacaoDoc { get; set; }
        public string DataProgramacaoEntrega { get; set; }
        public string Especie { get; set; }
        public string PesoBruto { get; set; }
        public string Volumes { get; set; }
        public string LocalCarregamento { get; set; }
        public string LocalDescarga { get; set; }
        public string PlacaVeiculo { get; set; }
        public string Motorista { get; set; }
        public string Situacao { get; set; }
        public string Status { get; set; }
        public string DataHoraProgrLocalColeta { get; set; }
        public string DataHoraChegadaLocalColeta { get; set; }
        public string DataHoraSaidaLocalColeta { get; set; }
        public string DataHoraProgrLocalDescarga { get; set; }
        public string DataHoraChegadaLocalDescarga { get; set; }
        public string DataHoraSaidaLocalDescarga { get; set; }
    }
}
