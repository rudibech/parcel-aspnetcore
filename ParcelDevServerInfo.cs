namespace RudiBech.AspNetCore.SpaServices.Parcel
{
    public static partial class ParcelDevMiddelware
    {
        class ParcelDevServerInfo
        {
            public int Port { get; set; }
            public string[] PublicPaths { get; set; }
        }
    }
}