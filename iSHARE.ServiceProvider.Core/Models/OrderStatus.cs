namespace iSHARE.ServiceProvider.Core.Models
{
    public enum OrderStatus
    {
        ToDo = 1,
        PreAssembly = 2,
        Assembled = 3,
        ReadyForPickup = 4,
        Picking = 5,
        Packed = 6,
        Completed = 7
    }
}