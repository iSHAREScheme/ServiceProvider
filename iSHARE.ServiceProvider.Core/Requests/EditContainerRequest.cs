namespace iSHARE.ServiceProvider.Core.Requests
{
    public class EditContainerRequest
    {

        public string ContainerId { get; set; }
        public ContainerEdit ContainerData { get; set; }
    }
}
