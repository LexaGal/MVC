using System.ComponentModel.DataAnnotations;

namespace Algorithm.DomainModels
{
    public class ResultInfo
    {
        [Key]
        public int Id { get; set; }
        public string Result { get; set; }
        public int PathCost { get; set; }
        public int ParametersId { get; set; }
        public int DistMatrixId { get; set; }
        public int FlowMatrixId { get; set; }
    }
}