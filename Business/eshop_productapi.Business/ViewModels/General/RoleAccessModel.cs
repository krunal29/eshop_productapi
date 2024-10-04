using eshop_productapi.Business.Enums.General;

namespace eshop_productapi.Business.ViewModels.General
{
    public class RoleAccessModel
    {
        public RoleAccessModel(ModuleEnum module, AccessTypeEnum accessType)
        {
            Module = module;
            AccessType = accessType;
        }

        public ModuleEnum Module { get; set; }

        public AccessTypeEnum AccessType { get; set; }
    }
}