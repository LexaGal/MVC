using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Algorithm.DomainModels
{
    public class ResultInfo
    {
        public ResultInfo(){}

        [Key]
        public int Id { get; set; }

        public string Result { get; set; }
        public int PathCost { get; set; }
        public int ParametersId { get; set; }
        public int DistMatrixId { get; set; }
        public int FlowMatrixId { get; set; }

        public void CopyFrom(ResultInfo resultInfo)
        {
            Result = resultInfo.Result;
            PathCost = resultInfo.PathCost;
            ParametersId = resultInfo.ParametersId;
            DistMatrixId = resultInfo.DistMatrixId;
            FlowMatrixId = resultInfo.FlowMatrixId;
        }
    }
}