using eshop_productapi.Business.Enums.General;

namespace eshop_productapi.Business.ViewModels.General
{
    public class DropMessageModel
    {
        public string Message { get; set; }

        public string Description { get; set; }

        public DropMessageType DropMessageType { get; set; }
    }
}