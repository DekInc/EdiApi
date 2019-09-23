using ComModels.Models.EdiDB;

namespace EdiViewer.Models {
    public class IenetAccessesModel : IenetAccesses
    {
        public int recid { get { return Id; } }
    }
}
