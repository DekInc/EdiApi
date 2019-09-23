using ComModels.Models.EdiDB;

namespace EdiViewer.Models {
    public class IenetGroupsModel : IenetGroups
    {
        public int recid { get { return Id; } }
    }
}
