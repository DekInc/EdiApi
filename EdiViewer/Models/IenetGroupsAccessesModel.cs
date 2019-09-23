using ComModels.Models.EdiDB;

namespace EdiViewer.Models {
    public class IenetGroupsAccessesModel : IenetGroupsAccesses
    {
        public int recid { get { return Id; } }
        public string Group { get; set; }
        public string Access { get; set; }
    }
}
